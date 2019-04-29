// -----------------------------------------------------------------------
// <copyright file="IgnoreMemberAttribute.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;

    /// <summary>
    /// source: https://github.com/jhewlett/ValueObject.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IgnoreMemberAttribute : Attribute
    {
    }
}