// -----------------------------------------------------------------------
// <copyright file="PagedListCollection.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PagedListCollection<T> : List<T>
    {
        public PagedListCollection()
        {
        }

        public PagedListCollection(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var count = source.Count();
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        public int CurrentPage { get; }

        public bool HasNext => this.CurrentPage < this.TotalPages;

        public bool HasPrevious => this.CurrentPage > 1;

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }
    }
}