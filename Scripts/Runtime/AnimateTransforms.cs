using UnityEngine;

namespace MyTools.Components
{
    [AddComponentMenu("My Tools/Animation/" + nameof(AnimateTransforms))]
    public class AnimateTransforms : MonoBehaviour
    {
        private Transform MyTransform = null;

        // Customize these values to control the animation
        [Header("Axis")] [SerializeField] bool x = false;
        [SerializeField] bool y = false;
        [SerializeField] bool z = false;
        [Header("Animation")] [SerializeField] float speed = 1f; // How fast the object moves
        [SerializeField] float amplitude = 0.1f; // How far the object moves horizontally
        [SerializeField] float frequency = 20f; // How quickly the object oscillates
        [Header("Options")] [SerializeField] bool easings = false;

        [HideInInspector] public int DropdownIndex = 0;
        [HideInInspector] public string DropdownLabel = "Easing Functions";

        [HideInInspector] public string[] DropdownItems = new string[]
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

        // Start is called before the first frame update
        void Start()
        {
            MyTransform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            float time = Time.time * speed;
            Vector3 newPosition = MyTransform.position;

            // Get the selected easing function from DropdownItems
            string selectedEasing = DropdownItems[DropdownIndex];

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

        // Helper method to apply easing function
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