#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace MyTools
{
    public static class Functions
    {
        public static EditorWindow GetView(string name)
        {
            Type viewType = typeof(UnityEditor.Editor).Assembly.GetType(name);
            return EditorWindow.GetWindow(viewType);
        }
        
        public static void ActivateWindowUnderCursor()
        {
            EditorWindow windowUnderCursor = EditorWindow.mouseOverWindow;

            if (windowUnderCursor != null)
            {
                windowUnderCursor.Focus();
            }
        }
        
        public static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
}
#endif