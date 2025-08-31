#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    public static class Utils
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

        public static void Log(string message)
        {
            Debug.Log($"[MyTools] {message}");
        }
        
        public static void LogWarning(string message)
        {
            Debug.LogWarning($"[MyTools] {message}");
        }
        
        public static void LogError(string message)
        {
            Debug.LogError($"[MyTools] {message}");
        }
    }
}
#endif