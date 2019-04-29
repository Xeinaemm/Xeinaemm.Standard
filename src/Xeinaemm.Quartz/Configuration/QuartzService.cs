// -----------------------------------------------------------------------
// <copyright file="QuartzService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Quartz;
    using global::Quartz.Impl;

    public class QuartzService : IQuartzService
    {
        private readonly string connectionString;

        private IScheduler serverInstance;

        public QuartzService(string connectionString) => this.connectionString = connectionString;

        public IScheduler ServerInstance => this.serverInstance ?? (this.serverInstance = this.RunQuartzServer());

        public async Task ScheduleJobAsync<TJob>(QuartzJob quartzJob)
            where TJob : IJob
        {
            var jobBuilder = JobBuilder.Create<TJob>()
                .WithIdentity(quartzJob.JobKey)
                .StoreDurably(true)
                .RequestRecovery(true);

            if (quartzJob.JobData != null)
            {
                jobBuilder = jobBuilder.UsingJobData(quartzJob.JobData);
            }

            var job = jobBuilder.Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"--> {quartzJob.JobKey.Group} {quartzJob.JobKey.Name}", quartzJob.JobKey.Group)
                .ForJob(job.Key)
                .WithCronSchedule(quartzJob.CronExpression)
                .Build();

            var jobExists = await this.ServerInstance.CheckExists(job.Key).ConfigureAwait(false);
            if (jobExists)
            {
                await this.ServerInstance.DeleteJob(job.Key).ConfigureAwait(false);
            }

            await this.ServerInstance.ScheduleJob(job, trigger).ConfigureAwait(false);
        }

        public async Task TriggerJobOnDemandAsync(JobKey jobKey)
        {
            try
            {
                await this.ServerInstance.TriggerJob(jobKey).ConfigureAwait(false);
            }
            catch
            {
                Debug.WriteLine("Register job and then invoke trigger on demand");
            }
        }

        private IScheduler RunQuartzServer()
        {
            var config = new NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "Task Runner Server",
                ["quartz.scheduler.instanceId"] = "AUTO",
                ["quartz.serializer.type"] = "json",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "10",
                ["quartz.threadPool.threadPriority"] = "Normal",
                ["quartz.jobStore.misfireThreshold"] = "60000",
                ["quartz.jobStore.clustered"] = "true",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "true",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.lockHandler.type"] = "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz",
                ["quartz.dataSource.default.connectionString"] = this.connectionString,
                ["quartz.dataSource.default.provider"] = "SqlServer",
            };

            ISchedulerFactory sf = new StdSchedulerFactory(config);
            var scheduler = sf.GetScheduler().Result;
            scheduler.Start();
            return scheduler;
        }
    }
}