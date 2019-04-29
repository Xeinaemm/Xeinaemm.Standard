// -----------------------------------------------------------------------
// <copyright file="IIdentitySeedData.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.Generic;
    using IdentityServer4.Models;

    public interface IIdentitySeedData
    {
        IEnumerable<IdentityResource> IdentityResources { get; }

        IEnumerable<ApiResource> ApiResources { get; }

        IEnumerable<Client> Clients { get; }

        IEnumerable<IUser> Users { get; }
    }
}
