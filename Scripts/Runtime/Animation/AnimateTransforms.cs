using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Animation/" + nameof(AnimateTransforms))]
    public class AnimateTransforms : MonoBehaviour
    {
        [Header("Axis")] 
        [SerializeField] bool x;
        [SerializeField] bool y;
        [SerializeField] bool z;

        [Header("Animation")] 
        [SerializeField] private AnimationSpace space = AnimationSpace.Local;
        [SerializeField] float speed = 1f;
        [SerializeField] float amplitude = 0.1f;
        [SerializeField] float frequency = 20f;
        [SerializeField] bool easings;
        [ValueDropdown("GetDropdownItems")] 
        [ShowIf("easings")]
        [SerializeField] private string selectedEasing = "EaseInSine";

        private Transform MyTransform;
        
        void Start()
        {
            MyTransform = GetComponent<Transform>();
        }

        void Update()
        {
            float time = Time.time * speed;
            Vector3 animatedOffset = Vector3.zero;

            if (x)
            {
                float value = amplitude * Mathf.Sin(time * frequency);
                if (easings) value = ApplyEasingFunction(value, selectedEasing);
                animatedOffset.x = value;
            }

            if (y)
            {
                float value = amplitude * Mathf.Sin(time * frequency);
                if (easings) value = ApplyEasingFunction(value, selectedEasing);
                animatedOffset.y = value;
            }

            if (z)
            {
                float value = amplitude * Mathf.Sin(time * frequency);
                if (easings) value = ApplyEasingFunction(value, selectedEasing);
                animatedOffset.z = value;
            }

            if (space == AnimationSpace.Local)
            {
                MyTransform.localPosition = animatedOffset;
            }
            else
            {
                MyTransform.position = animatedOffset;
            }
        }

        float ApplyEasingFunction(float value, string easingFunction)
        {
            switch (easingFunction)
            {
                case "EaseInSine":
                    return EasingFunctions.EaseInSine(value);
                case "EaseOutSine":
                    return EasingFunctions.EaseOutSine(value);
                case "EaseInOutSine":
                    return EasingFunctions.EaseInOutSine(value);
                case "EaseInQuad":
                    return EasingFunctions.EaseInQuad(value);
                case "EaseOutQuad":
                    return EasingFunctions.EaseOutQuad(value);
                case "EaseInOutQuad":
                    return EasingFunctions.EaseInOutQuad(value);
                case "EaseInCubic":
                    return EasingFunctions.EaseInCubic(value);
                case "EaseOutCubic":
                    return EasingFunctions.EaseOutCubic(value);
                case "EaseInOutCubic":
                    return EasingFunctions.EaseInOutCubic(value);
                case "EaseInQuart":
                    return EasingFunctions.EaseInQuart(value);
                case "EaseOutQuart":
                    return EasingFunctions.EaseOutQuart(value);
                case "EaseInOutQuart":
                    return EasingFunctions.EaseInOutQuart(value);
                case "EaseInQuint":
                    return EasingFunctions.EaseInQuint(value);
                case "EaseOutQuint":
                    return EasingFunctions.EaseOutQuint(value);
                case "EaseInOutQuint":
                    return EasingFunctions.EaseInOutQuint(value);
                case "EaseInExpo":
                    return EasingFunctions.EaseInExpo(value);
                case "EaseOutExpo":
                    return EasingFunctions.EaseOutExpo(value);
                case "EaseInOutExpo":
                    return EasingFunctions.EaseInOutExpo(value);
                case "EaseInCirc":
                    return EasingFunctions.EaseInCirc(value);
                case "EaseOutCirc":
                    return EasingFunctions.EaseOutCirc(value);
                case "EaseInOutCirc":
                    return EasingFunctions.EaseInOutCirc(value);
                case "EaseInBack":
                    return EasingFunctions.EaseInBack(value);
                case "EaseOutBack":
                    return EasingFunctions.EaseOutBack(value);
                case "EaseInOutBack":
                    return EasingFunctions.EaseInOutBack(value);
                case "EaseInElastic":
                    return EasingFunctions.EaseInElastic(value);
                case "EaseOutElastic":
                    return EasingFunctions.EaseOutElastic(value);
                case "EaseInOutElastic":
                    return EasingFunctions.EaseInOutElastic(value);
                case "EaseInBounce":
                    return EasingFunctions.EaseInBounce(value);
                case "EaseOutBounce":
                    return EasingFunctions.EaseOutBounce(value);
                case "EaseInOutBounce":
                    return EasingFunctions.EaseInOutBounce(value);
                default:
                    return value;
            }
        }

        private static IEnumerable<string> GetDropdownItems()
        {
            return new[]
            {
                "EaseInSine",
                "EaseOutSine",
                "EaseInOutSine",
                "EaseInQuad",
                "EaseOutQuad",
                "EaseInOutQuad",
                "EaseInCubic",
                "EaseOutCubic",
                "EaseInOutCubic",
                "EaseInQuart",
                "EaseOutQuart",
                "EaseInOutQuart",
                "EaseInQuint",
                "EaseOutQuint",
                "EaseInOutQuint",
                "EaseInExpo",
                "EaseOutExpo",
                "EaseInOutExpo",
                "EaseInCirc",
                "EaseOutCirc",
                "EaseInOutCirc",
                "EaseInBack",
                "EaseOutBack",
                "EaseInOutBack",
                "EaseInElastic",
                "EaseOutElastic",
                "EaseInOutElastic",
                "EaseInBounce",
                "EaseOutBounce",
                "EaseInOutBounce"
            };
        }
        
        private enum AnimationSpace
        {
            Local,
            World
        }
    }
}