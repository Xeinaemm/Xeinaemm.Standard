// -----------------------------------------------------------------------
// <copyright file="IBaseEntity.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.ObjectModel;

    public interface IBaseEntity
    {
        Guid Id { get; set; }

        Collection<BaseDomainEvent> Events { get; }
    }
}