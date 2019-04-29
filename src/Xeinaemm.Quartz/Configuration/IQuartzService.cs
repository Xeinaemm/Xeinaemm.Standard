// -----------------------------------------------------------------------
// <copyright file="IQuartzService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using System.Threading.Tasks;
    using global::Quartz;

    public interface IQuartzService
    {
        IScheduler ServerInstance { get; }

        Task ScheduleJobAsync<TJob>(QuartzJob quartzJob)
            where TJob : IJob;

        Task TriggerJobOnDemandAsync(JobKey jobKey);
    }
}