// -----------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }

        Collection<Expression<Func<T, object>>> Includes { get; }

        Collection<string> IncludeStrings { get; }
    }
}
