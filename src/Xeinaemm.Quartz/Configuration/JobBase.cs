// -----------------------------------------------------------------------
// <copyright file="JobBase.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Quartz;

    public abstract class JobBase : IJob
    {
        public abstract Task PreExecuteAsync(IJobExecutionContext context);

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await this.PreExecuteAsync(context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
