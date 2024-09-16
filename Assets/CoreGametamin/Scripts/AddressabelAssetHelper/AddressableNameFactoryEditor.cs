#if UNITY_EDITOR
using UnityEditor;
namespace Gametamin.Core
{
    [CustomEditor(typeof(AddressableNameFactory), true)]
    public class AddressableNameFactoryEditor : ReferenceNameFactoryEditor<AddressableNameFactory>
    {
        protected override ReferenceNameFactory<AddressableNameFactory> InspectedObject => AddressableNameFactory.Instance;
        protected override void OnMiddleGUI()
        {
            base.OnMiddleGUI();
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.GUIButton("Load From System", () =>
                {
                    var labels = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.GetLabels().ToArray();
                    labels.For(item =>
                    {
                        CheckAndAddName(item);
                    });
                });
                EditorGUIHelper.GUIButton("Add To System", () =>
                {
                    AddressableNameFactory.Instance.AddToSystem();
                });
            });
        }
    }
}
#endif
