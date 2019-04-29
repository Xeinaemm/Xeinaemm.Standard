// -----------------------------------------------------------------------
// <copyright file="BaseSpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria) => this.Criteria = criteria;

        public Expression<Func<T, bool>> Criteria { get; }

        public Collection<Expression<Func<T, object>>> Includes { get; } = new Collection<Expression<Func<T, object>>>();

        public Collection<string> IncludeStrings { get; } = new Collection<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => this.Includes.Add(includeExpression);

        protected virtual void AddInclude(string includeString) => this.IncludeStrings.Add(includeString);
    }
}