﻿/***********************************************************************************************************************
 *
 * monomorph -- an open source .net simple dependency framework
 * ============================================================
 *
 * Copyright (C) 2011-2014 by The Last Check, LLC
 * https://github.com/jerrybowman/tlc.monomorph
 *
 ***********************************************************************************************************************
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations under the License.
 *
 **********************************************************************************************************************/

using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class MismatchedBeanTypeException : MonomorphException
    {
        public MismatchedBeanTypeException(object baenName, Type beanType, Type type)
            : base(String.Format("Requested bean type of {0} does not match bean type of bean name {1}. "
                                 + "Bean name {1} has a type of {2}.", beanType.Name, beanType, type.Name))
        {
        }
    }
}
