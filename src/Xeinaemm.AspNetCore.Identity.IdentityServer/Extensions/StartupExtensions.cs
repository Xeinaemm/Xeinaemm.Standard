// -----------------------------------------------------------------------
// <copyright file="StartupExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Data;

    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddCustomInMemoryIdentityServer<TApplicationUser, TApplicationDbContext>(this IServiceCollection services, string dbName)
            where TApplicationUser : IdentityUser
            where TApplicationDbContext : IdentityDbContext<TApplicationUser>
        {
            services.AddDbContext<TApplicationDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddIdentity<TApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCustomInMemoryDbContext<TApplicationDbContext>(dbName);
            return services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            }).AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseInMemoryDatabase(dbName);
                options.DefaultSchema = "identity";
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseInMemoryDatabase(dbName);
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<TApplicationUser>();
        }

        public static IIdentityServerBuilder AddCustomIdentityServer<TApplicationUser, TApplicationDbContext>(
            this IServiceCollection services,
            string connectionString,
            string migrationsAssembly)
            where TApplicationUser : IdentityUser
            where TApplicationDbContext : IdentityDbContext<TApplicationUser>
        {
            services.AddDbContext<TApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<TApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCustomIISOptions();
            services.AddCustomDbContext<TApplicationDbContext>(connectionString);
            return services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            }).AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                        b.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                options.DefaultSchema = "identity";
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                        b.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<TApplicationUser>();
        }

        public static void AddCustomClientAuthentication(this IServiceCollection services, IClientParameters clientParameters, string idPAuthority) =>
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (options) => options.AccessDeniedPath = "/AccessDenied")
            .AddOpenIdConnect(IdentityServerConstants.ProtocolTypes.OpenIdConnect, options =>
            {
                options.Authority = idPAuthority;
                options.ClientId = clientParameters.Name;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClientSecret = clientParameters.Secret;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ResponseType = CustomIdentityOptions.ResponseType;
                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Address);
                foreach (var apiName in clientParameters.ApisNames)
                {
                    options.Scope.Add(apiName);
                }

                foreach (var resource in clientParameters.Resources)
                {
                    options.Scope.Add(resource.Name);
                }

                options.ClaimActions.Remove(CustomIdentityOptions.Amr);
                options.ClaimActions.DeleteClaim(CustomIdentityOptions.Sid);
                options.ClaimActions.DeleteClaim(CustomIdentityOptions.Idp);
                foreach (var resource in clientParameters.Resources)
                {
                    foreach (var claim in resource.ClaimTypes)
                    {
                        options.ClaimActions.MapUniqueJsonKey(claim, claim);
                    }
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });

        public static void AddCustomApiAuthentication(this IServiceCollection services, IApiParameters apiParameters) =>
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = apiParameters.Authority;
                    options.ApiName = apiParameters.Name;
                    options.ApiSecret = apiParameters.Secret;
                });
    }
}
