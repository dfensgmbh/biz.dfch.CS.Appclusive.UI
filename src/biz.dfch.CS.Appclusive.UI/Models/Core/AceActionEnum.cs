using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public enum AceActionEnum
    {
        BREAK_INHERITANCE = 0
        ,
        ALLOW = 0x00004000
        ,
        ALLOW_AND_INHERIT
        ,
        USE_BASE_PERMISSION = 0x00006000
        ,
        DENY = 0x00008000
        ,
        DENY_AND_INHERIT
    }

}