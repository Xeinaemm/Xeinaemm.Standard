// -----------------------------------------------------------------------
// <copyright file="StartupExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        public static void AddCustomInMemoryDbContext<TDbContext>(this IServiceCollection services, string name)
            where TDbContext : DbContext =>
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseInMemoryDatabase(name);
                options.UseInternalServiceProvider(
                    new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider());
            });

        public static void AddCustomDbContext<TDbContext>(this IServiceCollection services, string connectionString)
            where TDbContext : DbContext =>
            services.AddDbContext<TDbContext>(options =>
                options.UseSqlServer(connectionString));
    }
}