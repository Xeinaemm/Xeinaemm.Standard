// -----------------------------------------------------------------------
// <copyright file="ExceptionsJob.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Quartz
{
    using System;
    using System.Threading.Tasks;
    using global::Quartz;

    public class ExceptionsJob : JobBase
    {
        public override Task PreExecuteAsync(IJobExecutionContext context) =>
            throw new Exception();
    }
}