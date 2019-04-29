// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Kernel
{
	using System;
	using System.Collections.Generic;

	public interface IRepository
	{
		TEntity GetEntity<TEntity>(Guid id)
			where TEntity : BaseEntity;

		bool AddEntity<TEntity>(TEntity entity)
			where TEntity : BaseEntity;

		bool DeleteEntity<TEntity>(TEntity entity)
			where TEntity : BaseEntity;

		bool ExistsEntity<TEntity>(Guid entity)
			where TEntity : BaseEntity;

		bool UpdateEntity<TEntity>(TEntity entity)
			where TEntity : BaseEntity;

		List<TEntity> GetCollection<TEntity>(ISpecification<TEntity> specification = null)
			where TEntity : BaseEntity;

		PagedList<TEntity> GetCollection<TEntity, TDto>(ISpecification<TEntity> specification, IParameters parameters)
			where TEntity : BaseEntity
			where TDto : IDto;

		TEntity GetFirstOrDefault<TEntity>(ISpecification<TEntity> specification = null)
			where TEntity : BaseEntity;
	}
}