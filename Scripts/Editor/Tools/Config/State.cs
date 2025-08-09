#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    public static class ProjectPrefs
    {
        public static bool GetBool(string key, bool defaultValue = false) =>
            EditorPrefs.GetBool(key + projectId, defaultValue);

        public static void SetBool(string key, bool value) => EditorPrefs.SetBool(key + projectId, value);

        private static int projectId => PlayerSettings.productGUID.GetHashCode();
    }

    internal static class State
    {
        private const string MenuPath = Menus.GLOBAL_MENU + "Disable MyTools";
        private const string Key = "MyTools-PluginDisabled";

        public static bool disabled
        {
            get => ProjectPrefs.GetBool(Key, false);
            set
            {
                if (value == disabled) return;
                ProjectPrefs.SetBool(Key, value);
                UpdateMenuCheck();
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.RepaintProjectWindow();
            }
        }

        [MenuItem(MenuPath, priority = Menus.GLOBAL_INDEX)]
        private static void Toggle()
        {
            disabled = !disabled;
        }

        [MenuItem(MenuPath, validate = true)]
        private static bool Toggle_Validate()
        {
            UpdateMenuCheck();
            return true;
        }

        [InitializeOnLoadMethod]
        private static void Bootstrap()
        {
            UpdateMenuCheck();
        }

        private static void UpdateMenuCheck()
        {
            Menu.SetChecked(MenuPath, disabled);
        }
    }
}
#endif