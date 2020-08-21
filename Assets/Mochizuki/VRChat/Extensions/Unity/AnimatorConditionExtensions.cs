using System;

using UnityEditor.Animations;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorConditionExtensions
    {
        public static bool Equals(this AnimatorCondition obj1, AnimatorCondition obj2)
        {
            return obj1.parameter == obj2.parameter && obj1.mode == obj2.mode && Math.Abs(obj1.threshold - obj2.threshold) == 0;
        }
    }
}