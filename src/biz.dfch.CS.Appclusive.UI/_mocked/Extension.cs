using biz.dfch.CS.Appclusive.Api.Core;
using biz.dfch.CS.Appclusive.UI._mocked;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public static class Extension
    {

        #region Expand

        public static List<Customer> Expand(this List<Customer> list, string propertyName)
        {
            return list;
        }
        public static List<CimiTarget> Expand(this List<CimiTarget> list, string propertyName)
        {
            return list;
        }
        public static List<ContractMapping> Expand(this List<ContractMapping> list, string propertyName)
        {
            return list;
        }
        public static List<Tenant> Expand(this List<Tenant> list, string propertyName)
        {
            return list;
        }
        public static List<User> Expand(this List<User> list, string propertyName)
        {
            return list;
        }

        #endregion

        #region AddQueryOption

        public static List<Customer> AddQueryOption(this List<Customer> list, string propertyName, object value)
        {
            return list;
        }
        public static List<CimiTarget> AddQueryOption(this List<CimiTarget> list, string propertyName, object value)
        {
            return list;
        }
        public static List<ContractMapping> AddQueryOption(this List<ContractMapping> list, string propertyName, object value)
        {
            return list;
        }
        public static List<Tenant> AddQueryOption(this List<Tenant> list, string propertyName, object value)
        {
            return list;
        }
        public static List<User> AddQueryOption(this List<User> list, string propertyName, object value)
        {
            return list;
        }

        #endregion


        #region Execute

        public static QueryOperationResponse<Customer> Execute(this List<Customer> list)
        {
            return QueryOperationResponse<Customer>.Convert(list); 
        }
        public static QueryOperationResponse<CimiTarget> Execute(this List<CimiTarget> list)
        {
            return QueryOperationResponse<CimiTarget>.Convert(list);  
        }
        public static QueryOperationResponse<ContractMapping> Execute(this List<ContractMapping> list)
        {
            return QueryOperationResponse<ContractMapping>.Convert(list); 
        }
        public static QueryOperationResponse<Tenant> Execute(this List<Tenant> list)
        {
            return QueryOperationResponse<Tenant>.Convert(list); 
        }
        public static QueryOperationResponse<User> Execute(this List<User> list)
        {
            return QueryOperationResponse<User>.Convert(list); 
        }

        #endregion
    }
}