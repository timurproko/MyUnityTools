#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class ToggleXRSimulator
    {
        private const string MENU_NAME  = Menus.MY_TOOLS_MENU + "Meta XR Simulator";
        private const int    ITEM_INDEX = Menus.MODES_INDEX + 300;

        private static bool _enabled; 
        private static double _nextPollTime;

        static ToggleXRSimulator()
        {
            _enabled = ReadActualState();
            ApplyUi(_enabled);
            Persist(_enabled);

            EditorApplication.update += PollExternalChanges;
        }

        [MenuItem(MENU_NAME, priority = ITEM_INDEX)]
        private static void Toggle()
        {
            if (State.disabled) return;
            PerformAction(!_enabled);
        }

        [MenuItem(MENU_NAME, true, priority = ITEM_INDEX)]
        private static bool Validate()
        {
            var actual = ReadActualState();
            if (actual != _enabled)
            {
                _enabled = actual;
                Persist(_enabled);
            }
            ApplyUi(actual);
            return !State.disabled;
        }

        public static void EnableSimulator()  => PerformAction(true);
        public static void DisableSimulator() => PerformAction(false);
        public static bool IsActivated()       => ReadActualState();

        private static void PerformAction(bool enable)
        {
            if (State.disabled) return;

            var enablerType = FindEnablerType();
            if (enablerType == null)
            {
                Utils.LogWarning("Meta XR Simulator Enabler type not found. Is the Meta XR package installed?");
                return;
            }

            bool currentlyActive = GetActivated(enablerType);

            if (enable && !currentlyActive)
                InvokeStatic(enablerType, "ActivateSimulator", new object[] { false });
            else if (!enable && currentlyActive)
                InvokeStatic(enablerType, "DeactivateSimulator", new object[] { false });

            _enabled = ReadActualState();
            Persist(_enabled);
            ApplyUi(_enabled);

            Utils.Log($"Meta XR Simulator {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PollExternalChanges()
        {
            if (EditorApplication.timeSinceStartup < _nextPollTime) return;
            _nextPollTime = EditorApplication.timeSinceStartup;

            var actual = ReadActualState();
            if (actual != _enabled)
            {
                _enabled = actual;
                Persist(_enabled);
                ApplyUi(_enabled);
            }
        }

        private static bool ReadActualState()
        {
            var enablerType = FindEnablerType();
            return enablerType != null && GetActivated(enablerType);
        }

        private static void ApplyUi(bool isOn)
        {
            Menu.SetChecked(MENU_NAME, isOn);
        }

        private static void Persist(bool isOn)
        {
            EditorPrefs.SetBool(MENU_NAME, isOn);
        }

        private static Type FindEnablerType()
        {
            const string FullName = "Meta.XR.Simulator.Editor.Enabler";
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType(FullName, throwOnError: false);
                if (t != null) return t;
            }
            return null;
        }

        private static bool GetActivated(Type enablerType)
        {
            var prop = enablerType.GetProperty("Activated", BindingFlags.Public | BindingFlags.Static);
            return prop != null && prop.GetValue(null) is true;
        }

        private static void InvokeStatic(Type type, string method, object[] args)
        {
            var mi = type.GetMethod(method, BindingFlags.Public | BindingFlags.Static);
            mi?.Invoke(null, args);
        }
    }
}
#endif
