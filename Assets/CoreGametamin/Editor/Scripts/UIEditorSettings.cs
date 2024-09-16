using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UIEditorSettings : BaseEditorWindow
    {
        Vector2 _scrollViewPosition;
        [MenuItem("Gametamin/UI Editor")]
        static void ShowWindow()
        {
            GetWindow<UIEditorSettings>();
        }
        static string GuidSaveFolder
        {
            get => EditorPrefs.GetString("_save_folder_popup_");
            set => EditorPrefs.SetString("_save_folder_popup_", value);
        }
        static string _SelectedTabKey = "_selectededitortab";
        int _selectedTab = 0;
        int _SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                EditorPrefs.SetInt(_SelectedTabKey, value);
            }
        }
        string[] tabs = { "Create Popup", "Popup Data", "Create Atlas", "Sprite Atlas" };
        private void OnEnable()
        {
            _selectedTab = EditorPrefs.GetInt(_SelectedTabKey, 0);
            LoadPopupData();
            LoadAtlasConfig();
            LoadCreateAtlasData();
        }
        protected override void OnCustomGUI()
        {
            base.OnCustomGUI();
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.HorizontalLayout(() =>
            {
                GUITabs();
                EditorGUIHelper.GUIButton("Reload", () =>
                {
                    LoadPopupData();
                    LoadAtlasConfig();
                });
                EditorGUIHelper.GUIButton("Force Resolve", () =>
                {
                    ForceResolve();
                });
            }, true);

            _scrollViewPosition = GUILayout.BeginScrollView(_scrollViewPosition);
            GUIContents();
            GUILayout.EndScrollView();
        }
        void GUIContents()
        {
            if (_SelectedTab == 0)
            {
                GUICreatePopupData();
            }
            else if (_SelectedTab == 1)
            {
                GUIPopupDatas();
            }
            else if (_SelectedTab == 2)
            {
                GUICreateAtlas();
            }
            else if (_SelectedTab == 3)
            {
                GUIAtlases();
            }
        }
        void GUITabs()
        {
            _SelectedTab = GUILayout.Toolbar(_SelectedTab, tabs, GUI.skin.GetStyle("Button"), GUI.ToolbarButtonSize.FitToContents);
        }
        void ForceResolve()
        {
            AddressableNameFactory.Instance.Generate();
            AddressableNameFactory.Instance.AddToSystem();
            PoolNameReference.Instance.Generate();
            PopupPoolFactory.Instance.Generate();
            PopupPoolFactory.Instance.MarkAddressable();
            GameObjectReferenceNameFactory.Instance.Generate();
            TextureNameReferenceFactory.Instance.Generate();
            TextNameReferenceFactory.Instance.Generate();
            SpriteAtlasFactory.Instance.MarkAddressable();
        }
    }
}