// -----------------------------------------------------------------------
// <copyright file="BaseDomainEvent.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Kernel
{
	using System;

	public abstract class BaseDomainEvent
	{
		public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
	}
}