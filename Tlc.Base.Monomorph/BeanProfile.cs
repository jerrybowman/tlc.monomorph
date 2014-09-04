using System.Reflection;

namespace Tlc.Base.Monomorph
{
    internal class BeanProfile
    {
        public BeanAttribute Attribute { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public ApplicationConfig Config { get; set; }
        public object Instance { get; set; }
    }
}