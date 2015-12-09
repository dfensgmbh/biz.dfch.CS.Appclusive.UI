using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class VdiCartItem : CartItem
    {
        // same values like namne of the CatalogItem in the database
        public const string VDI_PERSONAL_NAME = "VDI Personal";
        public const string VDI_TECHNICAL_NAME = "VDI Technical";

        public string VdiName { get; set; }

        [Display(Name = "HelpText", ResourceType = typeof(GeneralResources))]
        public string HelpText
        {
            get
            {
                switch (this.VdiName)
                {
                    case VDI_PERSONAL_NAME:
                        return "";
                    case VDI_TECHNICAL_NAME:
                        return GeneralResources.VDI_TECHNICAL_HelpText;
                    default:
                        return GeneralResources.VDI_INVALID_HelpText;
                }
            }
        }

        public string Title
        {
            get
            {
                switch (this.VdiName)
                {
                    case VDI_PERSONAL_NAME:
                        return "Add VDI for personal use";
                    case VDI_TECHNICAL_NAME:
                        return "Add VDI for another user";
                    default:
                        return "invalid VDI";
                }
            }
        }


        public string Requester { get; set; }

        internal string RequesterToParameters()
        {
            if (this.VdiName == VDI_TECHNICAL_NAME)
            {
                // {"Requester":"tralala"}
                var obj = new { Requester = this.Requester };
                string json = JsonConvert.SerializeObject(obj);
                return json;
            }
            else
            {
                return null;
            }
        }
    }
}