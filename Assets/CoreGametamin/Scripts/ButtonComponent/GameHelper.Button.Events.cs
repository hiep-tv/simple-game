#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class ButtonHelper
    {
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonEventReference(this GameObject root, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            return root.GetComponentSafe<GameObjectReference>().SetButtonEventReference(onClick, referenceID);
        }
        /// <summary>
        /// set itself event and child button event
        /// return GameObjectReference of child button
        /// </summary>
        public static GameObjectReference SetButtonAndChildEventReference(this GameObject root, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            root.SetButtonEventSafe(onClick);
            return root.SetButtonEventReference(onClick, referenceID);
        }
        /// <summary>
        /// set itself event and child button event
        /// return GameObjectReference of child button
        /// </summary>
        public static GameObjectReference SetButtonAndChildEventReference(this GameObjectReference root, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            root.SetButtonEventSafe(onClick);
            return root.SetButtonEventReference(onClick, referenceID);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonEventReference(this GameObjectReference irootReference, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            return irootReference.GetComponentReference<ICommonButton>(referenceID).SetButtonEventSafe(onClick);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonEventSafe(this GameObject buttonObject, Action onClick = null)
        {
            return buttonObject.GetComponentSafe<ICommonButton>().SetButtonEventSafe(onClick);
        }
        /// <summary>
        /// return GameObjectReference of button, not irootReference
        /// </summary>
        public static GameObjectReference SetButtonEventSafe(this GameObjectReference ibuttonRef, Action onClick = null)
        {
            return ibuttonRef.GetComponentSafe<ICommonButton>().SetButtonEventSafe(onClick);
        }
        /// <summary>
        /// return GameObjectReference of button, not irootReference
        /// </summary>
        public static GameObjectReference SetButtonPreClickEvent(this GameObject buttonObject, Action onClick = null)
        {
            return buttonObject.GetComponentSafe<ICommonButton>().SetButtonPreClickEventSafe(onClick);
        }
        public static GameObjectReference SetButtonEventSafe(this ICommonButton ibutton, Action onClick = null)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.OnAddListener(onClick);
            }
            return ibutton.GetComponentSafe<GameObjectReference>();
        }
        public static GameObjectReference SetButtonPreClickEventSafe(this ICommonButton ibutton, Action onClick = null)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.OnAddPreListener(onClick);
            }
            return ibutton.GetComponentSafe<GameObjectReference>();
        }
        /// <summary>
        /// return GameObjectReference of button, not irootReference
        /// </summary>
        public static GameObjectReference SetButtonLastClickEvent(this GameObject buttonObject, Action onClick = null)
        {
            return buttonObject.GetComponentSafe<ICommonButton>().SetButtonLastClickEventSafe(onClick);
        }

        public static GameObjectReference SetButtonLastClickEventSafe(this ICommonButton ibutton, Action onClick = null)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.OnAddLastListener(onClick);
            }
            return ibutton.GetComponentSafe<GameObjectReference>();
        }
    }
}
