using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MyTools
{
    public static class SelectionGroups
    {
        private const string PrefsKeyPrefix = "SelectionSlot_";
        private static readonly Regex selectionPrefixRegex = new Regex(@"^\[Selection\s\d+\]\s", RegexOptions.Compiled);

        private static string GetProjectSpecificKey(int slot)
        {
            int displaySlot = slot == 10 ? 0 : slot;
            string projectName = Application.productName;
            return $"{PrefsKeyPrefix}{projectName}_{displaySlot}";
        }

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 1 #1", priority = Menus.SELECTION_INDEX + 100)] private static void SaveSelectionSlot1() => SaveSelection(1);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 2 #2", priority = Menus.SELECTION_INDEX + 101)] private static void SaveSelectionSlot2() => SaveSelection(2);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 3 #3", priority = Menus.SELECTION_INDEX + 102)] private static void SaveSelectionSlot3() => SaveSelection(3);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 4 #4", priority = Menus.SELECTION_INDEX + 103)] private static void SaveSelectionSlot4() => SaveSelection(4);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 5 #5", priority = Menus.SELECTION_INDEX + 104)] private static void SaveSelectionSlot5() => SaveSelection(5);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 6 #6", priority = Menus.SELECTION_INDEX + 105)] private static void SaveSelectionSlot6() => SaveSelection(6);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 7 #7", priority = Menus.SELECTION_INDEX + 106)] private static void SaveSelectionSlot7() => SaveSelection(7);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 8 #8", priority = Menus.SELECTION_INDEX + 107)] private static void SaveSelectionSlot8() => SaveSelection(8);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 9 #9", priority = Menus.SELECTION_INDEX + 108)] private static void SaveSelectionSlot9() => SaveSelection(9);
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 0 #0", priority = Menus.SELECTION_INDEX + 109)] private static void SaveSelectionSlot10() => SaveSelection(10);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200, validate = true)] private static bool ValidateLoadSelection1() => HasValidSavedSelection(1);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200)] private static void LoadSelectionSlot1() => LoadSelection(1);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201, validate = true)] private static bool ValidateLoadSelection2() => HasValidSavedSelection(2);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201)] private static void LoadSelectionSlot2() => LoadSelection(2);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202, validate = true)] private static bool ValidateLoadSelection3() => HasValidSavedSelection(3);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202)] private static void LoadSelectionSlot3() => LoadSelection(3);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203, validate = true)] private static bool ValidateLoadSelection4() => HasValidSavedSelection(4);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203)] private static void LoadSelectionSlot4() => LoadSelection(4);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204, validate = true)] private static bool ValidateLoadSelection5() => HasValidSavedSelection(5);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204)] private static void LoadSelectionSlot5() => LoadSelection(5);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205, validate = true)] private static bool ValidateLoadSelection6() => HasValidSavedSelection(6);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205)] private static void LoadSelectionSlot6() => LoadSelection(6);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206, validate = true)] private static bool ValidateLoadSelection7() => HasValidSavedSelection(7);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206)] private static void LoadSelectionSlot7() => LoadSelection(7);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207, validate = true)] private static bool ValidateLoadSelection8() => HasValidSavedSelection(8);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207)] private static void LoadSelectionSlot8() => LoadSelection(8);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208, validate = true)] private static bool ValidateLoadSelection9() => HasValidSavedSelection(9);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208)] private static void LoadSelectionSlot9() => LoadSelection(9);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209, validate = true)] private static bool ValidateLoadSelection10() => HasValidSavedSelection(10);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209)] private static void LoadSelectionSlot10() => LoadSelection(10);

        [MenuItem(Menus.SELECTION_MENU + "Remove From Selection #BACKSPACE", priority = Menus.SELECTION_INDEX + 300, validate = true)]
        private static bool ValidateRemoveFromSelection()
        {
            return Selection.gameObjects.Any(go => selectionPrefixRegex.IsMatch(go.name));
        }

        [MenuItem(Menus.SELECTION_MENU + "Remove From Selection #BACKSPACE", priority = Menus.SELECTION_INDEX + 300)]
        private static void RemoveFromSelection()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
                return;

            var slotsToUpdate = new HashSet<int>();
            foreach (var obj in selectedObjects)
            {
                if (selectionPrefixRegex.IsMatch(obj.name))
                {
                    string strippedName = selectionPrefixRegex.Replace(obj.name, "");
                    obj.name = strippedName;

                    for (int slot = 1; slot <= 10; slot++)
                    {
                        var key = GetProjectSpecificKey(slot);
                        var savedNames = EditorPrefs.GetString(key, string.Empty);
                        if (!string.IsNullOrEmpty(savedNames))
                        {
                            int displaySlot = slot == 10 ? 0 : slot;
                            if (savedNames.Split(';').Contains($"[Selection {displaySlot}] {strippedName}"))
                            {
                                slotsToUpdate.Add(slot);
                            }
                        }
                    }
                }
            }

            foreach (var slot in slotsToUpdate)
            {
                UpdateSlotAfterRemoval(slot);
            }
        }

        private static void UpdateSlotAfterRemoval(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            var savedNames = EditorPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(savedNames))
                return;

            var names = savedNames.Split(';');
            var validNames = new List<string>();
            int displaySlot = slot == 10 ? 0 : slot;

            foreach (var name in names)
            {
                var obj = GameObject.Find(name);
                if (obj != null && obj.name == name)
                {
                    validNames.Add(name);
                }
            }

            if (validNames.Count > 0)
            {
                EditorPrefs.SetString(key, string.Join(";", validNames));
            }
            else
            {
                EditorPrefs.DeleteKey(key);
            }
        }

        private static void SaveSelection(int slot)
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
                return;

            int displaySlot = slot == 10 ? 0 : slot;
            var key = GetProjectSpecificKey(slot);

            if (HasSavedSelection(slot))
            {
                var previousNames = EditorPrefs.GetString(key).Split(';');
                foreach (var name in previousNames)
                {
                    var obj = GameObject.Find(name);
                    if (obj != null)
                    {
                        obj.name = selectionPrefixRegex.Replace(obj.name, "");
                    }
                }
            }

            foreach (var obj in selectedObjects)
            {
                obj.name = selectionPrefixRegex.Replace(obj.name, "");
            }

            var names = new List<string>();
            foreach (var obj in selectedObjects)
            {
                obj.name = $"[Selection {displaySlot}] {obj.name}";
                names.Add(obj.name);
            }

            EditorPrefs.SetString(key, string.Join(";", names));
            Debug.Log($"MyTools: Selection saved to slot {displaySlot}.");

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        private static void LoadSelection(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            var savedNames = EditorPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(savedNames))
                return;

            var names = savedNames.Split(';');
            var objectsToSelect = new List<Object>();
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (var name in names)
            {
                var obj = allObjects.FirstOrDefault(go => go.name == name);
                if (obj != null)
                {
                    objectsToSelect.Add(obj);
                }
            }

            Selection.objects = objectsToSelect.ToArray();
        }

        private static bool HasSavedSelection(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            return !string.IsNullOrEmpty(EditorPrefs.GetString(key, string.Empty));
        }

        private static bool HasValidSavedSelection(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            var savedNames = EditorPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(savedNames))
                return false;

            var names = savedNames.Split(';');
            foreach (var name in names)
            {
                var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                if (allObjects.Any(go => go.name == name))
                    return true;
            }

            EditorPrefs.DeleteKey(key);
            return false;
        }
    }
}