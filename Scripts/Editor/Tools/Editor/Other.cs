using System;
using System.Reflection;
using UnityEditor;

namespace MyTools
{
    internal static class Other
    {
        [MenuItem(Menus.EDITOR_MENU + "Clear Console %l", priority = Menus.EDITOR_INDEX + 900)] // Ctrl+L
        static void ClearConsole()
        {
            Functions.ClearConsole();
        }
    }
}