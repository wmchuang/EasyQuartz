using System;

namespace EasyQuartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class JobIgnoreAttribute : Attribute
    {
    }
}
