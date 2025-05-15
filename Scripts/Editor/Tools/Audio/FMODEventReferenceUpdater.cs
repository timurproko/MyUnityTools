#if FMOD
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.SceneManagement;
using System.Linq;

namespace FMODUnity
{
    public class MyEventReferenceUpdater : EditorWindow
    {
        private const string MenuPath = "FMOD/Update Event References in Current Scene";

        float buttonHeight = EditorGUIUtility.singleLineHeight * 2;
        private TreeViewState treeViewState;
        private FMODTaskView taskView;

        private List<FMODComponentTask> tasks = new();

        [MenuItem(MenuPath)]
        public static void ShowWindow()
        {
            MyEventReferenceUpdater updater = GetWindow<MyEventReferenceUpdater>("FMOD Event Reference Updater");
            updater.minSize = new Vector2(800, 600);
            updater.Show();
        }

        private void OnEnable()
        {
            RefreshTasks();
        }

        private void RefreshTasks()
        {
            tasks.Clear();

            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] roots = currentScene.GetRootGameObjects();

            foreach (var root in roots)
            {
                var monoBehaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var behaviour in monoBehaviours)
                {
                    if (behaviour == null) continue;

                    if (behaviour is StudioEventEmitter emitter)
                    {
                        CheckStudioEventEmitter(emitter);
                    }

                    CheckEventReferenceFields(behaviour);
                }
            }

            if (treeViewState == null)
                treeViewState = new TreeViewState();

            taskView = new FMODTaskView(treeViewState, tasks);
            taskView.Reload();
        }

        private void CheckStudioEventEmitter(StudioEventEmitter emitter)
        {
#pragma warning disable 0618
            if (!string.IsNullOrEmpty(emitter.Event))
            {
                AddOrUpdateTask(emitter.gameObject, emitter,
                    typeof(StudioEventEmitter).GetField("Event"),
                    emitter.Event,
                    typeof(StudioEventEmitter).GetField("EventReference"),
                    emitter.EventReference);
            }
#pragma warning restore 0618

            if (!IsReferenceValid(emitter.EventReference))
            {
                AddOrUpdateTask(emitter.gameObject, emitter,
                    null, null,
                    typeof(StudioEventEmitter).GetField("EventReference"),
                    emitter.EventReference);
            }
        }

        private void CheckEventReferenceFields(MonoBehaviour behaviour)
        {
            var fields = behaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(EventReference))
                {
                    var value = (EventReference)field.GetValue(behaviour);
                    if (!IsReferenceValid(value))
                    {
                        AddOrUpdateTask(behaviour.gameObject, behaviour, null, null, field, value);
                    }
                }
            }
        }

        private void AddOrUpdateTask(GameObject go, Object target, FieldInfo oldField, string oldValue, FieldInfo newField, EventReference reference)
        {
            var task = tasks.FirstOrDefault(t => t.Target == target);
            if (task == null)
            {
                task = new FMODComponentTask(go, target);
                tasks.Add(task);
            }

            task.Fields.Add((oldField, oldValue, newField, reference));
        }

        private bool IsReferenceValid(EventReference reference)
        {
            if (reference.IsNull)
                return false;

            if (Settings.Instance.EventLinkage == EventLinkage.Path)
            {
                return !string.IsNullOrEmpty(reference.Path) && EventManager.EventFromPath(reference.Path) != null;
            }
            else
            {
                var editorEvent = EventManager.EventFromGUID(reference.Guid);
                if (editorEvent == null) return false;
                return editorEvent.Path == reference.Path;
            }
        }

        private void OnGUI()
        {
            GUILayout.Space(4);

            var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            if (taskView != null)
                taskView.OnGUI(rect);

            if (tasks.Count == 0)
            {
                Rect helpRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 24);
                EditorGUI.HelpBox(helpRect, "No FMOD references need updating in the current scene.", MessageType.Info);
            }

            GUILayout.FlexibleSpace();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Cancel", GUILayout.Height(buttonHeight)))
                {
                    Close();
                }

                if (GUILayout.Button("Scan", GUILayout.Height(buttonHeight)))
                {
                    RefreshTasks();
                }

                using (new EditorGUI.DisabledScope(tasks.Count == 0))
                {
                    if (GUILayout.Button($"Execute {tasks.Count} Tasks", GUILayout.Height(buttonHeight)))
                    {
                        ExecuteAllTasks();
                    }
                }
            }
        }
        
        private void ExecuteAllTasks()
        {
            Undo.RecordObjects(tasks.Select(t => t.Target).ToArray(), "Update FMOD References");

            foreach (var task in tasks)
            {
                task.Execute();
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            RefreshTasks();
        }

        private class FMODComponentTask
        {
            public GameObject GameObject;
            public Object Target;
            public List<(FieldInfo oldField, string oldValue, FieldInfo newField, EventReference reference)> Fields = new();
            public bool Executed;

            public FMODComponentTask(GameObject go, Object target)
            {
                GameObject = go;
                Target = target;
            }

            public void Execute()
            {
                foreach (var (oldField, oldValue, newField, reference) in Fields)
                {
                    if (oldField != null && newField != null)
                    {
                        var newRef = new EventReference { Path = oldValue };
                        var editorEvent = EventManager.EventFromPath(oldValue);
                        if (editorEvent != null)
                            newRef.Guid = editorEvent.Guid;

                        newField.SetValue(Target, newRef);
                        oldField.SetValue(Target, string.Empty);
                    }
                    else if (newField != null)
                    {
                        var editorEvent = EventManager.EventFromPath(reference.Path) ?? EventManager.EventFromGUID(reference.Guid);
                        if (editorEvent != null)
                        {
                            var newRef = new EventReference
                            {
                                Path = editorEvent.Path,
                                Guid = editorEvent.Guid
                            };
                            newField.SetValue(Target, newRef);
                        }
                    }
                }

                EditorUtility.SetDirty(Target);
                Executed = true;
            }

            public string GetDescription()
            {
                var fieldNames = Fields.Select(f => f.oldField?.Name ?? f.newField?.Name).Where(n => !string.IsNullOrEmpty(n));
                return $"Update: {string.Join(", ", fieldNames.Distinct())}";
            }

            public string GetPath()
            {
                Transform t = GameObject.transform;
                string path = t.name;
                while (t.parent != null)
                {
                    t = t.parent;
                    path = t.name + "/" + path;
                }
                return path;
            }
        }

        private class FMODTaskView : TreeView
        {
            private List<FMODComponentTask> tasks;

            public FMODTaskView(TreeViewState state, List<FMODComponentTask> tasks)
                : base(state, new MultiColumnHeader(CreateHeaderState()))
            {
                this.tasks = tasks;
                showAlternatingRowBackgrounds = true;
                showBorder = true;
                Reload();
            }

            private static MultiColumnHeaderState CreateHeaderState()
            {
                return new MultiColumnHeaderState(new[]
                {
                    new MultiColumnHeaderState.Column { headerContent = new GUIContent("Target"), width = 300, minWidth = 100 },
                    new MultiColumnHeaderState.Column { headerContent = new GUIContent("Task"), width = 300, minWidth = 100 },
                    new MultiColumnHeaderState.Column { headerContent = new GUIContent("Status"), width = 100, minWidth = 60 }
                });
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
                root.children = new List<TreeViewItem>();

                int id = 1;

                foreach (var task in tasks)
                {
                    var item = new TreeViewItem
                    {
                        id = id++,
                        depth = 0,
                        displayName = task.GetPath()
                    };
                    root.AddChild(item);
                }

                SetupDepthsFromParentsAndChildren(root);
                return root;
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                if (tasks.Count == 0)
                {
                    var rect = args.GetCellRect(0);
                    EditorGUI.LabelField(rect, "<No tasks>");
                    return;
                }

                if (args.item.id - 1 < 0 || args.item.id - 1 >= tasks.Count) return;
                var task = tasks[args.item.id - 1];

                for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                {
                    var rect = args.GetCellRect(i);
                    switch (i)
                    {
                        case 0:
                            EditorGUI.LabelField(rect, task.GetPath());
                            break;
                        case 1:
                            EditorGUI.LabelField(rect, task.GetDescription());
                            break;
                        case 2:
                            EditorGUI.LabelField(rect, task.Executed ? "✔ Done" : "Pending");
                            break;
                    }
                }
            }

            protected override void DoubleClickedItem(int id)
            {
                if (id - 1 < 0 || id - 1 >= tasks.Count) return;
                var task = tasks[id - 1];
                EditorGUIUtility.PingObject(task.GameObject);
                Selection.activeGameObject = task.GameObject;
            }
        }
    }
}
#endif