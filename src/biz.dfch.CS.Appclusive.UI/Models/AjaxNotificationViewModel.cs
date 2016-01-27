﻿/**
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
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class AjaxNotificationViewModel
    {
        public AjaxNotificationViewModel() { }
        public AjaxNotificationViewModel(ENotifyStyle level, string message, string elementId = null)
        {
            this.Level = level;
            this.Message = message;
            this.ElementId = elementId;
        }

        public string Message { get; set; }
        public ENotifyStyle Level { get; set; }
        public string ElementId { get; set; }
    }

    public enum ENotifyStyle
    {
        success,
        info,
        warn,
        error

    }
}