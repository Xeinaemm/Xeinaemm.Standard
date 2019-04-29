// -----------------------------------------------------------------------
// <copyright file="BaseEntity.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }

        [NotMapped]
        public Collection<BaseDomainEvent> Events { get; } = new Collection<BaseDomainEvent>();
    }
}