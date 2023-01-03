using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using TriplanoTest.UIBuilder;

namespace TriplanoTest.AppEditor
{
    public class LevelDesignTools
    {
        #region Rounder
        [Range(1, 4)]
        private SliderInt divisionFactor;

        [Button]
        public void RoundTransform()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Undo.RecordObject(obj.transform, "roundedTransform");

                obj.transform.localPosition = Round(obj.transform.localPosition);
            }
        }

        private Vector3 Round(Vector3 inicialValue)
        {
            return new Vector3()
            {
                x = Round(inicialValue.x),
                y = Round(inicialValue.y),
                z = Round(inicialValue.z)
            };
        }

        private float Round(float inicialValue)
        {
            float divisions = (int)Mathf.Pow(divisionFactor.value, 2f);
            return (float)Math.Round(inicialValue * divisions, MidpointRounding.AwayFromZero) / divisions;
        }
        #endregion
    }
}