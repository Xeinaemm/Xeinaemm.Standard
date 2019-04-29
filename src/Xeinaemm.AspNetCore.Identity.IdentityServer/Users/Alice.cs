// -----------------------------------------------------------------------
// <copyright file="Alice.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.ObjectModel;
    using System.Security.Claims;
    using IdentityModel;
    using IdentityServer4;

    public class Alice : IUser
    {
        public string SubjectId { get; } = "1";

        public string Username { get; } = nameof(Alice);

        public string Password { get; } = "Alice1234!";

        public Collection<Claim> Claims { get; } = new Collection<Claim>
        {
            new Claim(JwtClaimTypes.Name, "Alice Smith"),
            new Claim(JwtClaimTypes.GivenName, nameof(Alice)),
            new Claim(JwtClaimTypes.FamilyName, "Smith"),
            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            new Claim(JwtClaimTypes.Address, "{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
        };
    }
}
