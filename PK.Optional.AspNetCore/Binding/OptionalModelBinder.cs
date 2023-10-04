using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PK.Optional.AspNetCore.Binding;

internal class OptionalModelBinder : IModelBinder
{
    private readonly ModelMetadata _constructorMetadata;
    private readonly IModelBinder _valueBinder;

    public OptionalModelBinder([NotNull] ModelMetadata constructorMetadata, [NotNull] IModelBinder valueBinder)
    {
        _constructorMetadata = constructorMetadata ?? throw new ArgumentNullException(nameof(constructorMetadata));
        _valueBinder = valueBinder ?? throw new ArgumentNullException(nameof(valueBinder));
    }

    /// <inheritdoc />
    public async Task BindModelAsync([NotNull] ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return;
        }

        var modelState = bindingContext.ModelState;
        modelState.SetModelValue(modelName, valueProviderResult);

        var metadata = bindingContext.ModelMetadata;
        var type = metadata.UnderlyingOrModelType;

        try
        {
            var value = valueProviderResult.FirstValue;

            object model;

            if (string.IsNullOrWhiteSpace(value))
            {
                model = Activator.CreateInstance(type);
            }
            else if (type.IsGenericType  && type.GetGenericTypeDefinition() == typeof(Optional<>))
            {
                var parameter = _constructorMetadata.BoundConstructorParameters.First();
                ModelBindingResult result;
                using (bindingContext.EnterNestedScope(
                    modelMetadata: parameter,
                    fieldName: parameter.ParameterName,
                    modelName: modelName,
                    model: null))
                {
                    await _valueBinder.BindModelAsync(bindingContext);
                    result = bindingContext.Result;
                }

                if (!result.IsModelSet)
                {
                    var message = parameter.ModelBindingMessageProvider.MissingBindRequiredValueAccessor(parameter.ParameterName);
                    bindingContext.ModelState.TryAddModelError(modelName, message);
                }

                model = _constructorMetadata.BoundConstructorInvoker(new[] { result.Model });
            }
            else
            {
                throw new NotSupportedException();
            }

            bindingContext.Result = ModelBindingResult.Success(model);

        }
        catch (Exception exception)
        {
            // Conversion failed.
            modelState.TryAddModelError(modelName, exception, metadata);
        }
    }
}
