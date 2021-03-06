﻿// -----------------------------------------------------------------------
// <copyright file="ExternalProvider.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public class ExternalProvider
    {
        public string DisplayName { get; set; }

        public string AuthenticationScheme { get; set; }
    }
}