// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.EntityFrameworkCore;

    public interface IRepository<TDbContext>
        where TDbContext : DbContext
    {
        TEntity GetEntity<TEntity>(Guid id)
            where TEntity : BaseEntity;

        bool AddEntity<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        bool DeleteEntity<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        bool ExistsEntity<TEntity>(Guid id)
            where TEntity : BaseEntity;

        bool UpdateEntity<TEntity>(TEntity entity)
            where TEntity : BaseEntity;

        ReadOnlyCollection<TEntity> GetCollection<TEntity>(ISpecification<TEntity> specification)
            where TEntity : BaseEntity;

        TEntity GetFirstOrDefault<TEntity>(ISpecification<TEntity> specification)
            where TEntity : BaseEntity;
    }
}