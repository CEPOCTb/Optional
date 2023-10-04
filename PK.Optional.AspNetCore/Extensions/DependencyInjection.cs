using Microsoft.AspNetCore.Mvc;
using PK.Optional.AspNetCore.Binding;
using PK.Utils.AspNetCore.Extensions;
using PK.Utils.AspNetCore.Options;

namespace PK.Optional.AspNetCore.Extensions;

public static class DependencyInjection
{

	public static IServiceCollection AddAspNetOptionalSupport(this IServiceCollection services, Action<ExtendableModelMetadataProviderOptions> configureAction = null)
	{
		services.AddExtendableModelMetadataProvider(configureAction);
		services.AddSingleton<OptionalModelMetadataProvider>();
		services.AddOptions<ExtendableModelMetadataProviderOptions>()
			.Configure<OptionalModelMetadataProvider>((options, provider) =>
			{
				if (options.OnAfterCreateTypeDetails == null)
				{
					options.OnAfterCreateTypeDetails = provider.CreateTypeDetails;
				}
				else
				{
					var func = options.OnAfterCreateTypeDetails;
					options.OnAfterCreateTypeDetails = context =>
					{
						provider.CreateTypeDetails(context);
						func(context);
					};
				}

				if (options.OnAfterCreateParameterDetails == null)
				{
					options.OnAfterCreateParameterDetails = provider.CreateParameterDetails;
				}
				else
				{
					var func = options.OnAfterCreateParameterDetails;
					options.OnAfterCreateParameterDetails = context =>
					{
						provider.CreateParameterDetails(context);
						func(context);
					};
				}

				if (options.OnBeforeCreatePropertyDetails == null)
				{
					options.OnBeforeCreatePropertyDetails = provider.CreatePropertyDetails;
				}
				else
				{
					var func = options.OnBeforeCreatePropertyDetails;
					options.OnBeforeCreatePropertyDetails = context =>
					{
						provider.CreatePropertyDetails(context);
						func(context);
					};
				}

				if (options.OnBeforeGetMetadataForConstructor == null)
				{
					options.OnBeforeGetMetadataForConstructor = provider.GetMetadataForConstructor;
				}
				else
				{
					var func = options.OnBeforeGetMetadataForConstructor;
					options.OnBeforeGetMetadataForConstructor = context =>
					{
						provider.GetMetadataForConstructor(context);
						func(context);
					};
				}
			});

		services.Configure<MvcOptions>(
			options =>
			{
				options.ModelBinderProviders.Insert(0, new OptionalModelBinderProvider());
			});

		return services;
	}
}
