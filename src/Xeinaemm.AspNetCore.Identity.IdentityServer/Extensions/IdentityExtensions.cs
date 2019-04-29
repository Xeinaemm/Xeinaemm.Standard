// -----------------------------------------------------------------------
// <copyright file="IdentityExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Threading.Tasks;
    using IdentityServer4.Stores;

    public static class IdentityExtensions
    {
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string clientId)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                var client = await store.FindEnabledClientByIdAsync(clientId).ConfigureAwait(false);
                return client?.RequirePkce == true;
            }

            return false;
        }
    }
}
