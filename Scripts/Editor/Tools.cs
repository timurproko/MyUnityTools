using System;
using UnityEditor;

namespace MyTools
{
    static class Tools
    {
        public static void ActivateWindowUnderCursor()
        {
            EditorWindow windowUnderCursor = EditorWindow.mouseOverWindow;

            if (windowUnderCursor != null)
            {
                windowUnderCursor.Focus();
            }
        }

        public static EditorWindow GetView(string name)
        {
            Type viewType = typeof(Editor).Assembly.GetType(name);
            return EditorWindow.GetWindow(viewType);
        }
    }
}