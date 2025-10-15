#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    static class SceneViewBookmarkMenu
    {
        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 1 %1", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark1()
        {
            if (State.disabled) return;
            _MoveToBookmark1();
            _MoveToBookmark1();
        }
        static void _MoveToBookmark1() => SceneViewBookmarkManager.MoveToBookmark(1);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 2 %2", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark2()
        {
            if (State.disabled) return;
            _MoveToBookmark2();
            _MoveToBookmark2();
        }
        static void _MoveToBookmark2() => SceneViewBookmarkManager.MoveToBookmark(2);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 3 %3", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark3()
        {
            if (State.disabled) return;
            _MoveToBookmark3();
            _MoveToBookmark3();
        }
        static void _MoveToBookmark3() => SceneViewBookmarkManager.MoveToBookmark(3);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 4 %4", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark4()
        {
            if (State.disabled) return;
            _MoveToBookmark4();
            _MoveToBookmark4();
        }
        static void _MoveToBookmark4() => SceneViewBookmarkManager.MoveToBookmark(4);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 5 %5", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark5()
        {
            if (State.disabled) return;
            _MoveToBookmark5();
            _MoveToBookmark5();
        }
        static void _MoveToBookmark5() => SceneViewBookmarkManager.MoveToBookmark(5);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 6 %6", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark6()
        {
            if (State.disabled) return;
            _MoveToBookmark6();
            _MoveToBookmark6();
        }
        static void _MoveToBookmark6() => SceneViewBookmarkManager.MoveToBookmark(6);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 7 %7", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark7()
        {
            if (State.disabled) return;
            _MoveToBookmark7();
            _MoveToBookmark7();
        }
        static void _MoveToBookmark7() => SceneViewBookmarkManager.MoveToBookmark(7);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 8 %8", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark8()
        {
            if (State.disabled) return;
            _MoveToBookmark8();
            _MoveToBookmark8();
        }
        static void _MoveToBookmark8() => SceneViewBookmarkManager.MoveToBookmark(8);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 9 %9", false, Menus.SCENE_VIEW_INDEX + 100)]
        static void MoveToBookmark9()
        {
            if (State.disabled) return;
            _MoveToBookmark9();
            _MoveToBookmark9();
        }
        static void _MoveToBookmark9() => SceneViewBookmarkManager.MoveToBookmark(9);

        [MenuItem(Menus.BOOKMARKS_MENU + "Return to Previous View %0", false, Menus.SCENE_VIEW_INDEX + 200)]
        static void ReturnToPreviousView()
        {
            if (State.disabled) return;
            _ReturnToPreviousView();
            _ReturnToPreviousView();
        }
        static void _ReturnToPreviousView() => SceneViewBookmarkManager.ReturnToPreviousView();

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 1 %&1", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark1() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(1); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 2 %&2", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark2() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(2); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 3 %&3", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark3() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(3); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 4 %&4", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark4() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(4); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 5 %&5", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark5() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(5); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 6 %&6", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark6() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(6); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 7 %&7", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark7() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(7); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 8 %&8", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark8() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(8); }

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 9 %&9", false, Menus.SCENE_VIEW_INDEX + 300)]
        static void SetBookmark9() { if (!State.disabled) SceneViewBookmarkManager.SetBookmark(9); }

        #region Validation
        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 1 %1", true)]
        static bool ValidateMoveToBookmark1() => !State.disabled && SceneViewBookmarkManager.HasBookmark(1);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 2 %2", true)]
        static bool ValidateMoveToBookmark2() => !State.disabled && SceneViewBookmarkManager.HasBookmark(2);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 3 %3", true)]
        static bool ValidateMoveToBookmark3() => !State.disabled && SceneViewBookmarkManager.HasBookmark(3);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 4 %4", true)]
        static bool ValidateMoveToBookmark4() => !State.disabled && SceneViewBookmarkManager.HasBookmark(4);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 5 %5", true)]
        static bool ValidateMoveToBookmark5() => !State.disabled && SceneViewBookmarkManager.HasBookmark(5);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 6 %6", true)]
        static bool ValidateMoveToBookmark6() => !State.disabled && SceneViewBookmarkManager.HasBookmark(6);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 7 %7", true)]
        static bool ValidateMoveToBookmark7() => !State.disabled && SceneViewBookmarkManager.HasBookmark(7);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 8 %8", true)]
        static bool ValidateMoveToBookmark8() => !State.disabled && SceneViewBookmarkManager.HasBookmark(8);

        [MenuItem(Menus.BOOKMARKS_MENU + "Move to Bookmark 9 %9", true)]
        static bool ValidateMoveToBookmark9() => !State.disabled && SceneViewBookmarkManager.HasBookmark(9);

        [MenuItem(Menus.BOOKMARKS_MENU + "Return to Previous View %0", true)]
        static bool ValidateReturnToPreviousView() => !State.disabled && SceneViewBookmarkManager.hasPreviousView;
        
        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 1 %&1", true)]
        static bool ValidateSetBookmark1() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 2 %&2", true)]
        static bool ValidateSetBookmark2() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 3 %&3", true)]
        static bool ValidateSetBookmark3() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 4 %&4", true)]
        static bool ValidateSetBookmark4() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 5 %&5", true)]
        static bool ValidateSetBookmark5() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 6 %&6", true)]
        static bool ValidateSetBookmark6() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 7 %&7", true)]
        static bool ValidateSetBookmark7() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 8 %&8", true)]
        static bool ValidateSetBookmark8() => !State.disabled;

        [MenuItem(Menus.BOOKMARKS_MENU + "Set Bookmark 9 %&9", true)]
        static bool ValidateSetBookmark9() => !State.disabled;

        #endregion
    }
}
#endif
