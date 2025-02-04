using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    internal static class SceneViewBookmarkManager
    {
        public const string iconPath = "Packages/com.matthewminer.sceneviewbookmarks/Icons/SceneViewBookmarksIcon.png";
        public const int maxBookmarkCount = 9;

        const int previousViewSlot = 0;

        public static bool hasPreviousView => HasBookmark(previousViewSlot);

        public static bool HasBookmark(int slot)
        {
            var key = GetEditorPrefsKey(slot);
            return EditorPrefs.HasKey(key);
        }

        public static void MoveToBookmark(int slot)
        {
            if (slot != previousViewSlot)
            {
                SetBookmark(previousViewSlot);
            }

            var bookmark = ReadFromEditorPrefs(slot);
            var sceneView = SceneView.lastActiveSceneView;
            sceneView.pivot = bookmark.pivot;
            sceneView.orthographic = bookmark.orthographic;
            if (!sceneView.in2DMode) sceneView.rotation = bookmark.rotation;
            sceneView.size = bookmark.size;

            var type = bookmark.type;
            ActiveSceneView.SceneViewType = type;
            SceneViewNavigationIO.WriteToEditorPrefs(type);
        }

        public static void ReturnToPreviousView()
        {
            MoveToBookmark(previousViewSlot);
        }

        public static void SetBookmark(int slot)
        {
            var bookmark = new SceneViewBookmark(SceneView.lastActiveSceneView);
            WriteToEditorPrefs(slot, bookmark);

            bookmark.type = SceneViewNavigationIO.ReadFromEditorPrefs();
            
            if (slot != previousViewSlot)
            {
                Debug.Log("MyTools: Bookmarked Scene View in Slot " + slot);
            }
        }

        private static string GetEditorPrefsKey(int slot)
        {
            var projectName = Application.productName;
            return $"sceneViewBookmark_{projectName}_{slot}";
        }

        private static SceneViewBookmark ReadFromEditorPrefs(int slot)
        {
            var key = GetEditorPrefsKey(slot);
            var json = EditorPrefs.GetString(key);
            return JsonUtility.FromJson<SceneViewBookmark>(json);
        }

        private static void WriteToEditorPrefs(int slot, SceneViewBookmark bookmark)
        {
            var key = GetEditorPrefsKey(slot);
            var json = JsonUtility.ToJson(bookmark);
            EditorPrefs.SetString(key, json);
        }
    }
}