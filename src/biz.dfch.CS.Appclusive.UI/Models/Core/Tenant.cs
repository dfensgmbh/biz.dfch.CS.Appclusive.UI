using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Tenant
    {
        public Tenant()
        {
            this.Children = new List<Tenant>();
        }
        public Guid Id { get; set; }
        public string IdStr
        {
            get { return this.Id != null ? this.Id.ToString() : ""; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.Id = Guid.Empty;
                }
                else
                {
                    this.Id = Guid.Parse(value);
                }
            }
        }

        [Required]
        [StringLength(1024)]
        public string Name { get; set; }
        public string Description { get; set; }

        public string ExternalId { get; set; }
        public string ExternalType { get; set; }

        [Required]
        public long CreatedById { get; set; }

        public User CreatedBy { get; set; }

        [Required]
        public long ModifiedById { get; set; }

        public User ModifiedBy { get; set; }

        [Required]
        public DateTimeOffset Created { get; set; }
        [Required]
        public DateTimeOffset Modified { get; set; }


        [Required]
        public Guid ParentId { get; set; }
        public Tenant Parent { get; set; }

        public string ParentIdStr {
            get {return this.ParentId != null ? this.ParentId.ToString() : ""; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.ParentId = Guid.Empty;
                }
                else
                {
                    this.ParentId = Guid.Parse(value);
                }
            }
        }

        public List<Tenant> Children { get; set; }

    }
}