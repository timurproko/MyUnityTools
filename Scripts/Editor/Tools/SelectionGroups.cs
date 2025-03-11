using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    public static class SelectionGroups
    {
        private const string PrefsKeyPrefix = "SelectionSlot_";

        private static string GetProjectSpecificKey(int slot)
        {
            string projectName = Application.productName;
            return $"{PrefsKeyPrefix}{projectName}_{slot}";
        }

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 1 #1", priority = Menus.SELECTION_INDEX + 100)] // Shift+1
        private static void SaveSelectionSlot1() => SaveSelection(1);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 2 #2", priority = Menus.SELECTION_INDEX + 101)] // Shift+2
        private static void SaveSelectionSlot2() => SaveSelection(2);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 3 #3", priority = Menus.SELECTION_INDEX + 102)] // Shift+3
        private static void SaveSelectionSlot3() => SaveSelection(3);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 4 #4", priority = Menus.SELECTION_INDEX + 103)] // Shift+4
        private static void SaveSelectionSlot4() => SaveSelection(4);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 5 #5", priority = Menus.SELECTION_INDEX + 104)] // Shift+5
        private static void SaveSelectionSlot5() => SaveSelection(5);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 6 #6", priority = Menus.SELECTION_INDEX + 105)] // Shift+6
        private static void SaveSelectionSlot6() => SaveSelection(6);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 7 #7", priority = Menus.SELECTION_INDEX + 106)] // Shift+7
        private static void SaveSelectionSlot7() => SaveSelection(7);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 8 #8", priority = Menus.SELECTION_INDEX + 107)] // Shift+8
        private static void SaveSelectionSlot8() => SaveSelection(8);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 9 #9", priority = Menus.SELECTION_INDEX + 108)] // Shift+9
        private static void SaveSelectionSlot9() => SaveSelection(9);

        [MenuItem(Menus.SELECTION_MENU + "Save Selection 0 #0", priority = Menus.SELECTION_INDEX + 109)] // Shift+0
        private static void SaveSelectionSlot10() => SaveSelection(10);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200, validate = true)] // 1
        private static bool ValidateLoadSelection1() => HasSavedSelection(1);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200)] // 1
        private static void LoadSelectionSlot1() => LoadSelection(1);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201, validate = true)] // 2
        private static bool ValidateLoadSelection2() => HasSavedSelection(2);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201)] // 2
        private static void LoadSelectionSlot2() => LoadSelection(2);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202, validate = true)] // 3
        private static bool ValidateLoadSelection3() => HasSavedSelection(3);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202)] // 3
        private static void LoadSelectionSlot3() => LoadSelection(3);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203, validate = true)] // 4
        private static bool ValidateLoadSelection4() => HasSavedSelection(4);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203)] // 4
        private static void LoadSelectionSlot4() => LoadSelection(4);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204, validate = true)] // 5
        private static bool ValidateLoadSelection5() => HasSavedSelection(5);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204)] // 5
        private static void LoadSelectionSlot5() => LoadSelection(5);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205, validate = true)] // 6
        private static bool ValidateLoadSelection6() => HasSavedSelection(6);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205)] // 6
        private static void LoadSelectionSlot6() => LoadSelection(6);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206, validate = true)] // 7
        private static bool ValidateLoadSelection7() => HasSavedSelection(7);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206)] // 7
        private static void LoadSelectionSlot7() => LoadSelection(7);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207, validate = true)] // 8
        private static bool ValidateLoadSelection8() => HasSavedSelection(8);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207)] // 8
        private static void LoadSelectionSlot8() => LoadSelection(8);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208, validate = true)] // 9
        private static bool ValidateLoadSelection9() => HasSavedSelection(9);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208)] // 9
        private static void LoadSelectionSlot9() => LoadSelection(9);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209, validate = true)] // 0
        private static bool ValidateLoadSelection10() => HasSavedSelection(10);

        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209)] // 0
        private static void LoadSelectionSlot10() => LoadSelection(10);

        private static void SaveSelection(int slot)
        {
            var selectedObjects = Selection.objects;
            if (selectedObjects.Length == 0)
                return;

            var identifiers = new List<string>();
            foreach (var obj in selectedObjects)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                {
                    identifiers.Add($"INSTANCE:{obj.GetInstanceID()}");
                }
                else
                {
                    var guid = AssetDatabase.AssetPathToGUID(path);
                    identifiers.Add($"GUID:{guid}");
                }
            }

            var key = GetProjectSpecificKey(slot);
            EditorPrefs.SetString(key, string.Join(";", identifiers));
            Debug.Log($"MyTools: Selection saved to slot {slot}.");
        }

        private static void LoadSelection(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            var savedIdentifiers = EditorPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(savedIdentifiers))
                return;

            var identifiers = savedIdentifiers.Split(';');
            var objectsToSelect = new List<Object>();

            foreach (var identifier in identifiers)
            {
                if (identifier.StartsWith("INSTANCE:"))
                {
                    // Scene object: use instance ID
                    var instanceID = int.Parse(identifier.Substring("INSTANCE:".Length));
                    var obj = EditorUtility.InstanceIDToObject(instanceID);
                    if (obj != null)
                    {
                        objectsToSelect.Add(obj);
                    }
                }
                else if (identifier.StartsWith("GUID:"))
                {
                    // Asset: use GUID
                    var guid = identifier.Substring("GUID:".Length);
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                        if (obj != null)
                        {
                            objectsToSelect.Add(obj);
                        }
                    }
                }
            }

            Selection.objects = objectsToSelect.ToArray();
        }

        private static bool HasSavedSelection(int slot)
        {
            var key = GetProjectSpecificKey(slot);
            return !string.IsNullOrEmpty(EditorPrefs.GetString(key, string.Empty));
        }
    }
}