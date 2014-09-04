using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class DuplicationBeanDefinitionException : MonomorphException
    {
        public DuplicationBeanDefinitionException(string configName, string beanName)
            : base(String.Format("Duplicate bean defined for bean {0} in config {1}", beanName, configName))
        {
        }
    }
}