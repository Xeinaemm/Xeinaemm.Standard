// -----------------------------------------------------------------------
// <copyright file="StandardResources.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.Generic;
    using System.Linq;
    using IdentityServer4;
    using IdentityServer4.Models;

    public static class StandardResources
    {
        public static List<IdentityResource> Identity(this IEnumerable<IClientParameters> clients)
        {
            var identityResource = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
            };

            foreach (var client in clients)
            {
                foreach (var resource in client.Resources)
                {
                    identityResource.Add(new IdentityResource(resource.Name, resource.DisplayName, resource.ClaimTypes));
                }
            }

            return identityResource.Distinct().ToList();
        }

        public static List<ApiResource> Api(this IEnumerable<IApiParameters> apis)
        {
            var apiResources = new List<ApiResource>();

            foreach (var api in apis)
            {
                var apiResource = new ApiResource(api.Name, api.DisplayName, api.ClaimTypes)
                {
                    ApiSecrets = { new Secret(api.Secret.Sha256()) },
                };
                apiResources.Add(apiResource);
            }

            return apiResources;
        }

        public static Client Mvc(this IClientParameters clientParameters)
        {
            var client = new Client
            {
                ClientName = clientParameters.DisplayName,
                ClientId = clientParameters.Name,
                AllowedGrantTypes = GrantTypes.Hybrid,
                AccessTokenType = AccessTokenType.Reference,
                AccessTokenLifetime = 120,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris = { $"{clientParameters.Authority}/signin-oidc", },
                PostLogoutRedirectUris = { $"{clientParameters.Authority}/signout-callback-oidc", },
                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                    },
                ClientSecrets =
                    {
                        new Secret(clientParameters.Secret.Sha256()),
                    },
            };

            foreach (var apiName in clientParameters.ApisNames)
            {
                client.AllowedScopes.Add(apiName);
            }

            foreach (var resource in clientParameters.Resources)
            {
                client.AllowedScopes.Add(resource.Name);
            }

            return client;
        }
    }
}
