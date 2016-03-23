using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace biz.dfch.CS.Appclusive.UI.Tests.Models
{
    [TestClass]
    public class AuditTrailTest
    {
        [TestMethod]
        public void CreateCompareTableTest()
        {
            string jsonBefore = "{\"Created\":\"2016-01-11T15:01:40.0422145+01:00\", \"beforeProp\":8}";
            string jsonAfter = "{\"Created\":\"2016-01-11T15:01:40.0422145+01:00\",\"CreatedById\":60,\"Description\":\"\",\"EntityKindId\":29,\"Id\":0,\"Modified\":\"2016-01-11T15:01:40.0422145+01:00\",\"ModifiedById\":60,\"Name\":\"Name-f9565401-1736-4baf-8baa-3395cb35c339\",\"Parameters\":\"{}\",\"ParentId\":1,\"RowVersion\":null,\"Tid\":\"22222222-2222-2222-2222-222222222222\",\"Type\":\"com.swisscom.cms.rhel7\"}";

            Dictionary<string, object[]> properties = new Dictionary<string, object[]>();

            try
            {
                if (!string.IsNullOrWhiteSpace(jsonBefore))
                {
                    Newtonsoft.Json.Linq.JObject before = Newtonsoft.Json.Linq.JObject.Parse(jsonBefore);
                    foreach (var prop in before.Properties())
                    {
                        if (!properties.ContainsKey(prop.Name))
                        {
                            properties.Add(prop.Name, new object[2]);
                        }
                        properties[prop.Name][0] = prop.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(jsonAfter))
                {
                    Newtonsoft.Json.Linq.JObject after = Newtonsoft.Json.Linq.JObject.Parse(jsonAfter);
                    foreach (var prop in after.Properties())
                    {
                        if (!properties.ContainsKey(prop.Name))
                        {
                            properties.Add(prop.Name, new object[2]);
                        }
                        properties[prop.Name][1] = prop.Value;
                    }
                }
            }
            catch (Exception ex) {
                Assert.Fail(ex.Message);
            }

            // check
            Assert.IsTrue(properties.Count > 0);
        }
    }
}
