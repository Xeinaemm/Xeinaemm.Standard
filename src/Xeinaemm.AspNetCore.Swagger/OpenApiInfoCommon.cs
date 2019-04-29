// -----------------------------------------------------------------------
// <copyright file="OpenApiInfoCommon.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Swagger
{
    using System;
    using Microsoft.OpenApi.Models;

    public static class OpenApiInfoCommon
    {
        public static OpenApiInfo Create(string title, string description) =>
            new OpenApiInfo
            {
                Title = title,
                Description = description,
                Contact = new OpenApiContact
                {
                    Email = "",
                    Name = "",
                    Url = new Uri(""),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                },
            };
    }
}
