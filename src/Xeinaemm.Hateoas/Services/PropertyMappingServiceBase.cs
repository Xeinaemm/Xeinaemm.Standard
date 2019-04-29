// -----------------------------------------------------------------------
// <copyright file="PropertyMappingServiceBase.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xeinaemm.Domain;

    public abstract class PropertyMappingServiceBase : IPropertyMappingService
    {
        private readonly Collection<PropertyMapping> propertyMappings = new Collection<PropertyMapping>();

        protected PropertyMappingServiceBase(IDictionary<string, PropertyMappingValue> propertyMappingDictionary) =>
            this.propertyMappings.Add(new PropertyMapping(propertyMappingDictionary));

        public ReadOnlyDictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
            where TSource : IDto
            where TDestination : BaseEntity
        {
            if (this.propertyMappings.Count(f => f != null) == 1)
            {
                return new ReadOnlyDictionary<string, PropertyMappingValue>(this.propertyMappings.First(f => f != null).MappingDictionary);
            }

            throw new Exception(
                $"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
            where TSource : IDto
            where TDestination : BaseEntity => string.IsNullOrWhiteSpace(fields)
                || (from field in fields.Split(',')
                    select field.Trim()
                    into trimmedField
                    let indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal)
                    select indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace))
                .All(propertyName => this.GetPropertyMapping<TSource, TDestination>().ContainsKey(propertyName));
    }
}