using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class Message
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class Innererror
    {
        public string message { get; set; }
        public string type { get; set; }
        public string stacktrace { get; set; }
    }

    public class OdataError
    {
        public string code { get; set; }
        public Message message { get; set; }
        public Innererror innererror { get; set; }
    }

    public class OdataErrorRoot
    {
        public OdataError odata_error { get; set; }
    }
}