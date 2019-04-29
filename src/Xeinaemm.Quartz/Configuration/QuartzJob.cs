// -----------------------------------------------------------------------
// <copyright file="QuartzJob.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using global::Quartz;

    public class QuartzJob
    {
        public JobKey JobKey { get; set; }

        public string CronExpression { get; set; }

        public JobDataMap JobData { get; }
    }
}