using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class ContextAlreadyDefinedException : MonomorphException
    {
        public ContextAlreadyDefinedException(string contextName)
            : base(String.Format("Requested context {0} has already been created", contextName))
        {
        }
    }
}