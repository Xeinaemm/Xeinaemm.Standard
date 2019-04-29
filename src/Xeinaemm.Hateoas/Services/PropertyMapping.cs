// -----------------------------------------------------------------------
// <copyright file="PropertyMapping.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class PropertyMapping
    {
        public PropertyMapping(IDictionary<string, PropertyMappingValue> mappingDictionary) =>
            this.MappingDictionary = new ReadOnlyDictionary<string, PropertyMappingValue>(mappingDictionary);

        public ReadOnlyDictionary<string, PropertyMappingValue> MappingDictionary { get; }
    }
}