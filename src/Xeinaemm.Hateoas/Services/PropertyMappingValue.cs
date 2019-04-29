// -----------------------------------------------------------------------
// <copyright file="PropertyMappingValue.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class PropertyMappingValue
    {
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            this.DestinationProperties = new ReadOnlyCollection<string>((IList<string>)destinationProperties);
            this.Revert = revert;
        }

        public ReadOnlyCollection<string> DestinationProperties { get; }

        public bool Revert { get; }
    }
}