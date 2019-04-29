// -----------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Kernel
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public interface ISpecification<T>
	{
		Expression<Func<T, bool>> Criteria { get; }

		List<Expression<Func<T, object>>> Includes { get; }

		List<string> IncludeStrings { get; }
	}
}
