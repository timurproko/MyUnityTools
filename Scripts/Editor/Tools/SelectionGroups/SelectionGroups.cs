#if UNITY_EDITOR
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

        // --- Save slots ---
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 1 #1", priority = Menus.SELECTION_INDEX + 100)] private static void SaveSelectionSlot1() { if (State.disabled) return; SaveSelection(1); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 2 #2", priority = Menus.SELECTION_INDEX + 101)] private static void SaveSelectionSlot2() { if (State.disabled) return; SaveSelection(2); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 3 #3", priority = Menus.SELECTION_INDEX + 102)] private static void SaveSelectionSlot3() { if (State.disabled) return; SaveSelection(3); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 4 #4", priority = Menus.SELECTION_INDEX + 103)] private static void SaveSelectionSlot4() { if (State.disabled) return; SaveSelection(4); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 5 #5", priority = Menus.SELECTION_INDEX + 104)] private static void SaveSelectionSlot5() { if (State.disabled) return; SaveSelection(5); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 6 #6", priority = Menus.SELECTION_INDEX + 105)] private static void SaveSelectionSlot6() { if (State.disabled) return; SaveSelection(6); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 7 #7", priority = Menus.SELECTION_INDEX + 106)] private static void SaveSelectionSlot7() { if (State.disabled) return; SaveSelection(7); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 8 #8", priority = Menus.SELECTION_INDEX + 107)] private static void SaveSelectionSlot8() { if (State.disabled) return; SaveSelection(8); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 9 #9", priority = Menus.SELECTION_INDEX + 108)] private static void SaveSelectionSlot9() { if (State.disabled) return; SaveSelection(9); }
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 0 #0", priority = Menus.SELECTION_INDEX + 109)] private static void SaveSelectionSlot10(){ if (State.disabled) return; SaveSelection(10); }

        // validate for Save (just disable when plugin disabled)
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 1 #1", validate = true, priority = Menus.SELECTION_INDEX + 100)] private static bool ValidateSaveSelectionSlot1()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 2 #2", validate = true, priority = Menus.SELECTION_INDEX + 101)] private static bool ValidateSaveSelectionSlot2()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 3 #3", validate = true, priority = Menus.SELECTION_INDEX + 102)] private static bool ValidateSaveSelectionSlot3()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 4 #4", validate = true, priority = Menus.SELECTION_INDEX + 103)] private static bool ValidateSaveSelectionSlot4()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 5 #5", validate = true, priority = Menus.SELECTION_INDEX + 104)] private static bool ValidateSaveSelectionSlot5()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 6 #6", validate = true, priority = Menus.SELECTION_INDEX + 105)] private static bool ValidateSaveSelectionSlot6()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 7 #7", validate = true, priority = Menus.SELECTION_INDEX + 106)] private static bool ValidateSaveSelectionSlot7()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 8 #8", validate = true, priority = Menus.SELECTION_INDEX + 107)] private static bool ValidateSaveSelectionSlot8()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 9 #9", validate = true, priority = Menus.SELECTION_INDEX + 108)] private static bool ValidateSaveSelectionSlot9()  => !State.disabled;
        [MenuItem(Menus.SELECTION_MENU + "Save Selection 0 #0", validate = true, priority = Menus.SELECTION_INDEX + 109)] private static bool ValidateSaveSelectionSlot10() => !State.disabled;

        // --- Load slots ---
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200, validate = true)] private static bool ValidateLoadSelection1()  => !State.disabled && HasValidSavedSelection(1);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 1 _1", priority = Menus.SELECTION_INDEX + 200)] private static void LoadSelectionSlot1()  { if (State.disabled) return; LoadSelection(1); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201, validate = true)] private static bool ValidateLoadSelection2()  => !State.disabled && HasValidSavedSelection(2);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 2 _2", priority = Menus.SELECTION_INDEX + 201)] private static void LoadSelectionSlot2()  { if (State.disabled) return; LoadSelection(2); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202, validate = true)] private static bool ValidateLoadSelection3()  => !State.disabled && HasValidSavedSelection(3);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 3 _3", priority = Menus.SELECTION_INDEX + 202)] private static void LoadSelectionSlot3()  { if (State.disabled) return; LoadSelection(3); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203, validate = true)] private static bool ValidateLoadSelection4()  => !State.disabled && HasValidSavedSelection(4);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 4 _4", priority = Menus.SELECTION_INDEX + 203)] private static void LoadSelectionSlot4()  { if (State.disabled) return; LoadSelection(4); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204, validate = true)] private static bool ValidateLoadSelection5()  => !State.disabled && HasValidSavedSelection(5);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 5 _5", priority = Menus.SELECTION_INDEX + 204)] private static void LoadSelectionSlot5()  { if (State.disabled) return; LoadSelection(5); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205, validate = true)] private static bool ValidateLoadSelection6()  => !State.disabled && HasValidSavedSelection(6);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 6 _6", priority = Menus.SELECTION_INDEX + 205)] private static void LoadSelectionSlot6()  { if (State.disabled) return; LoadSelection(6); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206, validate = true)] private static bool ValidateLoadSelection7()  => !State.disabled && HasValidSavedSelection(7);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 7 _7", priority = Menus.SELECTION_INDEX + 206)] private static void LoadSelectionSlot7()  { if (State.disabled) return; LoadSelection(7); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207, validate = true)] private static bool ValidateLoadSelection8()  => !State.disabled && HasValidSavedSelection(8);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 8 _8", priority = Menus.SELECTION_INDEX + 207)] private static void LoadSelectionSlot8()  { if (State.disabled) return; LoadSelection(8); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208, validate = true)] private static bool ValidateLoadSelection9()  => !State.disabled && HasValidSavedSelection(9);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 9 _9", priority = Menus.SELECTION_INDEX + 208)] private static void LoadSelectionSlot9()  { if (State.disabled) return; LoadSelection(9); }
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209, validate = true)] private static bool ValidateLoadSelection10() => !State.disabled && HasValidSavedSelection(10);
        [MenuItem(Menus.SELECTION_MENU + "Load Selection 0 _0", priority = Menus.SELECTION_INDEX + 209)] private static void LoadSelectionSlot10(){ if (State.disabled) return; LoadSelection(10); }

        // --- Remove from selection ---
        [MenuItem(Menus.SELECTION_MENU + "Remove From Selection #BACKSPACE", priority = Menus.SELECTION_INDEX + 300, validate = true)]
        private static bool ValidateRemoveFromSelection()
        {
            if (State.disabled) return false;
            return Selection.gameObjects.Any(go => selectionPrefixRegex.IsMatch(go.name));
        }

        [MenuItem(Menus.SELECTION_MENU + "Remove From Selection #BACKSPACE", priority = Menus.SELECTION_INDEX + 300)]
        private static void RemoveFromSelection()
        {
            if (State.disabled) return;

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
            if (State.disabled) return;

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
            Utils.Log($"Selection saved to slot {displaySlot}.");

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        private static void LoadSelection(int slot)
        {
            if (State.disabled) return;

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
#endif
