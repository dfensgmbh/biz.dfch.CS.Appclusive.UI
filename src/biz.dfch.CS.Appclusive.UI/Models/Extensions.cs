/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public static class Extensions
    {
        static public DateTime ToDateTime(this DateTimeOffset dtOffset)
        {
            return dtOffset == DateTimeOffset.MinValue 
                ? DateTime.MinValue 
                : dtOffset.DateTime;
        }

        static public DateTimeOffset ToDateTimeOffset(this DateTime datetime)
        {
            return datetime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                    ? DateTimeOffset.MinValue
                    : new DateTimeOffset(datetime);
        }

        public static void ResolveReferencedEntityName(this IEntityReference entity, biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            try
            {
                if (entity.EntityId > 0 && null != entity.EntityKind && null != entity.EntityKind.EntityType)
                {
                    string uriStr = Properties.Settings.Default.AppclusiveApiBaseUrl + "Core/{0}s()?$filter=Id%20eq%20{1}L";
                    Uri requestUri = new Uri(string.Format(uriStr, entity.EntityKind.EntityType.Name, entity.EntityId));
                 
                    // call Generic execute Method:     IEnumerable<Api.Core.Node> result = coreRepository.Execute<Api.Core.Node>(requestUri, HttpMethod.Get.ToString(), true, null);                    
                    Type[] paramTypes = { typeof(Uri), typeof(String), typeof(Boolean), typeof(System.Data.Services.Client.OperationParameter[]) };
                    MethodInfo m = typeof(biz.dfch.CS.Appclusive.Api.Core.Core).GetMethod("Execute", paramTypes);
                    Type[] genericTypeArgs = { entity.EntityKind.EntityType };
                    MethodInfo execute = m.MakeGenericMethod(genericTypeArgs);

                    object[] args = {requestUri, HttpMethod.Get.ToString(), true, null};
                    IEnumerable<object> result = (IEnumerable<object>)execute.Invoke(coreRepository, args);
                    object refEntity = result.FirstOrDefault();

                    // read name property
                    PropertyInfo p = entity.EntityKind.EntityType.GetProperty("Name");
                    entity.EntityName = (string)p.GetValue(refEntity);
                }
            }
            catch (Exception ex)
            {
                // add logging 
            }
        }
    }
}