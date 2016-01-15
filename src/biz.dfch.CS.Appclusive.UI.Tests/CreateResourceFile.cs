using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace biz.dfch.CS.Appclusive.UI.Tests
{
    [TestClass]
    public class CreateResourceFile
    {
         [TestCategory("SkipOnTeamCity")]
        [TestMethod]
        public void GenerateTextsForAllProperties()
        {
            try
            {
                string resourceFilePath = @"C:\development\projects\GitHub\biz.dfch.CS.Appclusive.UI\src\biz.dfch.CS.Appclusive.UI\App_LocalResources\GeneralResources.resx";
                XmlDocument resourceDoc = new XmlDocument();
                resourceDoc.Load(resourceFilePath);

                Assembly modelAss = typeof(biz.dfch.CS.Appclusive.UI.Models.Core.Ace).Assembly;

                var types = from t in modelAss.GetTypes()
                            where !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith("biz.dfch.CS.Appclusive.UI.Models")
                            select t;

                foreach (Type t in types)
                {
                    foreach (PropertyInfo prop in t.GetProperties())
                    {
                        try
                        {
                            string keyName = prop.Name;
                            if (keyName != keyName.ToLower())
                            {
                                if ((new string[] { "Value", "Key" }).Contains(keyName))
                                {
                                    keyName += "Display";
                                }
                                string xPath = string.Format("/root/data[@name='{0}']", keyName);
                                if (resourceDoc.SelectSingleNode(xPath) == null)
                                {
                                    // <data name="ExpiresAt" xml:space="preserve">
                                    //   <value>Expires at</value>
                                    // </data>
                                    XmlNode el = resourceDoc.DocumentElement.AppendChild(resourceDoc.CreateElement("data"));
                                    el.Attributes.Append(resourceDoc.CreateAttribute("name")).InnerText = keyName;
                                    el.Attributes.Append(resourceDoc.CreateAttribute("xml:space")).InnerText = "preserve";
                                    el.AppendChild(resourceDoc.CreateElement("value")).InnerText = this.SplitCamelCase(prop.Name);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string log = ex.Message;
                            //throw ex;
                        }
                    }
                }

                resourceDoc.Save(resourceFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void SplitCamelCaseTest()
        {
            string camel = "CreatedByMrJones";
            string normel = "Created by mr jones";

            string transferred = this.SplitCamelCase(camel);

            Assert.AreEqual(transferred,normel);
        }

        private string SplitCamelCase(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            else
            {
                string res = Regex.Replace(
                    Regex.Replace(
                        str,
                        @"(\P{Ll})(\P{Ll}\p{Ll})",
                        "$1 $2"
                    ),
                    @"(\p{Ll})(\P{Ll})",
                    "$1 $2"
                );

                // retain first char
                return str.Substring(0, 1) + res.Substring(1).ToLower();
            }
        }
    }
}
