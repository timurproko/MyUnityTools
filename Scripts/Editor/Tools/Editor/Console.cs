#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    internal static class Console
    {
        [MenuItem(Menus.EDITOR_MENU + "Clear Console %l", priority = Menus.EDITOR_INDEX + 400)] // Ctrl+L
        static void Clear()
        {
            if (State.disabled) return;

            Utils.ClearConsole();
        }
        
        [MenuItem(Menus.EDITOR_MENU + "Clear Console %l", validate = true)]
        static bool ValidateClear()
        {
            return !State.disabled;
        }
    }
}
#endif