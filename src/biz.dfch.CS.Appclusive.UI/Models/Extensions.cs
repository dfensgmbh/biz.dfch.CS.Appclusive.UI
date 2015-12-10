using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}