using System;
using UnityEditor;

namespace MyTools
{
    public static class MyTools
    {
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
    }
}