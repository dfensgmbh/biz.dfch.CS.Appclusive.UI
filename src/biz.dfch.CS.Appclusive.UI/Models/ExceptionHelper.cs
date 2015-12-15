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

﻿using Newtonsoft.Json;
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
               // parse xml/json error
               
                    if (e.Message.Contains("odata.error"))
                    {
                        try
                        {
                            OdataErrorRoot error = JsonConvert.DeserializeObject<OdataErrorRoot>(e.Message.Replace("odata.error", "odata_error"));
                            if (null != error && null != error.odata_error && null != error.odata_error.message)
                            {
                                if (null != error.odata_error.innererror)
                                {
                                    noteList.Add(new AjaxNotificationViewModel()
                                    {
                                        Level = ENotifyStyle.error,
                                        Message =  error.odata_error.innererror.message
                                    });
                                }
                                Internalexception internalexception = error.odata_error.innererror.internalexception;
                                while (null != internalexception)
                                {
                                    noteList.Add(new AjaxNotificationViewModel()
                                    {
                                        Level = ENotifyStyle.error,
                                        Message = internalexception.message
                                    });
                                    internalexception = internalexception.internalexception;
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        noteList.Add(new AjaxNotificationViewModel()
                        {
                            Level = ENotifyStyle.error,
                            Message = e.Message
                        });
                    }
              
                e = e.InnerException;
            }
            return noteList;
        }
    }
}