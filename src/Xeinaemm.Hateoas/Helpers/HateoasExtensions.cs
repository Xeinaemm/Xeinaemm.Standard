// -----------------------------------------------------------------------
// <copyright file="HateoasExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;
    using Ardalis.GuardClauses;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;

    public static class HateoasExtensions
    {
        public static LinkDto AddRelAndMethod(
            this string href,
            string relName,
            string methodName) =>
            new LinkDto(href, relName, methodName);

        public static BasePagination CreateBasePagination<TEntity>(this PagedListCollection<TEntity> pagedListCollection)
            where TEntity : BaseEntity =>
            new BasePagination
            {
                TotalCount = pagedListCollection.TotalCount,
                PageSize = pagedListCollection.PageSize,
                CurrentPage = pagedListCollection.CurrentPage,
                TotalPages = pagedListCollection.TotalPages,
            };

        public static ReadOnlyCollection<ExpandoObject> ShapeDataCollection<TSource>(
            this IEnumerable<TSource> source,
            string fields = "")
        {
            Guard.Against.Null(source, nameof(source));

            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, PublicInstances());

                    Guard.Against.Null(propertyInfo, nameof(propertyInfo));

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (var sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataShapedObject);
            }

            return new ReadOnlyCollection<ExpandoObject>(expandoObjectList);
        }

        public static ReadOnlyCollection<ReadOnlyDictionary<string, object>> ShapeDataCollectionWithHateoasLinks(
            this IEnumerable<IDto> dtos,
            string fields,
            Func<Guid, string, IEnumerable<LinkDto>> function)
            => new ReadOnlyCollection<ReadOnlyDictionary<string, object>>(
                (IList<ReadOnlyDictionary<string, object>>)dtos.ShapeDataCollection(fields)
                .Select(dto =>
                {
                    var dictionary = dto as IDictionary<string, object>;
                    dictionary.Add(CommonConstants.KeyLink, function?.Invoke((Guid)dictionary["Id"], fields));

                    return new ReadOnlyDictionary<string, object>(dictionary);
                }));

        public static ExpandoObject ShapeDataObject<TSource>(
            this TSource source,
            string fields = "")
        {
            Guard.Against.Null(source, nameof(source));

            var dataShapedObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource)
                    .GetProperties(PublicInstances());

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                return dataShapedObject;
            }

            foreach (var field in fields.Split(','))
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(TSource)
                    .GetProperty(propertyName, PublicInstances());

                Guard.Against.Null(propertyInfo, nameof(propertyInfo));

                var propertyValue = propertyInfo.GetValue(source);
                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }

        public static ReadOnlyCollection<T> ApplySort<T>(
            this IEnumerable<T> source,
            string orderBy,
            IDictionary<string,
            PropertyMappingValue> mappingDictionary)
        {
            Guard.Against.Null(source, nameof(source));
            Guard.Against.Null(mappingDictionary, nameof(mappingDictionary));

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return new ReadOnlyCollection<T>(source.ToList());
            }

            foreach (var orderByClause in orderBy.Split(',').Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc", StringComparison.Ordinal);

                var propertyName = trimmedOrderByClause.RemoveSuffix();
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                Guard.Against.Null(propertyMappingValue, nameof(propertyMappingValue));

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.AsQueryable().OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending")).ToList();
                }
            }

            return new ReadOnlyCollection<T>(source.ToList());
        }

        public static ReadOnlyDictionary<string, object> ShapeDataWithoutParameters(
            this IBaseEntity entity,
            Func<Guid, string, IEnumerable<LinkDto>> function,
            string fields = "")
        {
            var dictionary = (IDictionary<string, object>)entity.ShapeDataObject(fields);
            dictionary.Add(CommonConstants.KeyLink, function?.Invoke(entity.Id, fields));
            return new ReadOnlyDictionary<string, object>(dictionary);
        }

        private static BindingFlags PublicInstances() =>
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        private static string RemoveSuffix(this string str) => str.IndexOf(" ", StringComparison.Ordinal) == -1
            ? str
            : str.Remove(str.IndexOf(" ", StringComparison.Ordinal));
    }
}