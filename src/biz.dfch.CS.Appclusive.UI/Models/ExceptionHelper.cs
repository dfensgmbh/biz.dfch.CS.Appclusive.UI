/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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