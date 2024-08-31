using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace MyTools
{
    class Duplicate : MonoBehaviour
    {
        [MenuItem("My Tools/Duplicate %d", priority = 16)] // Ctrl+D
        static void DuplicateGameObject()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
            {
                // Don't break default behavior elsewhere
                EditorApplication.ExecuteMenuItem("Edit/Duplicate");
                return;
            }

            // Prepare to keep track of the newly created objects
            List<int> newSelectList = new List<int>();
            int currentIndex = 1;  // Start from 1 instead of 0
            int digitCount = 0;

            // Find the largest index and name pattern among selected objects
            string baseName = "";
            foreach (GameObject selectedObject in selectedObjects)
            {
                baseName = ExtractBaseName(selectedObject.name, ref currentIndex, ref digitCount);
            }

            // Determine the position to place the duplicated objects
            int lastSelectedIndex = -1;
            Transform parentTransform = null;

            foreach (GameObject selectedObject in selectedObjects)
            {
                if (selectedObject != null)
                {
                    lastSelectedIndex = Mathf.Max(lastSelectedIndex, selectedObject.transform.GetSiblingIndex());
                    parentTransform = selectedObject.transform.parent;
                }
            }

            foreach (GameObject selectedObject in selectedObjects)
            {
                if (selectedObject != null)
                {
                    GameObject duplicate = DuplicateObject(selectedObject, currentIndex, digitCount);

                    // Register Undo
                    Undo.RegisterCreatedObjectUndo(duplicate, "Duplicated GameObject");

                    // Add to the selection list
                    newSelectList.Add(duplicate.GetInstanceID());

                    // Increment the index for the next object
                    currentIndex++;

                    // Move the duplicate below all selected objects
                    duplicate.transform.SetSiblingIndex(lastSelectedIndex + 1);
                    lastSelectedIndex++;
                }
            }

            // Select new objects
            if (newSelectList.Count > 0)
            {
                Selection.instanceIDs = newSelectList.ToArray();
            }
        }

        private static GameObject DuplicateObject(GameObject selectedObject, int index, int digitCount)
        {
            GameObject duplicate;
            // Check if the selected object is a prefab instance
            var prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(selectedObject);

            if (!string.IsNullOrEmpty(prefabAssetPath))
            {
                // Instantiate the prefab
                duplicate = (GameObject)PrefabUtility.InstantiatePrefab(
                    PrefabUtility.GetCorrespondingObjectFromOriginalSource(selectedObject),
                    selectedObject.transform.parent);

                // Set position and rotation to match the original
                duplicate.transform.SetPositionAndRotation(
                    selectedObject.transform.position,
                    selectedObject.transform.rotation);
            }
            else
            {
                // Fall back to normal duplication if it's not a prefab
                duplicate = Instantiate(selectedObject,
                    selectedObject.transform.position,
                    selectedObject.transform.rotation,
                    selectedObject.transform.parent);
            }

            // Rename and increment
            duplicate.name = IncrementName(selectedObject.name, index, digitCount);

            return duplicate;
        }

        private static string ExtractBaseName(string input, ref int currentIndex, ref int digitCount)
        {
            var dupNumberRegex = new Regex("(\\d+)$");
            var dupParenthesisRegex = new Regex(@"\((\d+)\)$");

            var dupNumberMatch = dupNumberRegex.Match(input).Value;
            var dupParenthesisMatch = dupParenthesisRegex.Match(input).Groups[1].Value;

            if (!string.IsNullOrEmpty(dupParenthesisMatch) && int.TryParse(dupParenthesisMatch, out int parsedParenthesisNumber))
            {
                currentIndex = Mathf.Max(currentIndex, parsedParenthesisNumber + 1);
                digitCount = dupParenthesisMatch.Length;
                return dupParenthesisRegex.Replace(input, "").TrimEnd();
            }
            else if (int.TryParse(dupNumberMatch, out int parsedNumber))
            {
                currentIndex = Mathf.Max(currentIndex, parsedNumber + 1);
                digitCount = dupNumberMatch.Length;
                return dupNumberRegex.Replace(input, "");
            }
            else
            {
                currentIndex = 1;
                digitCount = 0;  // No digits to pad by default
                return input;
            }
        }

        private static string IncrementName(string input, int index, int digitCount)
        {
            var dupNumberRegex = new Regex("(\\d+)$");
            var dupParenthesisRegex = new Regex(@"\((\d+)\)$");

            var baseName = dupNumberRegex.Replace(input, "");
            baseName = dupParenthesisRegex.Replace(baseName, "").TrimEnd();

            if (dupParenthesisRegex.IsMatch(input))
            {
                return $"{baseName} ({index})";
            }

            string formattedIndex = digitCount > 0 ? index.ToString().PadLeft(digitCount, '0') : index.ToString();
            return $"{baseName}{formattedIndex}";
        }
    }
}
