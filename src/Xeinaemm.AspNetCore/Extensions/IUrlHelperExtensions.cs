// -----------------------------------------------------------------------
// <copyright file="IUrlHelperExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public static class IUrlHelperExtensions
    {
        public static HateoasPagination CreateHateoasPagination<TEntity>(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters parameter,
            PagedListCollection<TEntity> pagedListCollection)
            where TEntity : BaseEntity =>
            new HateoasPagination
            {
                TotalCount = pagedListCollection.TotalCount,
                CurrentPage = pagedListCollection.CurrentPage,
                TotalPages = pagedListCollection.TotalPages,
                PageSize = pagedListCollection.PageSize,
                NextPage = urlHelper.NextPage(routeName, parameter, pagedListCollection),
                PreviousPage = urlHelper.PreviousPage(routeName, parameter, pagedListCollection),
            };

        public static ReadOnlyCollection<LinkDto> CreateLinks<TEntity>(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters parameter,
            PagedListCollection<TEntity> pagedListCollection)
            where TEntity : BaseEntity
        {
            var links = new List<LinkDto>
            {
                urlHelper.CreateResourceUri(routeName, parameter, ResourceUriType.Current)
                    .AddRelAndMethod(Rel.Self, WebRequestMethods.Http.Get),
            };

            if (pagedListCollection.HasNext)
            {
                links.Add(urlHelper.CreateResourceUri(routeName, parameter, ResourceUriType.NextPage)
                    .AddRelAndMethod(Rel.NextPage, WebRequestMethods.Http.Get));
            }

            if (pagedListCollection.HasPrevious)
            {
                links.Add(urlHelper.CreateResourceUri(routeName, parameter, ResourceUriType.PreviousPage)
                    .AddRelAndMethod(Rel.PreviousPage, WebRequestMethods.Http.Get));
            }

            return new ReadOnlyCollection<LinkDto>(links);
        }

        private static string CreatePage(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters resourceParameters,
            int pageNumber = 0)
            => urlHelper.Link(
                routeName,
                (resourceParameters.Fields, resourceParameters.OrderBy, resourceParameters.SearchQuery, resourceParameters.PageNumber + pageNumber, resourceParameters.PageSize));

        private static string CreateResourceUri(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters parameters,
            ResourceUriType type) => type switch
            {
                ResourceUriType.PreviousPage => urlHelper.CreatePage(routeName, parameters, -1),
                ResourceUriType.NextPage => urlHelper.CreatePage(routeName, parameters, 1),
                ResourceUriType.Current => urlHelper.CreatePage(routeName, parameters),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

        private static string NextPage<TEntity>(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters resourceParameters,
            PagedListCollection<TEntity> entities)
            where TEntity : BaseEntity
            => entities.HasNext
                ? urlHelper.CreateResourceUri(routeName, resourceParameters, ResourceUriType.NextPage)
                : null;

        private static string PreviousPage<TEntity>(
            this IUrlHelper urlHelper,
            string routeName,
            IParameters parameter,
            PagedListCollection<TEntity> entities)
            where TEntity : BaseEntity
            => entities.HasPrevious
                ? urlHelper.CreateResourceUri(routeName, parameter, ResourceUriType.PreviousPage)
                : null;
    }
}