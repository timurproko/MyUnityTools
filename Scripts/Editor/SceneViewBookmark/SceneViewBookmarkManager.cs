using UnityEditor;
using UnityEngine;
using SceneViewNavigation;

namespace SceneViewBookmarks
{
    static class SceneViewBookmarkManager
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
            // Bookmark the current scene view so that we can easily return to it later.
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

            // My Addition to Sync with SceneViewTools
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

            // My Addition to Sync with SceneViewTools
            bookmark.type = SceneViewNavigationIO.ReadFromEditorPrefs();
            
            if (slot != previousViewSlot)
            {
                Debug.Log("MyTools: Bookmarked Scene View in Slot " + slot);
            }
        }

        static string GetEditorPrefsKey(int slot)
        {
            return "sceneViewBookmark" + slot;
        }

        static SceneViewBookmark ReadFromEditorPrefs(int slot)
        {
            var key = GetEditorPrefsKey(slot);
            var json = EditorPrefs.GetString(key);
            return JsonUtility.FromJson<SceneViewBookmark>(json);
        }

        static void WriteToEditorPrefs(int slot, SceneViewBookmark bookmark)
        {
            var key = GetEditorPrefsKey(slot);
            var json = JsonUtility.ToJson(bookmark);
            EditorPrefs.SetString(key, json);
        }
    }
}