// -----------------------------------------------------------------------
// <copyright file="IIdentityBase.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public interface IIdentityBase
    {
        string Name { get; }

        string DisplayName { get; }

        string Secret { get; }

        string Authority { get; }
    }
}