#if UNITY_EDITOR
using System.Collections.Generic;

namespace Gametamin.Core
{
    public partial class PopupData
    {
        public void SetPopupData(PopupCopyData copyData, string popupName, string folder, PopupPartCopyType popupCopyType)
        {
            if (popupCopyType != PopupPartCopyType.Non)
            {
                _popup = popupCopyType.GetCopyPopupPart(copyData.Popup, folder, $"{popupName}_popup");
            }
            SetPopupPart(copyData.PopupParts, popupName, folder);
            SetPoolPart(copyData.PoolDatas, popupName, folder);
            this.MakeObjectDirty(true);
        }
        void SetPopupPart(PopupPartCopyData[] parts, string popupName, string folder)
        {
            List<PopupPartReferenceData> list = new();
            parts.For((originPart, index) =>
            {
                if (originPart.CopyType != PopupPartCopyType.Non)
                {
                    list.Add(CreatePart(originPart, popupName, folder));
                }
            });
            _popupPartDatas = list.ToArray();
        }
        PopupPartReferenceData CreatePart(PopupPartCopyData originPart, string popupName, string folder)
        {
            var newPart = new PopupPartReferenceData();
            var partId = originPart.Id;
            if (partId.IsNullOrEmptySafe())
            {
                partId = originPart.Target.name;
            }
            var target = originPart.CopyType.GetCopyPopupPart(originPart.Target, folder, $"{popupName}_{partId.ToLowerSafe()}");
            newPart.SetData(originPart.ParentID, originPart.Id, target);
            return newPart;
        }
        void SetPoolPart(PopupPartPoolCopyData[] parts, string popupName, string folder)
        {
            List<PopupPartPoolReferenceData> list = new();
            parts.For((originPart, index) =>
            {
                if (originPart.CopyType != PopupPartCopyType.Non)
                {
                    list.Add(CreatePool(originPart, popupName, folder));
                }
            });
            _pools = list.ToArray();
        }
        PopupPartPoolReferenceData CreatePool(PopupPartPoolCopyData originPart, string popupName, string folder)
        {
            var newPart = new PopupPartPoolReferenceData();
            var target = originPart.CopyType.GetCopyPopupPart(originPart.Target, folder, $"{popupName}_{originPart.Id.ToLowerSafe()}");
            newPart.SetData(originPart.Id, target);
            return newPart;
        }
        public void AddParts(List<PopupPartCopyData> parts)
        {
            var folder = this.GetAssetDirectoryPath();
            var popupName = name.Split('_').GetSafe(0);
            List<PopupPartReferenceData> list = new();
            list.SafeAddRange(_popupPartDatas);
            parts.For((originPart, index) =>
            {
                if (originPart.CopyType != PopupPartCopyType.Non)
                {
                    list.Add(CreatePart(originPart, popupName, folder));
                }
            });
            _popupPartDatas = list.ToArray();
            this.MakeObjectDirty(true);
        }
        public void AddPools(List<PopupPartPoolCopyData> parts)
        {
            var folder = this.GetAssetDirectoryPath();
            var popupName = name.Split('_').GetSafe(0);
            List<PopupPartPoolReferenceData> list = new();
            list.SafeAddRange(_pools);
            parts.For((originPart, index) =>
            {
                if (originPart.CopyType != PopupPartCopyType.Non)
                {
                    list.Add(CreatePool(originPart, popupName, folder));
                }
            });
            _pools = list.ToArray();
            this.MakeObjectDirty(true);
        }
    }
}
#endif
