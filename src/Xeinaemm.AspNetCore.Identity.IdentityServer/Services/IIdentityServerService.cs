// -----------------------------------------------------------------------
// <copyright file="IIdentityServerService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public interface IIdentityServerService
    {
        Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);

        Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model);

        Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId, ClaimsPrincipal user);

        Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId, ClaimsPrincipal user, HttpContext httpContext);

        Task<ProcessConsentResult> ProcessConsentAsync(ConsentInputModel model, ClaimsPrincipal user);

        Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl, ConsentInputModel model = null);

        Task<ProcessConsentResult> ProcessConsentAsync(DeviceAuthorizationInputModel model, ClaimsPrincipal user);

        Task<DeviceAuthorizationViewModel> BuildDeviceAuthorizationViewModelAsync(string userCode, DeviceAuthorizationInputModel model = null);

        Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl, HttpContext httpContext, IUrlHelper urlHelper, ControllerBase controllerBase, string callbackAsyncName);

        Task<(IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProviderAsync(AuthenticateResult result);

        Task<IdentityUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims);

        void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps);

        Task<GrantsViewModel> BuildGrantsViewModelAsync();
    }
}
