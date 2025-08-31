#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace MyTools
{
    internal static class Extensions
    {
        private static readonly NamedBuildTarget[] BuildTargets = {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
            NamedBuildTarget.WebGL
        };

        private static void ToggleSymbol(string symbol, string menuPath)
        {
            if (State.disabled) return;

            bool enable = !IsSymbolDefined(symbol);

            foreach (var target in BuildTargets)
            {
                var symbols = PlayerSettings.GetScriptingDefineSymbols(target)
                    .Split(';')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();

                if (enable)
                {
                    if (!symbols.Contains(symbol))
                        symbols.Add(symbol);
                }
                else
                {
                    symbols.RemoveAll(s => s == symbol);
                }

                PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", symbols));
            }

            Menu.SetChecked(menuPath, enable);
        }

        private static bool IsSymbolDefined(string symbol)
        {
            var nbt = NamedBuildTarget.FromBuildTargetGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);

            var symbols = PlayerSettings.GetScriptingDefineSymbols(nbt);
            return symbols.Split(';').Contains(symbol);
        }

        private static bool ValidateToggleSymbol(string symbol, string menuPath)
        {
            bool defined = IsSymbolDefined(symbol);
            Menu.SetChecked(menuPath, defined);
            return !State.disabled;
        }

        // ANIMANCER
        private const string Animancer = "ANIMANCER";
        private const string Menu_Animancer = Menus.GLOBAL_MENU + Animancer;

        [MenuItem(Menu_Animancer, priority = Menus.GLOBAL_INDEX + 101)]
        private static void Toggle_Animancer() => ToggleSymbol(Animancer, Menu_Animancer);

        [MenuItem(Menu_Animancer, true)]
        private static bool Validate_Animancer() => ValidateToggleSymbol(Animancer, Menu_Animancer);

        // FMOD
        private const string FMOD = "FMOD";
        private const string Menu_FMOD = Menus.GLOBAL_MENU + FMOD;

        [MenuItem(Menu_FMOD, priority = Menus.GLOBAL_INDEX + 102)]
        private static void Toggle_FMOD() => ToggleSymbol(FMOD, Menu_FMOD);

        [MenuItem(Menu_FMOD, true)]
        private static bool Validate_FMOD() => ValidateToggleSymbol(FMOD, Menu_FMOD);
        
        // VCONTAINER
        private const string VCONTAINER = "VCONTAINER";
        private const string Menu_VCONTAINER = Menus.GLOBAL_MENU + VCONTAINER;

        [MenuItem(Menu_VCONTAINER, priority = Menus.GLOBAL_INDEX + 102)]
        private static void Toggle_VCONTAINER() => ToggleSymbol(VCONTAINER, Menu_VCONTAINER);

        [MenuItem(Menu_VCONTAINER, true)]
        private static bool Validate_VCONTAINER() => ValidateToggleSymbol(VCONTAINER, Menu_VCONTAINER);
    }
}
#endif
