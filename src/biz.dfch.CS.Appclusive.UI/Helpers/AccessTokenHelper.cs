using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public static class AccessTokenHelper
    {
        private static string _headerKey;

        public static string HeaderKey
        {
            get
            {
                if (string.IsNullOrEmpty(_headerKey))
                {
                    _headerKey = ConfigurationManager.AppSettings[PermissionConfig.AccessTokenHeader_AppSettings_Key];
                }

                return _headerKey;
            }
        }

        public static string AccessToken
        {
            get
            {
                Contract.Assert(HasAccessToken);
                return HttpContext.Current.Items[Constants.AccessTokenSessionKey] as string;
            }

            set { HttpContext.Current.Items[Constants.AccessTokenSessionKey] = value; }
        }

        public static bool HasAccessToken
        {
            get { return HttpContext.Current.Items[Constants.AccessTokenSessionKey] != null; }
        }
    }
}