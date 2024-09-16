#if DEBUG_MODE
#define POPUP_TRACKING
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
#if POPUP_TRACKING
        static Dictionary<PoolReferenceID, PoolReferenceID> _popupShowHideTracking = new();
#endif
        [Conditional("DEBUG_MODE")]
        static void AddTracking(this GameObjectReference @ref, PoolReferenceID id)
        {
#if POPUP_TRACKING
            if (_popupShowHideTracking.ContainsKey(id))
            {
                Throw($"Do not call PlayShowPopupAnimation() several times in a row! (Popup ID={id}, {@ref.gameObject.name})");
            }
            else if (id == PoolReferenceID.Non)
            {
                Throw($"Popup ID is incorrect! (Popup ID={id}, {@ref.gameObject.name})");
            }
            else
            {
                _popupShowHideTracking.Add(id, id);
            }
#endif
        }
        [Conditional("DEBUG_MODE")]
        static void RemoveTracking(PoolReferenceID id)
        {
#if POPUP_TRACKING
            if (_popupShowHideTracking.ContainsKey(id))
            {
                _popupShowHideTracking.Remove(id);
            }
            else
            {
                Throw($"Must call PlayShowPopupAnimation() first! (popup ID={id})");
            }
#endif
        }
        [Conditional("DEBUG_MODE")]
        public static void Tracking()
        {

        }
        [Conditional("DEBUG_MODE")]
        static void Throw(object message)
        {
#if POPUP_TRACKING
            UnityEngine.Debug.LogError(message);
            message.ToString().ShowNoticeMessage();
            //throw new Exception($"{message}");
#endif
        }
        [Conditional("DEBUG_MODE")]
        static void ClearTracking()
        {
#if POPUP_TRACKING
            _popupShowHideTracking.Clear();
#endif
        }
#if POPUP_TRACKING
        public static bool ShowDebugPopupStack
        {
            get => LocalPrefs.GetBool("_debug_popup_stack");
            set => LocalPrefs.SetBool("_debug_popup_stack", value);
        }
        static GUIStyle _debugPopupStackStyle;
        static GUIStyle _DebugPopupStackStyle
        {
            get
            {
                if (_debugPopupStackStyle == null)
                {
                    _debugPopupStackStyle = GUIHelper.CreateLabelStyle(Color.red, 40);
                    _debugPopupStackStyle.fontStyle = FontStyle.Bold;
                }
                return _debugPopupStackStyle;
            }
        }
#endif
        [Conditional("DEBUG_MODE")]
        public static void OnGUIPopupStack()
        {
#if POPUP_TRACKING
            if (ShowDebugPopupStack)
            {
                GUILayout.BeginArea(GUIHelper.ScreenRect);
                {
                    GUILayout.BeginVertical();
                    {
                        bool top = true;
                        if (top)
                        {
                            GUILayout.Space(10);
                        }
                        else
                        {
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.FlexibleSpace();
                            if (_popupStacks.Count > 0)
                            {
                                foreach (var item in _popupStacks)
                                {
                                    GUILayout.Label($"{item}", _DebugPopupStackStyle);
                                }
                            }
                            else
                            {
                                GUILayout.Label("[No Popup]", _DebugPopupStackStyle);
                            }
                            GUILayout.Space(10);
                        }
                        GUILayout.EndHorizontal();
                        if (top)
                        {
                            GUILayout.FlexibleSpace();
                        }
                        else
                        {
                            GUILayout.Space(20);
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndArea();
            }
#endif
        }
    }
}
