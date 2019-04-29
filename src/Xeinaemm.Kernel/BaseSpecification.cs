// -----------------------------------------------------------------------
// <copyright file="BaseSpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Kernel
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public abstract class BaseSpecification<T> : ISpecification<T>
	{
		protected BaseSpecification(Expression<Func<T, bool>> criteria) => this.Criteria = criteria;

		public Expression<Func<T, bool>> Criteria { get; }

		public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

		public List<string> IncludeStrings { get; } = new List<string>();

		protected virtual void AddInclude(Expression<Func<T, object>> includeExpression) => this.Includes.Add(includeExpression);

		// string-based includes allow for including children of children, e.g. Basket.Items.Product
		protected virtual void AddInclude(string includeString) => this.IncludeStrings.Add(includeString);
	}
}