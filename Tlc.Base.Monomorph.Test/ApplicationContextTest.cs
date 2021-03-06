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

using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tlc.Base.Monomorph.Exception;

namespace Tlc.Base.Monomorph.Test

{
    [TestFixture]
    public class ApplicationContextTest
    {
        [Test]
        public void CreateContextTest()
        {
            Assert.Throws<ContextNotFoundException>(() => ApplicationContext.GetContext());
            Assert.Throws<ContextNotFoundException>(() => ApplicationContext.GetContext("context2"));

            var ctx1 = ApplicationContext.NewContext();
            Assert.IsInstanceOf<ApplicationContext>(ctx1);
            Assert.Throws<ContextAlreadyDefinedException>(() => ApplicationContext.NewContext());

            var ctx2 = ApplicationContext.NewContext("context2");
            Assert.IsInstanceOf<ApplicationContext>(ctx2);

            var ctx3 = ApplicationContext.GetContext();
            Assert.IsInstanceOf<ApplicationContext>(ctx3);
            Assert.AreSame(ctx1, ctx3);

            var ctx4 = ApplicationContext.GetContext("context2");
            Assert.IsInstanceOf<ApplicationContext>(ctx4);
            Assert.AreSame(ctx2, ctx4);
        }

        [Test]
        public void AddConfigTest()
        {
            var ctx = ApplicationContext.NewContext("addconfig");
            ctx.AddConfig(new MockConfig());
            Assert.Throws<DuplicationBeanDefinitionException>(() => ctx.AddConfig(new MockConfig()));
        }

        [Test]
        public void GetBeanTest()
        {
            var ctx = ApplicationContext.NewContext("beantest");
            ctx
                .AddConfig(new MockConfig());

            var bean1 = ctx.GetBean<MockBeanOne>();
            Assert.IsInstanceOf<MockBeanOne>(bean1);

            var bean1A = ctx.GetBean<MockBeanOne>("MockBeanOne");
            Assert.IsInstanceOf<MockBeanOne>(bean1A);

            Assert.AreSame(bean1, bean1A);

            var bean2 = ctx.GetBean<MockBeanTwo>();
            Assert.IsInstanceOf<MockBeanTwo>(bean2);

            var bean2B = ctx.GetBean<MockBeanTwo>();
            Assert.IsInstanceOf<MockBeanTwo>(bean2B);

            Assert.AreNotSame(bean2, bean2B);

            MockBeanOne bean12 = ctx.GetBean<MockBeanOne>("MockBeanOneWithTwo");
            Assert.IsInstanceOf<MockBeanOne>(bean12);
            Assert.IsNotNull(bean12.Two);
            Assert.IsInstanceOf<MockBeanTwo>(bean12.Two);

            var bean2A = ctx.GetBean<MockBeanTwo>("mockbeantwo");
            Assert.IsInstanceOf<MockBeanTwo>(bean2A);

            Assert.Throws<MismatchedBeanTypeException>(() => ctx.GetBean<MockBeanTwo>("MockBeanOneWithTwo"));

            var bean3 = ctx.GetBean<MockBeanThree>("BeanThree");
            Assert.IsInstanceOf<MockBeanThree>(bean3);

        }

        private class MockConfig : ApplicationConfig
        {
            [Bean]
            public MockBeanOne MockBeanOneWithTwo()
            {
                return new MockBeanOne(Context.GetBean<MockBeanTwo>());
            }

            [Bean]
            public MockBeanOne MockBeanOne()
            {
                return new MockBeanOne();
            }

            [Bean(IsSingleton = false)]
            public MockBeanTwo mockbeantwo()
            {
                return new MockBeanTwo();
            }

            [Bean("BeanThree", IsSingleton = false)]
            public MockBeanThree MockBeanThree()
            {
                return new MockBeanThree();
            }
        }

        private class MockBeanOne
        {
            public MockBeanTwo Two { get; set; }

            public MockBeanOne(MockBeanTwo two)
            {
                Two = two;
            }

            public MockBeanOne()
            {
            }
        }

        private class MockBeanTwo
        {
        }

        private class MockBeanThree
        {
        }
    }
}
