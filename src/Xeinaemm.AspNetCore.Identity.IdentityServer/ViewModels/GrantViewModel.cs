// -----------------------------------------------------------------------
// <copyright file="GrantViewModel.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System;
    using System.Collections.Generic;

    public class GrantViewModel
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Expires { get; set; }

        public IEnumerable<string> IdentityGrantNames { get; set; }

        public IEnumerable<string> ApiGrantNames { get; set; }
    }
}