// -----------------------------------------------------------------------
// <copyright file="ContainerConfiguration.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Configuration.Autofac
{
    using System.Collections.Generic;
    using System.Reflection;

    public class ContainerConfiguration
    {
        public List<Assembly> RegisterAssemblies { get; set; } = new List<Assembly>();
    }
}
