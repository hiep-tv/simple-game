#if UNITY_EDITOR
using UnityEngine;

namespace Gametamin.Core
{
    public partial class AtlasConfig
    {
        [SerializeField][HideInInspector] Object _textureFolder;
        [SerializeField][AtlasAddressableLabel] string _atlasName;
        public string AtlasName
        {
            get
            {
                if (!Atlas.IsNullSafe())
                {
                    var addresses = Atlas.GetAddressableAssetLabels();
                    if (addresses != null && !addresses.Contains(_atlasName))
                    {
                        Debug.LogError($"Atlas name on '{name}' is incorrect!");
                    }
                }
                return _atlasName;
            }
            set
            {
                _atlasName = value;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        public void SetTextureFolder(Object folder)
        {
            if (!folder.IsNullSafe())
            {
                _textureFolder = folder;
                this.MakeObjectDirty();
            }
        }
        public string GetTextureID(string textureName)
        {
            string result = string.Empty;
            TextureNames.ForBreakable(data =>
            {
                var exist = data.TextureName.Equals(textureName);
                if (exist)
                {
                    result = data.Id;
                }
                return exist;
            });
            return result;
        }
        public void AddTextureID(string id, string textureName)
        {
            var exist = false;
            var indexExist = -1;
            TextureNames.ForBreakable((data, index) =>
            {
                exist = data.TextureName.Equals(textureName);
                //Debug.Log($"data.TextureName={data.TextureName}, textureName={textureName}, id={id}, exist={exist}");
                if (exist)
                {
                    data.Id = id;
                    indexExist = index;
                }
                return exist;
            });
            //remove others same id
            TextureNames.ForReverse((data, index) =>
            {
                if (id == data.Id && index != indexExist)
                {
                    TextureNames.RemoveAt(index);
                }
            });
            if (!exist)
            {
                //Debug.Log($"Addnew: id={id}, textureName={textureName}");
                TextureNames.Add(new TextureNameReferenceData(id, textureName));
            }
            this.MakeObjectDirty();
        }
    }
}
#endif