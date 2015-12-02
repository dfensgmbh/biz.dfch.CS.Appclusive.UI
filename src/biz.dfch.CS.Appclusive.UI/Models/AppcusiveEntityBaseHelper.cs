using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    static public class AppcusiveEntityBaseHelper
    {
        /// <summary>
        /// setzt all the neccessary default values
        /// </summary>
        public static  void InitEntity(IAppcusiveEntityBase entity) {
            
            entity.Created = DateTimeOffset.Now;
            entity.Modified = DateTimeOffset.Now;
            entity.ModifiedBy = "admin-ui";
            entity.CreatedBy = "admin-ui";
            entity.Tid = "tenant-id";
        }
    }
}