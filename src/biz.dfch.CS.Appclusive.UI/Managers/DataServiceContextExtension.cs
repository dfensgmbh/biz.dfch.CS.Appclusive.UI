using System;
using System.Data.Services.Client;
using System.Net;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Helpers;
using Microsoft.HBase.Client.LoadBalancing;

namespace biz.dfch.CS.Appclusive.UI.Managers
{
    public static class DataServiceContextExtension
    {
        public static void ConfigureForAppclusive(this DataServiceContext dataServiceContext)
        {
            dataServiceContext.IgnoreMissingProperties = true;
            dataServiceContext.Format.UseJson();
            dataServiceContext.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
            dataServiceContext.MergeOption = MergeOption.PreserveChanges;

            NetworkCredential apiCreds = HttpContext.Current.Session[Config.Constants.LoginDataSessionKey] as NetworkCredential;
            if (apiCreds != null)
            {
                dataServiceContext.Credentials = apiCreds;
            }

            dataServiceContext.SendingRequest2 += (sender, args) =>
            {
                if (JwtHelper.HasJwtHeader)
                {
                    args.RequestMessage.SetHeader(JwtHelper.JwtHeaderKey, JwtHelper.JwtHeader);
                }

                if (AccessTokenHelper.HasAccessToken)
                {
                    args.RequestMessage.SetHeader(
                        AccessTokenHelper.HeaderKey,
                        string.Format("Bearer {0}", AccessTokenHelper.AccessToken));
                    args.RequestMessage.SetHeader(
                        "Scs-Impersonation-Token",
                        string.Format("Bearer {0}", AccessTokenHelper.AccessToken));

                    if (TenantHelper.HasFixedTenantId)
                    {
                        args.RequestMessage.SetHeader("Tenant-Id", TenantHelper.FixedTenantId.ToString());
                    }
                    else
                    {
                        args.RequestMessage.SetHeader(
                            "Tenant-Id", Navigation.PermissionDecisions.Current.Tenant.Id.ToString());
                    }
                }
            };
        }

        public static void ConfigureForAppclusive(this Api.Core.Core core)
        {
            core.TenantID = Navigation.PermissionDecisions.Current.Tenant.Id.ToString();
            ConfigureForAppclusive(core as DataServiceContext);
        }

        public static void ConfigureForAppclusive(this Api.Cmp.Cmp cmp)
        {
            cmp.TenantID = Navigation.PermissionDecisions.Current.Tenant.Id.ToString();
            ConfigureForAppclusive(cmp as DataServiceContext);
        }

        public static void ConfigureForAppclusive(this Api.Diagnostics.Diagnostics diagnostics)
        {
            diagnostics.TenantID = Navigation.PermissionDecisions.Current.Tenant.Id.ToString();
            ConfigureForAppclusive(diagnostics as DataServiceContext);
        }
    }
}