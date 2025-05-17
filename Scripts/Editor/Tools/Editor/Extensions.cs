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
        private const string Extension1 = "ANIMANCER";
        private const string Menu_Extension1 = Menus.EXTENSIONS_MENU + Extension1;

        [MenuItem(Menu_Extension1, priority = Menus.EDITOR_INDEX + 100)]
        private static void Toggle_Extension1() => ToggleSymbol(Extension1, Menu_Extension1);

        [MenuItem(Menu_Extension1, true)]
        private static bool Validate_Extension1() => ValidateSymbolToggle(Extension1, Menu_Extension1);
        
        // FMOD
        private const string Extension2 = "FMOD";
        private const string Menu_Extension2 = Menus.EXTENSIONS_MENU + Extension2;

        [MenuItem(Menu_Extension2, priority = Menus.EDITOR_INDEX + 101)]
        private static void Toggle_Extension2() => ToggleSymbol(Extension2, Menu_Extension2);

        [MenuItem(Menu_Extension2, true)]
        private static bool Validate_Extension2() => ValidateSymbolToggle(Extension2, Menu_Extension2);
    }
}