using System;
using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    public class MainCamera : Singleton<MainCamera>
    {
        public static Transform Transform => Instance.transform;
        public static Transform PlayerTarget { get; private set; }

        public static void SetPlayerTarget(Transform cameraTarget)
        {
            PlayerTarget = cameraTarget;
        }
    }
}