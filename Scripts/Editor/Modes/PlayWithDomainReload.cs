#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    internal static class PlayWithDomainReload
    {
        private static EnterPlayModeOptions _originalOptions;
        private static bool _wasFastPlayEnabled;

        [MenuItem(Menus.MY_TOOLS_MENU + "Play With Reload %&#p", priority = Menus.MODES_INDEX + 999)]
        static void PlayDomainReload()
        {
            if (State.disabled) return;

            if (EditorApplication.isPlaying)
                return;

            _wasFastPlayEnabled = EditorSettings.enterPlayModeOptionsEnabled;
            _originalOptions = EditorSettings.enterPlayModeOptions;

            if (_wasFastPlayEnabled)
            {
                EditorSettings.enterPlayModeOptionsEnabled = true;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
            }

            EditorApplication.playModeStateChanged += RestoreFastPlayModeAfterExit;
            EditorApplication.isPlaying = true;
        }

        [MenuItem(Menus.MY_TOOLS_MENU + "Play With Reload %&#p", validate = true, priority = Menus.MODES_INDEX + 999)]
        static bool ValidatePlayDomainReload() => !State.disabled;

        private static void RestoreFastPlayModeAfterExit(PlayModeStateChange state)
        {
            if (State.disabled)
            {
                EditorApplication.playModeStateChanged -= RestoreFastPlayModeAfterExit;
                return;
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (_wasFastPlayEnabled)
                {
                    EditorSettings.enterPlayModeOptions = _originalOptions;
                    EditorSettings.enterPlayModeOptionsEnabled = true;
                }

                EditorApplication.playModeStateChanged -= RestoreFastPlayModeAfterExit;
            }
        }
    }
}
#endif