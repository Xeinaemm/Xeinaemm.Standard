// -----------------------------------------------------------------------
// <copyright file="ModelBindingContextExtensions.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Api
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class ModelBindingContextExtensions
    {
        public static object[] ConvertItemsToEnumerableType(this ModelBindingContext bindingContext) =>
            bindingContext.InputtedValue().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => TypeDescriptor.GetConverter(bindingContext.ModelType.GetTypeInfo()
                    .GenericTypeArguments[0]).ConvertFromString(x.Trim()))
                .ToArray();

        public static Array CreateArrayOfCertainType(this ModelBindingContext bindingContext) => Array.CreateInstance(
            bindingContext.ModelType.GetTypeInfo()
                .GenericTypeArguments[0], bindingContext.ConvertItemsToEnumerableType().Length);

        public static Task FailedResultAsync(this ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        public static string InputtedValue(this ModelBindingContext bindingContext) => bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .ToString();

        public static Task NullResultAsync(this ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        public static Task SuccessResultAsync(this ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}