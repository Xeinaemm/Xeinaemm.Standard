// -----------------------------------------------------------------------
// <copyright file="IdentityServerService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class IdentityServerService : IIdentityServerService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IIdentityServerInteractionService identityServerInteraction;
        private readonly IDeviceFlowInteractionService deviceFlowInteraction;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IResourceStore resourceStore;
        private readonly IEventService events;

        public IdentityServerService(
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService identityServerInteraction,
            IDeviceFlowInteractionService deviceFlowInteraction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IResourceStore resourceStore,
            IEventService events)
        {
            this.userManager = userManager;
            this.identityServerInteraction = identityServerInteraction;
            this.deviceFlowInteraction = deviceFlowInteraction;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.resourceStore = resourceStore;
            this.events = events;
        }

        public async Task<IdentityUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            var filtered = new List<Claim>();

            var name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (name != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else
            {
                var first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
               claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }

            var user = new IdentityUser { UserName = Guid.NewGuid().ToString() };
            var identityResult = await this.userManager.CreateAsync(user).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            if (filtered.Count > 0)
            {
                identityResult = await this.userManager.AddClaimsAsync(user, filtered).ConfigureAwait(false);
                if (!identityResult.Succeeded)
                {
                    throw new Exception(identityResult.Errors.First().Description);
                }
            }

            identityResult = await this.userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider)).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            return user;
        }

        public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId, ClaimsPrincipal user, HttpContext httpContext)
        {
            var logout = await this.identityServerInteraction.GetLogoutContextAsync(logoutId).ConfigureAwait(false);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId,
            };

            if (user?.Identity.IsAuthenticated == true)
            {
                var idp = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await httpContext.GetSchemeSupportsSignOutAsync(idp).ConfigureAwait(false);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            vm.LogoutId = await this.identityServerInteraction.CreateLogoutContextAsync().ConfigureAwait(false);
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await this.identityServerInteraction.GetAuthorizationContextAsync(returnUrl).ConfigureAwait(false);
            if (context?.IdP != null)
            {
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                    ExternalProviders = new ExternalProvider[] { new ExternalProvider { AuthenticationScheme = context.IdP } },
                };
            }

            var schemes = await this.schemeProvider.GetAllSchemesAsync().ConfigureAwait(false);

            var providers = schemes
                .Where(x => x.DisplayName != null
                            || x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name,
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await this.clientStore.FindEnabledClientByIdAsync(context.ClientId).ConfigureAwait(false);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions?.Count > 0)
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray(),
            };
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await this.BuildLoginViewModelAsync(model.ReturnUrl).ConfigureAwait(false);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        public async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId, ClaimsPrincipal user)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (user?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await this.identityServerInteraction.GetLogoutContextAsync(logoutId).ConfigureAwait(false);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await this.identityServerInteraction.GetAuthorizationContextAsync(returnUrl).ConfigureAwait(false);
            if (request != null)
            {
                var client = await this.clientStore.FindEnabledClientByIdAsync(request.ClientId).ConfigureAwait(false);
                if (client != null)
                {
                    var resources = await this.resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested).ConfigureAwait(false);
                    if (resources != null && (resources.IdentityResources.Count > 0 || resources.ApiResources.Count > 0))
                    {
                        var vm = new ConsentViewModel
                        {
                            RememberConsent = model?.RememberConsent ?? true,
                            ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                            ReturnUrl = returnUrl,

                            ClientName = client.ClientName ?? client.ClientId,
                            ClientUrl = client.ClientUri,
                            ClientLogoUrl = client.LogoUri,
                            AllowRememberConsent = client.AllowRememberConsent,
                        };

                        vm.IdentityScopes = resources.IdentityResources.Select(x => new ScopeViewModel
                        {
                            Name = x.Name,
                            DisplayName = x.DisplayName,
                            Description = x.Description,
                            Emphasize = x.Emphasize,
                            Required = x.Required,
                            Checked = vm.ScopesConsented.Contains(x.Name) || model == null || x.Required,
                        }).ToArray();
                        vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => new ScopeViewModel
                        {
                            Name = x.Name,
                            DisplayName = x.DisplayName,
                            Description = x.Description,
                            Emphasize = x.Emphasize,
                            Required = x.Required,
                            Checked = vm.ScopesConsented.Contains(x.Name) || model == null || x.Required,
                        }).ToArray();
                        if (resources.OfflineAccess)
                        {
                            vm.ResourceScopes = vm.ResourceScopes.Union(new ScopeViewModel[]
                            {
                                new ScopeViewModel
                                {
                                    Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                                    DisplayName = ConsentOptions.OfflineAccessDisplayName,
                                    Description = ConsentOptions.OfflineAccessDescription,
                                    Emphasize = true,
                                    Checked = vm.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null,
                                },
                            });
                        }

                        return vm;
                    }
                }
            }

            return null;
        }

        public async Task<DeviceAuthorizationViewModel> BuildDeviceAuthorizationViewModelAsync(string userCode, DeviceAuthorizationInputModel model = null)
        {
            var request = await this.deviceFlowInteraction.GetAuthorizationContextAsync(userCode).ConfigureAwait(false);
            if (request != null)
            {
                var client = await this.clientStore.FindEnabledClientByIdAsync(request.ClientId).ConfigureAwait(false);
                if (client != null)
                {
                    var resources = await this.resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested).ConfigureAwait(false);
                    if (resources != null && (resources.IdentityResources.Count > 0 || resources.ApiResources.Count > 0))
                    {
                        var scopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();
                        var vm = new DeviceAuthorizationViewModel
                        {
                            UserCode = userCode,

                            RememberConsent = model?.RememberConsent ?? true,
                            ScopesConsented = scopesConsented,

                            ClientName = client.ClientName ?? client.ClientId,
                            ClientUrl = client.ClientUri,
                            ClientLogoUrl = client.LogoUri,
                            AllowRememberConsent = client.AllowRememberConsent,
                            IdentityScopes = resources.IdentityResources.Select(x => new ScopeViewModel
                            {
                                Name = x.Name,
                                DisplayName = x.DisplayName,
                                Description = x.Description,
                                Emphasize = x.Emphasize,
                                Required = x.Required,
                                Checked = scopesConsented.Contains(x.Name) || model == null || x.Required,
                            }),
                            ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => new ScopeViewModel
                            {
                                Name = x.Name,
                                DisplayName = x.DisplayName,
                                Description = x.Description,
                                Emphasize = x.Emphasize,
                                Required = x.Required,
                                Checked = scopesConsented.Contains(x.Name) || model == null || x.Required,
                            }),
                        };

                        if (resources.OfflineAccess)
                        {
                            vm.ResourceScopes = vm.ResourceScopes.Union(new[]
                            {
                                new ScopeViewModel
                                {
                                    Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                                    DisplayName = ConsentOptions.OfflineAccessDisplayName,
                                    Description = ConsentOptions.OfflineAccessDescription,
                                    Emphasize = true,
                                    Checked = scopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null,
                                },
                            });
                        }

                        return vm;
                    }
                }
            }

            return null;
        }

        public async Task<GrantsViewModel> BuildGrantsViewModelAsync()
        {
            var grants = await this.identityServerInteraction.GetAllUserConsentsAsync().ConfigureAwait(false);

            var list = new List<GrantViewModel>();
            foreach (var grant in grants)
            {
                var client = await this.clientStore.FindClientByIdAsync(grant.ClientId).ConfigureAwait(false);
                if (client != null)
                {
                    var resource = await this.resourceStore.FindResourcesByScopeAsync(grant.Scopes).ConfigureAwait(false);

                    list.Add(new GrantViewModel
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resource.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resource.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                    });
                }
            }

            return new GrantsViewModel { Grants = list };
        }

        public async Task<(IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            var user = await this.userManager.FindByLoginAsync(provider, providerUserId).ConfigureAwait(false);

            return (user, provider, providerUserId, claims);
        }

        public async Task<ProcessConsentResult> ProcessConsentAsync(ConsentInputModel model, ClaimsPrincipal user)
        {
            var result = new ProcessConsentResult();

            var request = await this.identityServerInteraction.GetAuthorizationContextAsync(model.ReturnUrl).ConfigureAwait(false);
            if (request == null)
            {
                return result;
            }

            ConsentResponse grantedConsent = null;

            if (model?.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;
                await this.events.RaiseAsync(new ConsentDeniedEvent(user.GetSubjectId(), request.ClientId, request.ScopesRequested)).ConfigureAwait(false);
            }
            else if (model?.Button == "yes")
            {
                if (model.ScopesConsented?.Any() == true)
                {
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented.ToArray(),
                    };

                    await this.events.RaiseAsync(new ConsentGrantedEvent(user.GetSubjectId(), request.ClientId, request.ScopesRequested, grantedConsent.ScopesConsented, grantedConsent.RememberConsent)).ConfigureAwait(false);
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                await this.identityServerInteraction.GrantConsentAsync(request, grantedConsent).ConfigureAwait(false);

                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.ClientId;
            }
            else
            {
                result.ViewModel = await this.BuildConsentViewModelAsync(model.ReturnUrl, model).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<ProcessConsentResult> ProcessConsentAsync(DeviceAuthorizationInputModel model, ClaimsPrincipal user)
        {
            var result = new ProcessConsentResult();

            var request = await this.deviceFlowInteraction.GetAuthorizationContextAsync(model.UserCode).ConfigureAwait(false);
            if (request == null)
            {
                return result;
            }

            ConsentResponse grantedConsent = null;

            if (model.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;

                await this.events.RaiseAsync(new ConsentDeniedEvent(user.GetSubjectId(), request.ClientId, request.ScopesRequested)).ConfigureAwait(false);
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented?.Any() == true)
                {
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented.ToArray(),
                    };

                    await this.events.RaiseAsync(new ConsentGrantedEvent(user.GetSubjectId(), request.ClientId, request.ScopesRequested, grantedConsent.ScopesConsented, grantedConsent.RememberConsent)).ConfigureAwait(false);
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                await this.deviceFlowInteraction.HandleRequestAsync(model.UserCode, grantedConsent).ConfigureAwait(false);

                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.ClientId;
            }
            else
            {
                result.ViewModel = await this.BuildDeviceAuthorizationViewModelAsync(model.UserCode, model).ConfigureAwait(false);
            }

            return result;
        }

        public void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            var id_token = externalResult.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = nameof(id_token), Value = id_token } });
            }
        }

        public async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl, HttpContext httpContext, IUrlHelper urlHelper, ControllerBase controllerBase, string callbackAsyncName)
        {
            var result = await httpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName).ConfigureAwait(false);
            if (result?.Principal is WindowsPrincipal wp)
            {
                var props = new AuthenticationProperties
                {
                    RedirectUri = urlHelper.Action(callbackAsyncName),
                    Items =
                    {
                        { nameof(returnUrl), returnUrl },
                        { "scheme", AccountOptions.WindowsAuthenticationSchemeName },
                    },
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                if (AccountOptions.IncludeWindowsGroups)
                {
                    var wi = wp.Identity as WindowsIdentity;
                    var groups = wi.Groups.Translate(typeof(NTAccount));
                    var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                    id.AddClaims(roles);
                }

                await httpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props).ConfigureAwait(false);
                return controllerBase.Redirect(props.RedirectUri);
            }

            return controllerBase.Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }
    }
}
