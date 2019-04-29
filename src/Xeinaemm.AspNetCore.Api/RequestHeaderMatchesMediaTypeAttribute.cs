// -----------------------------------------------------------------------
// <copyright file="RequestHeaderMatchesMediaTypeAttribute.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.AspNetCore.Api
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Net.Http.Headers;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly MediaTypeCollection mediaTypes = new MediaTypeCollection();
        private readonly string requestHeaderToMatch;

        public RequestHeaderMatchesMediaTypeAttribute(
            string requestHeaderToMatch,
            string mediaType,
            params string[] otherMediaTypes)
        {
            this.requestHeaderToMatch = requestHeaderToMatch;

            if (MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
            {
                this.mediaTypes.Add(parsedMediaType);
            }

            foreach (var otherMediaType in otherMediaTypes)
            {
                if (MediaTypeHeaderValue.TryParse(otherMediaType, out var parsedOtherMediaType))
                {
                    this.mediaTypes.Add(parsedOtherMediaType);
                }
            }
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context) =>
            this.ContainsRequestHeaderIn(context) && this.MediaTypesMatches(context);

        private static IHeaderDictionary GetRequestHeaders(ActionConstraintContext context) =>
                    context.RouteContext.HttpContext.Request.Headers;

        private bool ContainsRequestHeaderIn(ActionConstraintContext context) =>
            GetRequestHeaders(context).ContainsKey(this.requestHeaderToMatch);

        private bool MediaTypesMatches(ActionConstraintContext context) =>
            this.mediaTypes.Any(mediaType => new MediaType(GetRequestHeaders(context)[this.requestHeaderToMatch]).Equals(new MediaType(mediaType)));
    }
}