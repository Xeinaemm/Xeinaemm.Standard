// -----------------------------------------------------------------------
// <copyright file="LoggedOutViewModel.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }

        public string ClientName { get; set; }

        public string SignOutIframeUrl { get; set; }

        public bool AutomaticRedirectAfterSignOut { get; set; } = false;

        public string LogoutId { get; set; }

        public bool TriggerExternalSignout => this.ExternalAuthenticationScheme != null;

        public string ExternalAuthenticationScheme { get; set; }
    }
}