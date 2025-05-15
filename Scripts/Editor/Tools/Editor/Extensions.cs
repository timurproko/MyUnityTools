using System.Collections.Generic;
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
            ForceDomainReload();
        }

        private static void ForceDomainReload()
        {
            string dummyFile = "Assets/~domain_reload_trigger.cs";
            System.IO.File.WriteAllText(dummyFile, "// domain reload trigger");
            AssetDatabase.ImportAsset(dummyFile, ImportAssetOptions.ForceUpdate);
            System.IO.File.Delete(dummyFile);
            System.IO.File.Delete(dummyFile + ".meta");
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

        // FMOD installer
        private const string FMODSymbol = "FMOD";
        private const string FMODMenuPath = Menus.EXTENSIONS_MENU + "FMOD";

        [MenuItem(FMODMenuPath, priority = Menus.EDITOR_INDEX + 100)]
        private static void ToggleFMOD() => ToggleSymbol(FMODSymbol, FMODMenuPath);

        [MenuItem(FMODMenuPath, true)]
        private static bool ValidateFMOD() => ValidateSymbolToggle(FMODSymbol, FMODMenuPath);
    }
}
