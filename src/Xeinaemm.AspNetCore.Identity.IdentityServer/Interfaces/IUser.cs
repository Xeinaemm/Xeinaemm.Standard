// -----------------------------------------------------------------------
// <copyright file="IUser.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.ObjectModel;
    using System.Security.Claims;

    public interface IUser
    {
        string SubjectId { get; }

        string Username { get; }

        string Password { get; }

        Collection<Claim> Claims { get; }
    }
}
