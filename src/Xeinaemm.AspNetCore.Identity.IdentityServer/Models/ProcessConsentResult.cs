// -----------------------------------------------------------------------
// <copyright file="ProcessConsentResult.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    public class ProcessConsentResult
    {
        public bool IsRedirect => this.RedirectUri != null;

        public string RedirectUri { get; set; }

        public string ClientId { get; set; }

        public bool ShowView => this.ViewModel != null;

        public ConsentViewModel ViewModel { get; set; }

        public bool HasValidationError => this.ValidationError != null;

        public string ValidationError { get; set; }
    }
}
