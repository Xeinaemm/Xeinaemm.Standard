// -----------------------------------------------------------------------
// <copyright file="IHateoasRepository.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Hateoas
{
    using Microsoft.EntityFrameworkCore;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;

    public interface IHateoasRepository<TDbContext> : IRepository<TDbContext>
        where TDbContext : DbContext
    {
        PagedListCollection<TEntity> GetCollection<TEntity>(ISpecification<TEntity> specification, IParameters parameters)
            where TEntity : BaseEntity;
    }
}
