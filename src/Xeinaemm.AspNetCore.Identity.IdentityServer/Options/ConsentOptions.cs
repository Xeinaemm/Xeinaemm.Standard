// -----------------------------------------------------------------------
// <copyright file="ConsentOptions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public static class ConsentOptions
    {
        public const string OfflineAccessDisplayName = "Offline Access";
        public const string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

        public const string MustChooseOneErrorMessage = "You must pick at least one permission";
        public const string InvalidSelectionErrorMessage = "Invalid selection";
    }
}
