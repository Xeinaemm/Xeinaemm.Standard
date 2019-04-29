// -----------------------------------------------------------------------
// <copyright file="RemoveRuntimeNode.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Sdk
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Build.Utilities;

    public class RemoveRuntimeNode : Task
    {
        public string ConfigFilePath { get; set; }

        public override bool Execute()
        {
            var doc = XDocument.Load(this.ConfigFilePath);

            if (doc == null)
            {
                return false;
            }

            doc.Root.Nodes().OfType<XElement>().FirstOrDefault(e => e.Name.LocalName == "runtime")?.Remove();

            using (var stream = new StreamWriter(this.ConfigFilePath))
            {
                doc.Save(stream);
            }

            return true;
        }
    }
}
