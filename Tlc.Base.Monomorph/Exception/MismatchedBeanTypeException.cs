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