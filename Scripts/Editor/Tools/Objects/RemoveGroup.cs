#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    internal static class RemoveGroup
    {
        [MenuItem(Menus.OBJECT_MENU + "Ungroup %u", validate = true, priority = Menus.OBJECT_INDEX + 102)] // Ctrl+U
        static bool ValidateUngroup()
        {
            if (Selection.transforms.Length == 0)
                return false;

            foreach (var t in Selection.transforms)
            {
                if (t.childCount > 0)
                    return true;
            }

            return false;
        }

        [MenuItem(Menus.OBJECT_MENU + "Ungroup %u", priority = Menus.OBJECT_INDEX + 102)] // Ctrl+U
        static void Ungroup()
        {
            var selectedObjects = Selection.transforms;
            if (selectedObjects.Length == 0)
                return;

            foreach (var group in selectedObjects)
            {
                if (group.childCount == 0)
                    continue;

                Undo.RegisterFullObjectHierarchyUndo(group, "Ungroup");

                Transform parent = group.parent;
                int index = group.GetSiblingIndex();

                while (group.childCount > 0)
                {
                    Transform child = group.GetChild(0);
                    Undo.SetTransformParent(child, parent, "Ungroup");
                    child.SetSiblingIndex(index);
                    index++;
                }

                Undo.DestroyObjectImmediate(group.gameObject);
            }
        }
    }
}
#endif