using UnityEngine;
[AddComponentMenu("My Tools/Animation/" + nameof(AnimateTransforms))]

namespace MyTools
{
    public class AnimateTransforms : MonoBehaviour
    {
        private Transform MyTransform = null;
        // Customize these values to control the animation
        [Header("Axis")]
        [SerializeField] bool x = false; 
        [SerializeField] bool y = false; 
        [SerializeField] bool z = false; 
        [Header("Animation")]
        [SerializeField] float speed = 1f;  // How fast the object moves
        [SerializeField] float amplitude = 0.1f;  // How far the object moves horizontally
        [SerializeField] float frequency = 20f;  // How quickly the object oscillates
        [Header("Options")]
        [SerializeField] bool easings = false; 

        [HideInInspector]
        public int DropdownIndex = 0;
        [HideInInspector]
        public string DropdownLabel = "Easing Functions";
        [HideInInspector]
        public string[] DropdownItems = new string[] {
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
                    return FunctionsEasing.EaseInSine(value);
                case "EaseOutSine":
                    return FunctionsEasing.EaseOutSine(value);
                case "EaseInOutSine":
                    return FunctionsEasing.EaseInOutSine(value);
                case "EaseInQuad":
                    return FunctionsEasing.EaseInQuad(value);
                case "EaseOutQuad":
                    return FunctionsEasing.EaseOutQuad(value);
                case "EaseInOutQuad":
                    return FunctionsEasing.EaseInOutQuad(value);
                case "EaseInCubic":
                    return FunctionsEasing.EaseInCubic(value);
                case "EaseOutCubic":
                    return FunctionsEasing.EaseOutCubic(value);
                case "EaseInOutCubic":
                    return FunctionsEasing.EaseInOutCubic(value);
                case "EaseInQuart":
                    return FunctionsEasing.EaseInQuart(value);
                case "EaseOutQuart":
                    return FunctionsEasing.EaseOutQuart(value);
                case "EaseInOutQuart":
                    return FunctionsEasing.EaseInOutQuart(value);
                case "EaseInQuint":
                    return FunctionsEasing.EaseInQuint(value);
                case "EaseOutQuint":
                    return FunctionsEasing.EaseOutQuint(value);
                case "EaseInOutQuint":
                    return FunctionsEasing.EaseInOutQuint(value);
                case "EaseInExpo":
                    return FunctionsEasing.EaseInExpo(value);
                case "EaseOutExpo":
                    return FunctionsEasing.EaseOutExpo(value);
                case "EaseInOutExpo":
                    return FunctionsEasing.EaseInOutExpo(value);
                case "EaseInCirc":
                    return FunctionsEasing.EaseInCirc(value);
                case "EaseOutCirc":
                    return FunctionsEasing.EaseOutCirc(value);
                case "EaseInOutCirc":
                    return FunctionsEasing.EaseInOutCirc(value);
                case "EaseInBack":
                    return FunctionsEasing.EaseInBack(value);
                case "EaseOutBack":
                    return FunctionsEasing.EaseOutBack(value);
                case "EaseInOutBack":
                    return FunctionsEasing.EaseInOutBack(value);
                case "EaseInElastic":
                    return FunctionsEasing.EaseInElastic(value);
                case "EaseOutElastic":
                    return FunctionsEasing.EaseOutElastic(value);
                case "EaseInOutElastic":
                    return FunctionsEasing.EaseInOutElastic(value);
                case "EaseInBounce":
                    return FunctionsEasing.EaseInBounce(value);
                case "EaseOutBounce":
                    return FunctionsEasing.EaseOutBounce(value);
                case "EaseInOutBounce":
                    return FunctionsEasing.EaseInOutBounce(value);
                default:
                    return value;
            }
        }
    }
}