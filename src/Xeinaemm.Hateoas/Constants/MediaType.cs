// -----------------------------------------------------------------------
// <copyright file="MediaType.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    public static class MediaType
    {
        public const string InputFormatterJson = "application/vnd.xeinaemm.full+json";
        public const string OutputFormatterJson = "application/vnd.xeinaemm.hateoas+json";
        public const string PatchFormatterJson = "application/json-patch+json";
    }
}