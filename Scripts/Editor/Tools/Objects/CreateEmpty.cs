#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MyTools
{
    internal static class CreateEmpty
    {
        static string baseName = "GameObject";

        [MenuItem(Menus.OBJECT_MENU + "Create Empty %&n", priority = Menus.OBJECT_INDEX + 100)] // Ctrl+Alt+N
        static void Create()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                var emptyObject = GetEmptyObject();
                Undo.RegisterCreatedObjectUndo(emptyObject, "Create Empty");
                SelectAndRename(emptyObject);
            }
            else
            {
                foreach (var selectedObject in selectedObjects)
                {
                    var emptyObject = GetEmptyObject();
                    emptyObject.transform.SetParent(selectedObject.transform, false);
                    Undo.RegisterCreatedObjectUndo(emptyObject, "Create Empty Child");
                    SelectAndRename(emptyObject);
                }
            }
        }

        private static void SelectAndRename(GameObject emptyObject)
        {
            Selection.activeGameObject = emptyObject;
            EditorApplication.delayCall += () => { EditorApplication.ExecuteMenuItem("Edit/Rename"); };
        }

        private static GameObject GetEmptyObject()
        {
            string uniqueName = GetUniqueGameObjectName(baseName);
            GameObject emptyObject = new GameObject(uniqueName)
            {
                transform =
                {
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    localScale = Vector3.one
                }
            };

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                emptyObject.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
            }

            return emptyObject;
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
    }
}
#endif
