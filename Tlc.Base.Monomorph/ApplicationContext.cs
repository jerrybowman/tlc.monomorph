/***********************************************************************************************************************
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
using System.Collections.Generic;
using Tlc.Base.Monomorph.Exception;

namespace Tlc.Base.Monomorph
{
    public class ApplicationContext
    {
        private const string DefaultContextName = "default";

        private static readonly Dictionary<string, ApplicationContext> contextMap =
            new Dictionary<string, ApplicationContext>();

        private readonly List<ApplicationConfig> configList = new List<ApplicationConfig>();
        private readonly Dictionary<Type, BeanProfile> beanMap = new Dictionary<Type, BeanProfile>();
        private readonly Dictionary<string, BeanProfile> beanNameMap = new Dictionary<string, BeanProfile>();

        private ApplicationContext()
        {
        }

        public static ApplicationContext NewContext() 
        {
            return NewContext(DefaultContextName);
        }

        public static ApplicationContext NewContext(string contextName) 
        {
            if (contextMap.ContainsKey(contextName))
                throw new ContextAlreadyDefinedException(contextName);
            var ctx = new ApplicationContext();
            lock(contextMap)
            {
                contextMap.Add(contextName, ctx);
            }
            return ctx;
        }

        public static ApplicationContext GetContext()
        {
            return GetContext(DefaultContextName);
        }

        public static ApplicationContext GetContext(string contextName)
        {
            ApplicationContext ctx;
            bool found;
            lock (contextMap)
            {
                found = contextMap.TryGetValue(contextName, out ctx);
            }
            if (!found)
                    throw new ContextNotFoundException(contextName);
            return ctx;
        }

        public T GetBean<T>()
        {
            BeanProfile profile;
            var found = beanMap.TryGetValue(typeof (T), out profile);
            if (!found)
                throw new BeanNotFoundException(typeof (T));
           
            return MakeInstance<T>(profile);
        }

        public T GetBean<T>(string beanName)
        {
            BeanProfile profile;
            var found = beanNameMap.TryGetValue(beanName, out profile);
            if (!found)
                throw new BeanNotFoundException(beanName);
            if (profile.MethodInfo.ReturnType != typeof(T))
                throw new MismatchedBeanTypeException(beanName, profile.MethodInfo.ReturnType, typeof(T));
            return MakeInstance<T>(profile);
        }

        private T MakeInstance<T>(BeanProfile profile)
        {
            if (profile.Attribute.IsSingleton && profile.Instance != null)
                return (T)profile.Instance;
            var instance = (T)profile.MethodInfo.Invoke(profile.Config, new object[0]);
            if (profile.Attribute.IsSingleton)
                profile.Instance = instance;
            return instance;
        }

        public ApplicationContext AddConfig(ApplicationConfig config)
        {
            configList.Add(config);
            config.Context = this;
            UpdateBeanMaps(config);
            return this;
        }

        private void UpdateBeanMaps(ApplicationConfig config)
        {
            var configType = config.GetType();
            var methods = configType.GetMethods();
            foreach (var methodInfo in methods)
            {
                var beanAttrs = (BeanAttribute[])methodInfo.GetCustomAttributes(typeof(BeanAttribute), false);
                if (beanAttrs.Length == 0) continue;
                var returnType = methodInfo.ReturnType;
                var profile = new BeanProfile()
                                          {
                                              Attribute = beanAttrs[0],
                                              MethodInfo = methodInfo,
                                              Config = config
                                          };
                if (methodInfo.Name.Equals(returnType.Name, StringComparison.InvariantCultureIgnoreCase)
                    && methodInfo.GetGenericArguments().Length == 0)
                {
                    if (beanMap.ContainsKey(returnType))
                        throw new DuplicationBeanDefinitionException(configType.FullName, returnType.FullName);
                    beanMap.Add(returnType, profile);
                }
                var beanName = profile.Attribute.Name ?? methodInfo.Name;
                if (beanNameMap.ContainsKey(beanName))
                    throw new DuplicationBeanDefinitionException(configType.FullName, beanName);
                beanNameMap.Add(beanName, profile);
            }
        }
    }
}
