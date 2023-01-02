using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace TriplanoTest.UIBuilder.Editor
{
    public static class ReflectionUtils
    {
        public static IEnumerable<MethodInfo> GetMethodsWith<T>(object target) where T : Attribute
        {
            return target.GetType()
                .GetMethods()
                .Where(method => method.GetCustomAttribute(typeof(T), false) != null);
        }
    }
}