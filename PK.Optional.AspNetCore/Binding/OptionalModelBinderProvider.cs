using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PK.Optional.AspNetCore.Binding;

internal class OptionalModelBinderProvider : IModelBinderProvider
{
    /// <inheritdoc />
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var metadata = context.Metadata;

        if (metadata.ModelType.IsGenericType && metadata.ModelType.GetGenericTypeDefinition() == typeof(Optional<>))
        {
            var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<OptionalModelBinder>();

            var metadataProvider = (ModelMetadataProvider)context.MetadataProvider;
            var constructorMetadata = metadataProvider.GetMetadataForConstructor(context.Metadata.ModelType.GetConstructor(context.Metadata.ModelType.GetGenericArguments()), context.Metadata.ModelType);
            var constructorParameterMetadata = context.CreateBinder(constructorMetadata.BoundConstructorParameters.First());

            return new OptionalModelBinder(constructorMetadata, constructorParameterMetadata);
        }

        return null;
    }
}
