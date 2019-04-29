// -----------------------------------------------------------------------
// <copyright file="AutofacConfiguration.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Configuration.Autofac
{
    using System;
    using global::Autofac;
    using global::Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Xeinaemm.AspNetCore.Identity.IdentityServer;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public static class AutofacConfiguration
    {
        public static IServiceProvider InitializeWeb(this IServiceCollection services, ContainerConfiguration config, Action<ContainerBuilder> extendedSetupAction = null) =>
            new AutofacServiceProvider(BaseAutofacSetup(config, setupAction =>
            {
                setupAction.Populate(services);
                extendedSetupAction?.Invoke(setupAction);
            }));

        public static IServiceProvider InitializeApi(this IServiceCollection services, ContainerConfiguration config, Action<ContainerBuilder> extendedSetupAction = null) =>
            new AutofacServiceProvider(BaseAutofacSetup(config, setupAction =>
            {
                setupAction.Populate(services);
                extendedSetupAction?.Invoke(setupAction);
            }));

        public static IServiceProvider InitializeIdp(this IServiceCollection services, ContainerConfiguration config, Action<ContainerBuilder> extendedSetupAction = null) =>
            new AutofacServiceProvider(BaseAutofacSetup(config, setupAction =>
            {
                setupAction.Populate(services);
                extendedSetupAction?.Invoke(setupAction);
            }));

        public static IContainer InitializeTests(ContainerConfiguration config, Action<ContainerBuilder> extendedSetupAction = null)
            => BaseAutofacSetup(config, extendedSetupAction);

        private static IContainer BaseAutofacSetup(ContainerConfiguration config, Action<ContainerBuilder> setupAction = null)
        {
            var builder = new ContainerBuilder();
            setupAction?.Invoke(builder);
            config.RegisterAssemblies.Add(typeof(IDomainEventDispatcher).Assembly);
            config.RegisterAssemblies.Add(typeof(ITypeHelperService).Assembly);
            config.RegisterAssemblies.Add(typeof(IIdentityServerService).Assembly);
            builder.RegisterAssemblyTypes(config.RegisterAssemblies.ToArray()).AsImplementedInterfaces();
            return builder.Build();
        }
    }
}
