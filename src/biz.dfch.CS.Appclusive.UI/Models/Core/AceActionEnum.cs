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
