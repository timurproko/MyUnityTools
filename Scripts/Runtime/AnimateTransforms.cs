using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Animation/" + nameof(AnimateTransforms))]
    public class AnimateTransforms : MonoBehaviour
    {
        private Transform MyTransform;

        [Header("Axis")] [SerializeField] bool x;
        [SerializeField] bool y;
        [SerializeField] bool z;

        [Header("Animation")] [SerializeField] float speed = 1f;
        [SerializeField] float amplitude = 0.1f;
        [SerializeField] float frequency = 20f;

        [Header("Options")] [SerializeField] bool easings;

        [ValueDropdown("GetDropdownItems")] [SerializeField]
        private string selectedEasing = "EaseInSine";

        // Use this method to populate the dropdown options
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

        void Start()
        {
            MyTransform = GetComponent<Transform>();
        }

        void Update()
        {
            float time = Time.time * speed;
            Vector3 newPosition = MyTransform.position;

            // Use the selected easing function from the dropdown
            if (x)
            {
                newPosition.x = amplitude * Mathf.Sin(time * frequency);

                if (easings)
                    newPosition.x = ApplyEasingFunction(newPosition.x, selectedEasing);
            }

            if (y)
            {
                newPosition.y = amplitude * Mathf.Sin(time * frequency);

                if (easings)
                    newPosition.y = ApplyEasingFunction(newPosition.y, selectedEasing);
            }

            if (z)
            {
                newPosition.z = amplitude * Mathf.Sin(time * frequency);

                if (easings)
                    newPosition.z = ApplyEasingFunction(newPosition.z, selectedEasing);
            }

            MyTransform.position = newPosition;
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
    }
}