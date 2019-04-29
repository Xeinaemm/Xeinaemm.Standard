// -----------------------------------------------------------------------
// <copyright file="IPropertyMappingService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System.Collections.ObjectModel;
    using Xeinaemm.Domain;

    public interface IPropertyMappingService
    {
        bool ValidMappingExistsFor<TSource, TDestination>(string fields)
            where TSource : IDto
            where TDestination : BaseEntity;

        ReadOnlyDictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
            where TSource : IDto
            where TDestination : BaseEntity;
    }
}