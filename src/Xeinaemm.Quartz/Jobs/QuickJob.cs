// -----------------------------------------------------------------------
// <copyright file="QuickJob.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Quartz;

    public class QuickJob : JobBase
    {
        public override async Task PreExecuteAsync(IJobExecutionContext context)
        {
            await Task.Delay(10).ConfigureAwait(false);
            Debug.WriteLine("Quick job invoked");
        }
    }
}
