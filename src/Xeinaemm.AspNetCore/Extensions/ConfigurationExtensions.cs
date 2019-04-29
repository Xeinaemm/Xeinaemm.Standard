// -----------------------------------------------------------------------
// <copyright file="ConfigurationExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfiguration config) =>
            config.GetConnectionString("Connection");

        public static string GetIdPAuthorityString(this IConfiguration config) =>
            config.GetSection("Security")["AuthorityIdP"];

        public static string GetSecurityString(this IConfiguration config) =>
            config.GetSection("Security")["SecurityKey"];

        public static string GetAuthorityString(this IConfiguration config) =>
            config.GetSection("Security")["Authority"];
    }
}
