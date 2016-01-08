using biz.dfch.CS.Appclusive.UI.App_LocalResources;
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
            this.Created = DateTimeOffset.Now;
            this.Modified = DateTimeOffset.Now;
            this.CreatedById = 1;
            this.ModifiedById = 1;
            this.Children = new List<Tenant>();
        }

        [Display(Name = "Id", ResourceType = typeof(GeneralResources))]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Id", ResourceType = typeof(GeneralResources))]
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
        [Display(Name = "Name", ResourceType = typeof(GeneralResources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GeneralResources))]
        public string Description { get; set; }

        [Display(Name = "ExternalId", ResourceType = typeof(GeneralResources))]
        public string ExternalId { get; set; }

        [Display(Name = "ExternalType", ResourceType = typeof(GeneralResources))]
        public string ExternalType { get; set; }

        [Required]
        [Display(Name = "CreatedById", ResourceType = typeof(GeneralResources))]
        public long CreatedById { get; set; }

        [Display(Name = "CreatedBy", ResourceType = typeof(GeneralResources))]
        public User CreatedBy { get; set; }

        [Required]
        [Display(Name = "ModifiedById", ResourceType = typeof(GeneralResources))]
        public long ModifiedById { get; set; }

        [Display(Name = "ModifiedBy", ResourceType = typeof(GeneralResources))]
        public User ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Created", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset Created { get; set; }

        [Required]
        [Display(Name = "Modified", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset Modified { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(GeneralResources))]
        public Tenant Parent { get; set; }

        [Required]
        [Display(Name = "ParentId", ResourceType = typeof(GeneralResources))]
        public Guid ParentId { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(GeneralResources))]
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

        [Display(Name = "Children", ResourceType = typeof(GeneralResources))]
        public List<Tenant> Children { get; set; }

        [Required]
        [Display(Name = "CustomerId", ResourceType = typeof(GeneralResources))]
        public long CustomerId { get; set; }

        [Display(Name = "Customer", ResourceType = typeof(GeneralResources))]
        public Customer Customer { get; set; }
    }
}