using System;
using UnityEditor;
using System.Reflection;
using UnityEngine;

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
    }
}