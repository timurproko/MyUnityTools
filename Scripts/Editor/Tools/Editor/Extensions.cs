#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace MyTools
{
    internal static class Extensions
    {
        private static readonly BuildTargetGroup[] BuildTargets = {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WebGL
        };

        private static void ToggleSymbol(string symbol, string menuPath)
        {
            bool enable = !IsSymbolDefined(symbol);

            foreach (var target in BuildTargets)
            {
                var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target).Split(';').ToList();

                if (enable)
                {
                    if (!symbols.Contains(symbol))
                        symbols.Add(symbol);
                }
                else
                {
                    symbols.RemoveAll(s => s == symbol);
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", symbols));
            }

            Menu.SetChecked(menuPath, enable);
        }
        
        private static bool IsSymbolDefined(string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            return symbols.Split(';').Contains(symbol);
        }

        private static bool ValidateSymbolToggle(string symbol, string menuPath)
        {
            bool defined = IsSymbolDefined(symbol);
            Menu.SetChecked(menuPath, defined);
            return true;
        }
        
        // ANIMANCER
        private const string Animancer = "ANIMANCER";
        private const string Menu_Animancer = Menus.EXTENSIONS_MENU + Animancer;

        [MenuItem(Menu_Animancer, priority = Menus.EDITOR_INDEX + 101)]
        private static void Toggle_Animancer() => ToggleSymbol(Animancer, Menu_Animancer);

        [MenuItem(Menu_Animancer, true)]
        private static bool Validate_Animancer() => ValidateSymbolToggle(Animancer, Menu_Animancer);
        
        // FMOD
        private const string FMOD = "FMOD";
        private const string Menu_FMOD = Menus.EXTENSIONS_MENU + FMOD;

        [MenuItem(Menu_FMOD, priority = Menus.EDITOR_INDEX + 102)]
        private static void Toggle_FMOD() => ToggleSymbol(FMOD, Menu_FMOD);

        [MenuItem(Menu_FMOD, true)]
        private static bool Validate_FMOD() => ValidateSymbolToggle(FMOD, Menu_FMOD);
        
    }
}
#endif