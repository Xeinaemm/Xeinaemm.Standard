// -----------------------------------------------------------------------
// <copyright file="GrantsViewModel.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.Generic;

    public class GrantsViewModel
    {
        public IEnumerable<GrantViewModel> Grants { get; set; }
    }
}