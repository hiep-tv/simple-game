using System;
using UnityEngine;

namespace Gametamin.Core
{
    public interface ICommonUIButton : ICommonButton
    {
        Action OnClickListener { get; set; }
        void OnAddPopup(GameObject popup);
    }
}
