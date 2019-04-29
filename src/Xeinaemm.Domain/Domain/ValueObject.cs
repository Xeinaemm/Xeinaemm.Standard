// -----------------------------------------------------------------------
// <copyright file="ValueObject.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// source: https://github.com/jhewlett/ValueObject.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<FieldInfo> fields;
        private List<PropertyInfo> properties;

        public static bool operator !=(ValueObject obj1, ValueObject obj2) => !(obj1 == obj2);

        public static bool operator ==(ValueObject obj1, ValueObject obj2) => obj1?.Equals(obj2) ?? Equals(obj2, null);

        public bool Equals(ValueObject other) => this.Equals(other as object);

        public override bool Equals(object obj) => obj != null && this.GetType() == obj.GetType()
                   && this.GetProperties().All(p => this.PropertiesAreEqual(obj, p))
                   && this.GetFields().All(f => this.FieldsAreEqual(obj, f));

        public override int GetHashCode()
        {
            var hash = this.GetProperties().Select(prop => prop.GetValue(this, null)).Aggregate(17, HashValue);

            return this.GetFields().Select(field => field.GetValue(this)).Aggregate(hash, HashValue);
        }

        private static int HashValue(int seed, object value) => (seed * 23) + value?.GetHashCode() ?? 0;

        private bool FieldsAreEqual(object obj, FieldInfo f) => Equals(f.GetValue(this), f.GetValue(obj));

        private IEnumerable<FieldInfo> GetFields() =>
            this.fields ?? (this.fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToList());

        private IEnumerable<PropertyInfo> GetProperties() => this.properties ?? (this.properties = this.GetType()
                                                                 .GetProperties(
                                                                     BindingFlags.Instance | BindingFlags.Public)
                                                                 .Where(p =>
                                                                     p.GetCustomAttribute(
                                                                         typeof(IgnoreMemberAttribute)) == null)
                                                                 .ToList());

        private bool PropertiesAreEqual(object obj, PropertyInfo p) =>
            Equals(p.GetValue(this, null), p.GetValue(obj, null));
    }
}