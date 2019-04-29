// -----------------------------------------------------------------------
// <copyright file="HateoasRepository.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public class HateoasRepository<TDbContext> : Repository<TDbContext>, IHateoasRepository<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IPropertyMappingService propertyMappingService;

        public HateoasRepository(TDbContext dbContext, IPropertyMappingService propertyMappingService)
            : base(dbContext) => this.propertyMappingService = propertyMappingService;

        public PagedListCollection<TEntity> GetCollection<TEntity>(
            ISpecification<TEntity> specification,
            IParameters parameters)
            where TEntity : BaseEntity
        {
            var entities = this.GetCollection(specification)
                .ApplySort(
                    parameters.OrderBy,
                    this.propertyMappingService.GetPropertyMapping<IDto, TEntity>()).ToList();

            return new PagedListCollection<TEntity>(entities, parameters.PageNumber, parameters.PageSize);
        }
    }
}
