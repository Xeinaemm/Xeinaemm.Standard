// -----------------------------------------------------------------------
// <copyright file="HateoasDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class HateoasDto
    {
        public HateoasDto(IEnumerable<IDictionary<string, object>> values, IEnumerable<LinkDto> links)
        {
            this.Values = new ReadOnlyCollection<ReadOnlyDictionary<string, object>>((IList<ReadOnlyDictionary<string, object>>)values);
            this.Links = new ReadOnlyCollection<LinkDto>((IList<LinkDto>)links);
        }

        public ReadOnlyCollection<LinkDto> Links { get; }

        public ReadOnlyCollection<ReadOnlyDictionary<string, object>> Values { get; }
    }
}