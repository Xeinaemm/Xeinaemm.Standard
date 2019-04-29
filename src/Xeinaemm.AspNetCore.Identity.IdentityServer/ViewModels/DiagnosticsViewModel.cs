// -----------------------------------------------------------------------
// <copyright file="DiagnosticsViewModel.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Identity.IdentityServer
{
    using System.Collections.ObjectModel;
    using System.Text;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Newtonsoft.Json;

    public class DiagnosticsViewModel
    {
        public DiagnosticsViewModel(AuthenticateResult result)
        {
            this.AuthenticateResult = result;

            if (result.Properties.Items.ContainsKey("client_list"))
            {
                var encoded = result.Properties.Items["client_list"];
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);

                this.Clients = JsonConvert.DeserializeObject<Collection<string>>(value);
            }
        }

        public AuthenticateResult AuthenticateResult { get; }

        public Collection<string> Clients { get; } = new Collection<string>();
    }
}