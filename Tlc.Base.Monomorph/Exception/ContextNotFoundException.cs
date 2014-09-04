using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class ContextNotFoundException : MonomorphException
    {
        public ContextNotFoundException()
        {
        }

        public ContextNotFoundException(string contextName)
            : base(String.Format("Requested context {0} has not been created", contextName))
        {
        }
    }
}