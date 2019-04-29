// -----------------------------------------------------------------------
// <copyright file="StartupExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore
{
    using System.Collections.Generic;
    using System.Net;
    using AspNetCoreRateLimit;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        public static void AddCustomHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(
                expirationModelOptions => expirationModelOptions.MaxAge = 600,
                validationModelOptions => validationModelOptions.MustRevalidate = true);

        public static void AddCustomIpRateLimitOptions(this IServiceCollection services, List<RateLimitRule> rules) =>
            services.Configure<IpRateLimitOptions>(options =>
                {
                    options.IpWhitelist = new List<string> { IPAddress.Loopback.ToString() };
                    options.GeneralRules = rules;
                });

        public static void AddCustomCookiePolicy(this IServiceCollection services) =>
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

        public static void AddCustomIISOptions(this IServiceCollection services) =>
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
    }
}