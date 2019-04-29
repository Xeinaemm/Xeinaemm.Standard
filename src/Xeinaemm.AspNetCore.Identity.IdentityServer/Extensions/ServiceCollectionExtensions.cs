// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static async Task EnsureIdentitySeedDataAsync<TIdentityUser, TIdentityDbContext>(this IServiceProvider provider, IIdentitySeedData seedData, bool inMemoryDatabase = false)
            where TIdentityUser : IdentityUser
            where TIdentityDbContext : IdentityDbContext<TIdentityUser>
        {
            if (!inMemoryDatabase)
            {
                await provider.GetRequiredService<TIdentityDbContext>().Database.MigrateAsync().ConfigureAwait(false);
                await provider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync().ConfigureAwait(false);
                await provider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync().ConfigureAwait(false);
            }

            var userMgr = provider.GetRequiredService<UserManager<TIdentityUser>>();
            foreach (var user in seedData.Users)
            {
                var usr = await userMgr.FindByNameAsync(user.Username).ConfigureAwait(false);
                if (usr == null)
                {
                    usr = new IdentityUser { UserName = user.Username } as TIdentityUser;
                    var result = await userMgr.CreateAsync(usr, user.Password).ConfigureAwait(false);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    usr = await userMgr.FindByNameAsync(user.Username).ConfigureAwait(false);
                    result = await userMgr.AddClaimsAsync(usr, user.Claims).ConfigureAwait(false);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
            }

            var context = provider.GetRequiredService<ConfigurationDbContext>();
            if (!context.Clients.Any())
            {
                foreach (var client in seedData.Clients)
                {
                    await context.Clients.AddAsync(client.ToEntity()).ConfigureAwait(false);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in seedData.IdentityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity()).ConfigureAwait(false);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in seedData.ApiResources)
                {
                    await context.ApiResources.AddAsync(resource.ToEntity()).ConfigureAwait(false);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
