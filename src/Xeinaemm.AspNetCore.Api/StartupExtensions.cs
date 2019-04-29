// -----------------------------------------------------------------------
// <copyright file="StartupExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Api
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Serialization;
    using MediaType = Xeinaemm.Hateoas.MediaType;

    public static class StartupExtensions
    {
        public static void AddCustomApiControllers(this IServiceCollection services) =>
            services.AddControllers(setupAction =>
                    {
                        setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                        setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                        setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                        setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                        setupAction.Filters.Add(new ProducesDefaultResponseTypeAttribute());
                        setupAction.Filters.Add(new AuthorizeFilter());

                        setupAction.ReturnHttpNotAcceptable = true;

                        setupAction.InputFormatters
                            .OfType<InputFormatter>()
                            .FirstOrDefault()?
                            .SupportedMediaTypes
                            .Add(MediaType.InputFormatterJson);

                        setupAction.OutputFormatters
                            .OfType<OutputFormatter>()
                            .FirstOrDefault()?
                            .SupportedMediaTypes
                            .Add(MediaType.OutputFormatterJson);

                        var jsonOutputFormatter = setupAction.OutputFormatters
                            .OfType<OutputFormatter>().FirstOrDefault();

                        if (jsonOutputFormatter?.SupportedMediaTypes.Contains("text/json") == true)
                        {
                            jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                        }
                    })
                    .AddControllersAsServices()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver());

        public static void AddCustomApiBehavior(this IServiceCollection services) =>
            services.Configure<ApiBehaviorOptions>(
                options => options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext = actionContext as ActionExecutingContext;

                    return actionContext.ModelState.ErrorCount > 0
                    && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count
                        ? new UnprocessableEntityObjectResult(actionContext.ModelState)
                        : (IActionResult)new BadRequestObjectResult(actionContext.ModelState);
                });

        public static void AddCustomVersionedApiExplorer(this IServiceCollection services) =>
            services.AddVersionedApiExplorer(setupAction => setupAction.GroupNameFormat = "'v'VV");

        public static void AddCustomApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });
    }
}