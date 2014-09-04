using System;

namespace Tlc.Base.Monomorph
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class BeanAttribute : Attribute
    {
        public BeanAttribute()
        {
            IsSingleton = true;
        }

        public BeanAttribute(string name) : this()
        {
            Name = name;
        }

        public string Name { get; set; }
        public bool IsSingleton { get; set; }
    }
}