// -----------------------------------------------------------------------
// <copyright file="HateoasPagination.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using System;

    public sealed class HateoasPagination : BasePagination
    {
        public string NextPage { get; set; }

        public string PreviousPage { get; set; }
    }
}