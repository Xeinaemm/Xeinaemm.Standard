// -----------------------------------------------------------------------
// <copyright file="IHandle.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Kernel
{
	public interface IHandle<in T>
		where T : BaseDomainEvent
	{
		void Handle(T domainEvent);
	}
}