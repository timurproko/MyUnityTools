#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    internal static class PlayWithDomainReload
    {
        private static bool _wasFastPlayEnabled;

        [MenuItem(Menus.MY_TOOLS_MENU + "Play With Reload %&#p", validate = true, priority = Menus.MODES_INDEX + 999)]
        private static bool ValidatePlayDomainReload() => !State.disabled;

        [MenuItem(Menus.MY_TOOLS_MENU + "Play With Reload %&#p", priority = Menus.MODES_INDEX + 302)]
        private static void PlayDomainReload()
        {
            if (State.disabled) return;
            if (EditorApplication.isPlaying) return;

            _wasFastPlayEnabled = EditorSettings.enterPlayModeOptionsEnabled;

            if (_wasFastPlayEnabled)
            {
                EditorSettings.enterPlayModeOptionsEnabled = true;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
            }

#if UNITY_EDITOR
            try
            {
                var simulatorWasOn = ToggleXRSimulator.IsActivated();
                if (simulatorWasOn)
                    ToggleXRSimulator.DisableSimulator();
            }
            catch
            {
            }
#endif
            
            EditorApplication.isPlaying = true;
        }
    }
}
#endif