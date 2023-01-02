using UnityEngine;
using System;
using UnityEngine.UIElements;
using System.Reflection;
using TriplanoTest.Shared.ExtensionMethods;

namespace TriplanoTest.UIBuilder.Editor
{
    public static class SliderIntBuilder
    {
        internal static SliderInt CreateSliderIntFromVisualElement(FieldInfo field, object target)
        {
            RangeAttribute range = field.GetCustomAttribute<RangeAttribute>();

            if (field.GetValue(target) is not SliderInt sliderInt)
            {
                sliderInt = Activator.CreateInstance(typeof(SliderInt)) as SliderInt;
                SetSliderInt(sliderInt, field.Name.GetNiceString());

                range ??= new RangeAttribute(sliderInt.lowValue, sliderInt.highValue);
            }

            if (range != null)
            {
                sliderInt.lowValue = (int)range.min;
                sliderInt.highValue = (int)range.max;
            }

            // Bind
            field.SetValue(target, sliderInt);

            return sliderInt;
        }

        internal static void SetSliderInt(SliderInt sliderInt, string label, int value = 0)
        {
            sliderInt.label = label;
            sliderInt.showInputField = true;
            sliderInt.value = sliderInt.value > sliderInt.highValue ? sliderInt.highValue : value;
        }
    }
}