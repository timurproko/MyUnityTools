using System;
using System.Reflection;
using UnityEditor;

namespace MyTools
{
    public static class MyTools
    {
        private const string MY_TOOLS_MENU = "My Tools/";
        public const string ASSETS_AND_PREFABS_MENU = MY_TOOLS_MENU + "Assets && Prefabs/";
        public const string SCENE_VIEW_MENU = MY_TOOLS_MENU + "Scene View Tools/";
        public const string UNITY_EDITOR_MENU = MY_TOOLS_MENU + "Unity Editor/";
        
        public static EditorWindow GetView(string name)
        {
            Type viewType = typeof(Editor).Assembly.GetType(name);
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