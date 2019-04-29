// -----------------------------------------------------------------------
// <copyright file="LinkDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    public sealed class LinkDto
    {
        public LinkDto(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;
        }

        public string Href { get; }

        public string Method { get; }

        public string Rel { get; }
    }
}