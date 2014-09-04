using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class BeanNotFoundException : MonomorphException
    {
        public BeanNotFoundException(Type type)
            : base(String.Format("Requested bean {0} is not found", type.Name))
        {
        }

        public BeanNotFoundException(string beanName)
            : base(String.Format("Requested bean {0} is not found", beanName))
        {
        }
    }
}