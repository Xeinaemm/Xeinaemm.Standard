// -----------------------------------------------------------------------
// <copyright file="IParameters.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    public interface IParameters
    {
        int PageNumber { get; set; }

        int PageSize { get; set; }

        string SearchQuery { get; set; }

        string OrderBy { get; set; }

        string Fields { get; set; }
    }
}