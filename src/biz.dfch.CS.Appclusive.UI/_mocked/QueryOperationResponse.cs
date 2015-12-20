using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI._mocked
{
    public class QueryOperationResponse<T> :List<T> 
    {
        public long TotalCount { get { return this.Count; } }

        public static QueryOperationResponse<T> Convert(List<T> list)
        {
            QueryOperationResponse<T> qr = new QueryOperationResponse<T>();
            list.ForEach(i => qr.Add(i));
            return qr;
        }
        
    }
}