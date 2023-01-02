using System;
using UnityEngine;

namespace TriplanoTest.UIBuilder
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ButtonAttribute : PropertyAttribute
    {
        public string Name { get; set; }
    }
}