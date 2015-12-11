
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.Api.Core
{
    public class Tenant 
    {
        public Tenant()
        {
            this.Children = new List<Tenant>();
            Id = System.Guid.Empty;
        }

        public Guid Id { get; set; }

        public string ExternalId { get; set; }
        public string ExternalType { get; set; }

        public Guid ParentId { get; set; }
        public Tenant Parent { get; set; }

        public List<Tenant> Children { get; set; }

        public string DisplayName
        {
            get
            {
                if (Id == System.Guid.Empty)
                {
                    return "-";
                }
                else
                {
                    return Id.ToString();
                }
            }
        }

    }
}