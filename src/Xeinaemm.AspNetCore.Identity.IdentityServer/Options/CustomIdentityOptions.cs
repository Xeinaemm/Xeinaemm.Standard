// -----------------------------------------------------------------------
// <copyright file="CustomIdentityOptions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public static class CustomIdentityOptions
    {
        public const string ResponseType = "code id_token";
        public const string Amr = "amr";
        public const string Sid = "sid";
        public const string Idp = "idp";
    }
}
