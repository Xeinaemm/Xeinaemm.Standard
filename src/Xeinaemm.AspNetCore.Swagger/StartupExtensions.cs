// -----------------------------------------------------------------------
// <copyright file="StartupExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using IdentityServer4;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public static class StartupExtensions
    {
        public static void UseCustomSwaggerUI(this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider, string name) =>
            app.UseSwaggerUI(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerEndpoint(
                        $"/swagger/{name}{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                setupAction.RoutePrefix = string.Empty;
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });

        public static void CustomSwaggerDoc(this SwaggerGenOptions swaggerGenOptions, IServiceCollection services, string name, OpenApiInfo info)
        {
            var apiVersionDescriptionProvider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                swaggerGenOptions.SwaggerDoc(
                    $"{name}{description.GroupName}",
                    new OpenApiInfo
                    {
                        Title = info.Title,
                        Version = description.ApiVersion.ToString(),
                        Description = info.Description,
                        Contact = new OpenApiContact
                        {
                            Email = info.Contact.Email,
                            Name = info.Contact.Name,
                            Url = info.Contact.Url,
                        },
                        License = new OpenApiLicense
                        {
                            Name = info.License.Name,
                            Url = info.License.Url,
                        },
                    });
            }
        }

        public static void CustomSecurityDefinition(this SwaggerGenOptions swaggerGenOptions) =>
            swaggerGenOptions.AddSecurityDefinition(IdentityServerConstants.ProtocolTypes.OpenIdConnect, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                Scheme = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                Description = "Open Id Connect authorization, login to your Identity Provider to get access.",
            });

        public static void CustomSecurityRequirement(this SwaggerGenOptions swaggerGenOptions) =>
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                            },
                        }, new List<string>()
                    },
                });

        public static void CustomDocInclusionPredicate(this SwaggerGenOptions swaggerGenOptions, string name) =>
            swaggerGenOptions.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    return actionApiVersionModel == null
                        || (actionApiVersionModel.DeclaredApiVersions.Count > 0
                        ? actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"{name}{v}" == documentName)
                        : actionApiVersionModel.ImplementedApiVersions.Any(v =>
                        $"{name}{v}" == documentName));
                });

        public static void CustomXmlComments<TAssembly>(this SwaggerGenOptions swaggerGenOptions)
        {
            var xmlCommentsFile = $"{Assembly.GetAssembly(typeof(TAssembly)).GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            swaggerGenOptions.IncludeXmlComments(xmlCommentsFullPath);
        }
    }
}