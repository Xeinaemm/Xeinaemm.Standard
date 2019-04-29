// -----------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.Generic;
    using System.Linq;

    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;

        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

        public IEnumerable<ExternalProvider> VisibleExternalProviders => this.ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => !this.EnableLocalLogin && this.ExternalProviders?.Count() == 1;

        public string ExternalLoginScheme => this.IsExternalLoginOnly ? this.ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
}