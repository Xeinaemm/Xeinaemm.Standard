// -----------------------------------------------------------------------
// <copyright file="AutoMoqDataAttribute.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Tests.Common.Attributes
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Xunit2;

    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        private static readonly Fixture FixtureInstance = new Fixture();

        public AutoMoqDataAttribute()
            : base(() => FixtureInstance.Customize(new AutoMoqCustomization()))
        {
            FixtureInstance.Behaviors.Remove(new ThrowingRecursionBehavior());
            FixtureInstance.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}