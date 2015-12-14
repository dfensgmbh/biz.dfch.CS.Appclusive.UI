using Newtonsoft.Json;
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
                string innerMessage = null;
                if (e.Message.StartsWith("<?xml version="))
                {
                    //<?xml version="1.0" encoding="utf-8"?>
                    //<m:error xmlns:m="http://schemas.microsoft.com/ado/2007/08/dataservices/metadata">
                    //  <m:code />
                    //  <m:message xml:lang="en-US">There can only be one VDI in the cart.</m:message>
                    //</m:error>
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
                else
                {
                    if (e.Message.Contains("odata.error"))
                    {
                        try
                        {
                            OdataErrorRoot error = JsonConvert.DeserializeObject<OdataErrorRoot>(e.Message.Replace("odata.error", "odata_error"));
                            if (null != error && null != error.odata_error && null != error.odata_error.message)
                            {
                                message = error.odata_error.message.value;
                                if (null != error.odata_error.innererror)
                                {
                                    innerMessage = error.odata_error.innererror.message;
                                }
                            }
                        }
                        catch { }
                    }
                }

                #endregion

                noteList.Add(new AjaxNotificationViewModel()
                {
                    Level = ENotifyStyle.error,
                    Message = message
                });
                if (!string.IsNullOrEmpty(innerMessage))
                {
                    noteList.Add(new AjaxNotificationViewModel()
                    {
                        Level = ENotifyStyle.error,
                        Message = innerMessage
                    });
                }
                e = e.InnerException;
            }
            return noteList;
        }
    }
}