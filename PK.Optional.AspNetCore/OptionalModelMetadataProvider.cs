using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using PK.Utils.AspNetCore.Contexts;
using System.Collections.Concurrent;

namespace PK.Optional.AspNetCore;

internal class OptionalModelMetadataProvider
{
    private readonly ConcurrentDictionary<ModelMetadataIdentity, ModelMetadataCacheEntry> _modelMetadataCache = new();

    internal void CreateTypeDetails(CreateTypeDetailsContext context)
    {
        if (context.Key.ModelType.IsGenericType && context.Key.ModelType.GetGenericTypeDefinition() == typeof(Optional<>))
        {
            context.Metadata.Properties = Array.Empty<ModelMetadata>();
        }

        context.Handled = true;
    }

    internal void CreateParameterDetails(CreateParameterDetailsContext context)
    {
        if (context.Key.ModelType.IsGenericType && context.Key.ModelType.GetGenericTypeDefinition() == typeof(Optional<>))
        {
            context.Metadata.Properties = Array.Empty<ModelMetadata>();
        }

        context.Handled = true;
    }

    internal void CreatePropertyDetails(CreatePropertyDetailsContext context)
    {
        if (context.Key.ModelType.IsGenericType && context.Key.ModelType.GetGenericTypeDefinition() == typeof(Optional<>))
        {
            context.Handled = true;
            context.Metadata = Array.Empty<DefaultMetadataDetails>();
        }
    }

    internal void GetMetadataForConstructor(GetMetadataForConstructorContext context)
    {
        if (context.ModelType.IsGenericType && context.ModelType.GetGenericTypeDefinition() == typeof(Optional<>))
        {
            var key = ModelMetadataIdentity.ForConstructor(context.ConstructorInfo, context.ModelType);

            var entry = _modelMetadataCache.GetOrAdd(
                key,
                constructorKey =>
                {
                    var constructor = constructorKey.ConstructorInfo;
                    var parameters = constructor!.GetParameters();
                    var parameterMetadata = new ModelMetadata[parameters.Length];
                    var parameterTypes = new Type[parameters.Length];

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        var parameterDetails = context.Provider.CreateParameterDetailsFunc(ModelMetadataIdentity.ForParameter(parameter));
                        parameterMetadata[i] = context.Provider.CreateModelMetadataFunc(parameterDetails);

                        parameterTypes[i] = parameter.ParameterType;
                    }


                    var constructorDetails = new DefaultMetadataDetails(constructorKey, ModelAttributes.GetAttributesForType(context.ModelType))
                    {
                        BoundConstructorParameters = parameterMetadata,
                        BoundConstructorInvoker = objects => Activator.CreateInstance(context.ModelType, objects)
                    };

                    var metadata = context.Provider.CreateModelMetadataFunc(constructorDetails);

                    return new ModelMetadataCacheEntry(metadata, constructorDetails);
                });


            context.Handled = true;
            context.Metadata = entry.Metadata;
        }
    }

    private readonly struct ModelMetadataCacheEntry
    {
        public ModelMetadataCacheEntry(ModelMetadata metadata, DefaultMetadataDetails details)
        {
            Metadata = metadata;
            Details = details;
        }

        public ModelMetadata Metadata { get; }

        public DefaultMetadataDetails Details { get; }
    }
}