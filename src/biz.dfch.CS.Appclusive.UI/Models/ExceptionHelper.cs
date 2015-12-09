using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Xml;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public static class ExceptionHelper
    {
        public static List<AjaxNotificationViewModel> GetAjaxNotifications(Exception ex)
        {
            Contract.Requires(null != ex);

            List<AjaxNotificationViewModel> noteList = new List<AjaxNotificationViewModel>();
            Exception e = ex;
            while (null != e)
            {
                #region parse xml

                string message = e.Message;
                if (e.Message.StartsWith("<?xml version="))
                {
                    try
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(e.Message);
                        XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);
                        nsManager.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                        XmlNode mel = xml.SelectSingleNode("/*/m:message", nsManager);
                        if (mel != null)
                        {
                            message = mel.InnerText;
                        }
                    }
                    catch { }
                }
                //<?xml version="1.0" encoding="utf-8"?>
                //<m:error xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata">
                //  <m:code />
                //  <m:message xml:lang="en-US">There can only be one VDI in the cart.</m:message>
                //</m:error>

                #endregion

                noteList.Add(new AjaxNotificationViewModel()
                {
                    Level = ENotifyStyle.error,
                    Message = message
                });
                e = e.InnerException;
            }
            return noteList;
        }
    }
}