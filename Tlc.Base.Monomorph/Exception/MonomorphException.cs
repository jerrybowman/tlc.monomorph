using System;

namespace Tlc.Base.Monomorph.Exception
{
    public class MonomorphException : ApplicationException
    {
        public MonomorphException()
        {
        }

        public MonomorphException(string message) : base(message)
        {
        }
    }
}