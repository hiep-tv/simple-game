using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static class MyEditor
    {
        [MenuItem("Gametamin/DoSomething")]
        static void DoSomething()
        {
            Debug.Log(Application.dataPath);
            Debug.Log(Path.GetDirectoryName(Application.dataPath));
            //Path.Combine(Path.GetDirectoryName(Application.dataPath), Path.);
        }
        [MenuItem("GameObject/RemoveScripts")]
        static void RemoveScripts()
        {
            var target = Selection.activeGameObject;
            if (!target.IsNullSafe())
            {
                var components = target.GetComponentsInChildren(typeof(Component));
                components.For(component =>
                {
                    var type = component.GetType();
                    if (type != typeof(RectTransform)
                    && type != typeof(Image)
                    && type != typeof(Animator)
                    && type != typeof(CanvasGroup)
                    && type != typeof(TextMeshProUGUI)
                    && type != typeof(CanvasRenderer)
                    )
                    {
                        Object.DestroyImmediate(component);
                    }
                });

                //components = target.GetComponentsInChildren(typeof(Component));
                //components.For(component =>
                //{
                //    var type = component.GetType();
                //    if (type != typeof(RectTransform)
                //    && type != typeof(Image)
                //    && type != typeof(Animator)
                //    && type != typeof(CanvasGroup)
                //    && type != typeof(TextMeshProUGUI)
                //    && type != typeof(CanvasRenderer)
                //    )
                //    {
                //        Object.DestroyImmediate(component);
                //    }
                //});
            }
        }
        [MenuItem("Gametamin/Process With GameObjects")]
        static void ProcessWithGameObject()
        {
            Debug.Log($"Start Update========>");
            //Debug.Log("TODO: do something here to edit prefabs!");

            string[] searchInFolders = { "Assets/" };
            List<GameObject> _results = new();
            searchInFolders.FindAssetDatabase<GameObject>("prefab", result =>
            {
                _results.Add(result);
            });
            var total = _results.GetCountSafe();
            Update();
            void Update(int index = 0)
            {
                var result = _results.GetSafe(index);
                var path = result.GetAssetPath();
                bool save = false;
                var components = result.GetComponentsInChildrenSafe<SpriteLoader>();
                var count = components.GetCountSafe();
                if (count > 0)
                {
                    using (var editingScope = new PrefabUtility.EditPrefabContentsScope(path))
                    {
                        //foreach (var component in components)
                        //{
                        //Object.DestroyImmediate(component, true);
                        //component.SetSprite(null);
                        //}
                        save = true;
                        Debug.Log($"Updated {result.name}");
                    }
                }
                index++;
                if (index <= total)
                {
                    if (save)
                    {
                        var delay = EditorApplication.timeSinceStartup + .1f;
                        EditorApplication.update += OnUpdate;
                        void OnUpdate()
                        {
                            if (EditorApplication.timeSinceStartup >= delay)
                            {
                                EditorApplication.update -= OnUpdate;
                                if (index >= total)
                                {
                                    AssetDatabase.SaveAssets();
                                    Debug.Log($"End Update========>");
                                }
                                else
                                {
                                    Update(index);
                                }
                            }
                        }
                    }
                    else
                    {
                        Update(index);
                    }
                }
                else
                {
                    Debug.Log($"End Update========>");
                }
            }
        }
    }
}
