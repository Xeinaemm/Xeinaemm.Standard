// -----------------------------------------------------------------------
// <copyright file="GenerateConfigBindingRedirects.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Xeinaemm.Sdk
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class GenerateConfigBindingRedirects : Task
    {
        public string ConfigFilePath { get; set; }

        public ITaskItem[] SuggestedBindingRedirects { get; set; }

        public override bool Execute()
        {
            if (this.SuggestedBindingRedirects == null || this.SuggestedBindingRedirects.Length == 0)
            {
                return true;
            }

            var doc = XDocument.Load(this.ConfigFilePath);

            if (doc == null)
            {
                return false;
            }

            var runtimeNode = doc.Root.Nodes().OfType<XElement>().FirstOrDefault(e => e.Name.LocalName == "runtime");

            if (runtimeNode == null)
            {
                runtimeNode = new XElement("runtime");
                doc.Root.Add(runtimeNode);
            }
            else
            {
                return false;
            }

            var ns = XNamespace.Get("urn:schemas-microsoft-com:asm.v1");

            var redirectNodes = from redirect in this.ParseSuggestedRedirects()
                                select new XElement(
                                        ns + "dependentAssembly",
                                        new XElement(
                                            ns + "assemblyIdentity",
                                            new XAttribute("name", redirect.Key.Name),
                                            new XAttribute("publicKeyToken", GetPublicKeyToken(redirect.Key.GetPublicKeyToken())),
                                            new XAttribute("culture", string.IsNullOrEmpty(redirect.Key.CultureName) ? "neutral" : redirect.Key.CultureName)),
                                        new XElement(
                                            ns + "bindingRedirect",
                                            new XAttribute("oldVersion", "0.0.0.0-" + redirect.Value),
                                            new XAttribute("newVersion", redirect.Value)));

            var assemblyBinding = new XElement(ns + "assemblyBinding", redirectNodes);

            runtimeNode.Add(assemblyBinding);
            using (var stream = new StreamWriter(this.ConfigFilePath))
            {
                doc.Save(stream);
            }

            return true;
        }

        private static string GetPublicKeyToken(byte[] bytes)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.GetLength(0); i++)
            {
                builder.AppendFormat("{0:x2}", bytes[i]);
            }

            return builder.ToString();
        }

        private IDictionary<AssemblyName, string> ParseSuggestedRedirects()
        {
            var map = new Dictionary<AssemblyName, string>();
            foreach (var redirect in this.SuggestedBindingRedirects)
            {
                try
                {
                    var maxVerStr = redirect.GetMetadata("MaxVersion");
                    var assemblyIdentity = new AssemblyName(redirect.ItemSpec);
                    map.Add(assemblyIdentity, maxVerStr);
                }
                catch
                {
                }
            }

            return map;
        }
    }
}
