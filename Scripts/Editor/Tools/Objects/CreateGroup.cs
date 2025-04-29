using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MyTools
{
    internal static class CreateGroup
    {
        static string baseName = "Group";

        [MenuItem(Menus.OBJECT_MENU + "Group %g", validate = true, priority = Menus.OBJECT_INDEX + 101)] // Ctrl+G
        static bool ValidateGroup()
        {
            return Selection.transforms.Length > 0;
        }

        [MenuItem(Menus.OBJECT_MENU + "Group %g", priority = Menus.OBJECT_INDEX + 101)] // Ctrl+G
        static void Group()
        {
            var selectedObjects = Selection.transforms;
            if (selectedObjects.Length == 0)
                return;

            var groupPosition = GetCenterPosition(selectedObjects);
            var groupObject = GetEmptyObject(groupPosition);
            Undo.RegisterCreatedObjectUndo(groupObject, "Create Group");

            foreach (var obj in selectedObjects)
            {
                Undo.SetTransformParent(obj, groupObject.transform, "Group Selected Objects");
            }

            SelectAndRename(groupObject);
        }

        private static void SelectAndRename(GameObject groupObject)
        {
            Selection.activeGameObject = groupObject;
            EditorApplication.delayCall += () => { EditorApplication.ExecuteMenuItem("Edit/Rename"); };
        }

        private static GameObject GetEmptyObject(Vector3 position)
        {
            string uniqueName = GetUniqueGameObjectName(baseName);
            GameObject groupObject = new GameObject(uniqueName)
            {
                transform =
                {
                    position = position,
                    rotation = Quaternion.identity,
                    localScale = Vector3.one
                }
            };

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                groupObject.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
            }

            return groupObject;
        }

        static string GetUniqueGameObjectName(string baseName)
        {
            int count = 1;
            string newName = baseName;

            while (GameObject.Find(newName) != null)
            {
                newName = baseName + " (" + count + ")";
                count++;
            }

            return newName;
        }

        static Vector3 GetCenterPosition(Transform[] objects)
        {
            if (objects.Length == 1)
                return objects[0].position;

            var bounds = new Bounds(objects[0].position, Vector3.zero);
            for (int i = 1; i < objects.Length; i++)
            {
                bounds.Encapsulate(objects[i].position);
            }
            return bounds.center;
        }
    }
}
