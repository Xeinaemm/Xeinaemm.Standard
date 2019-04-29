// -----------------------------------------------------------------------
// <copyright file="AccountOptions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System;
    using Microsoft.AspNetCore.Server.IISIntegration;

    public static class AccountOptions
    {
        public const bool AllowLocalLogin = true;
        public const bool AllowRememberLogin = true;
        public const bool AutomaticRedirectAfterSignOut = false;
        public const bool IncludeWindowsGroups = false;
        public const string InvalidCredentialsErrorMessage = "Invalid username or password";
        public const bool ShowLogoutPrompt = true;

        public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static readonly string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;
    }
}
