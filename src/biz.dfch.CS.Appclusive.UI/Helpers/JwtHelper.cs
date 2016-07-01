using System.Configuration;
using System.Diagnostics.Contracts;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public class JwtHelper
    {
        private const string JwtHeaderCacheKey = "JwtHeader";

        private static string _jwtHeaderKey = string.Empty;
        public static string JwtHeaderKey
        {
            get
            {
                if (string.IsNullOrEmpty(_jwtHeaderKey))
                {
                    _jwtHeaderKey = ConfigurationManager.AppSettings[PermissionConfig.JwtHeader_AppSettings_Key];
                }

                return _jwtHeaderKey;
            }
        }

        public static string JwtHeader
        {
            get
            {
                Contract.Assert(HasJwtHeader);
                return HttpContext.Current.Items[JwtHeaderCacheKey] as string;
            }
            set { HttpContext.Current.Items[JwtHeaderCacheKey] = value; }
        }

        public static bool HasJwtHeader
        {
            get { return HttpContext.Current.Items[JwtHeaderCacheKey] != null; }
        }
    }
}