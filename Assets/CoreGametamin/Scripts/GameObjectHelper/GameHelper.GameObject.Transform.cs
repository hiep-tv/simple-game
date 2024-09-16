#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        public static void SetParentSafe(this Component target, Transform parent)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetParentSafe(parent);
            }
        }
        public static void SetParentSafe(this GameObject target, Transform parent)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetParentSafe(parent);
            }
        }
        public static void SetParentSafe(this Transform target, Transform parent)
        {
            if (!target.IsNullSafe())
            {
                target.SetParent(parent);
            }
        }
        public static void SetScaleSafe(this Component target, float scale)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetScaleSafe(new Vector3(scale, scale, scale));
            }
        }
        public static void SetScaleSafe(this Component target, Vector3 scale)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetScaleSafe(scale);
            }
        }
        public static void SetScaleSafe(this Transform target, Vector3 scale)
        {
            if (!target.IsNullSafe())
            {
                var iscale = target.GetComponentSafe<ISetScale>();
                if (iscale.IsNullObjectSafe())
                {
                    target.localScale = scale;
                }
                else
                {
                    iscale.OnSetScale(scale);
                }
            }
        }
        public static void SetPositionSafe(this Component target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionSafe(position);
            }
        }
        public static void SetPositionSafe(this GameObject target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionSafe(position);
            }
        }
        public static void SetPositionSafe(this Transform target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.position = position;
            }
        }
        public static void SetPositionYSafe(this Component target, float y)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionYSafe(y);
            }
        }
        public static void SetPositionYSafe(this GameObject target, float y)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionYSafe(y);
            }
        }
        public static void SetPositionYSafe(this Transform target, float y)
        {
            if (!target.IsNullSafe())
            {
                var current = target.position;
                current.y = y;
                target.position = current;
            }
        }
        public static void SetPositionXSafe(this Component target, float x)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionYSafe(x);
            }
        }
        public static void SetPositionXSafe(this GameObject target, float x)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetPositionYSafe(x);
            }
        }
        public static void SetPositionXSafe(this Transform target, float x)
        {
            if (!target.IsNullSafe())
            {
                var current = target.position;
                current.x = x;
                target.position = current;
            }
        }
        public static Vector3 GetPositionSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionSafe();
            }
            return Vector3.one;
        }
        public static Vector3 GetPositionSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionSafe();
            }
            return Vector3.one;
        }
        public static Vector3 GetPositionSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.position;
            }
            return Vector3.one;
        }
        public static float GetPositionXSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionXSafe();
            }
            return 0f;
        }
        public static float GetPositionXSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionXSafe();
            }
            return 0f;
        }
        public static float GetPositionXSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.position.x;
            }
            return 0f;
        }
        public static float GetPositionYSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionYSafe();
            }
            return 0f;
        }
        public static float GetPositionYSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetPositionYSafe();
            }
            return 0f;
        }
        public static float GetPositionYSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.position.y;
            }
            return 0f;
        }
        public static void SetLocalPositionSafe(this Component target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionSafe(position);
            }
        }
        public static void SetLocalPositionSafe(this GameObject target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionSafe(position);
            }
        }
        public static void SetLocalPositionSafe(this Transform target, Vector3 position)
        {
            if (!target.IsNullSafe())
            {
                target.localPosition = position;
            }
        }
        public static void SetLocalPositionXSafe(this Component target, float x)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionXSafe(x);
            }
        }
        public static void SetLocalPositionXSafe(this GameObject target, float x)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionXSafe(x);
            }
        }
        public static void SetLocalPositionXSafe(this Transform target, float x)
        {
            if (!target.IsNullSafe())
            {
                var position = target.localPosition;
                position.x = x;
                target.localPosition = position;
            }
        }
        public static void SetLocalPositionYSafe(this Component target, float y)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionYSafe(y);
            }
        }
        public static void SetLocalPositionYSafe(this GameObject target, float y)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetLocalPositionYSafe(y);
            }
        }
        public static void SetLocalPositionYSafe(this Transform target, float y)
        {
            if (!target.IsNullSafe())
            {
                var position = target.localPosition;
                position.y = y;
                target.localPosition = position;
            }
        }
        public static Vector3 GetLocalPositionSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionSafe();
            }
            return Vector3.one;
        }
        public static Vector3 GetLocalPositionSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionSafe();
            }
            return Vector3.one;
        }
        public static Vector3 GetLocalPositionSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.localPosition;
            }
            return Vector3.one;
        }
        public static float GetLocalPositionXSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionXSafe();
            }
            return 0f;
        }
        public static float GetLocalPositionXSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionXSafe();
            }
            return 0f;
        }
        public static float GetLocalPositionXSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.localPosition.x;
            }
            return 0f;
        }
        public static float GetLocalPositionYSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionYSafe();
            }
            return 0f;
        }
        public static float GetLocalPositionYSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                return target.transform.GetLocalPositionYSafe();
            }
            return 0f;
        }
        public static float GetLocalPositionYSafe(this Transform target)
        {
            if (!target.IsNullSafe())
            {
                return target.localPosition.y;
            }
            return 0f;
        }
        public static void SetAsFirstSiblingSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetAsFirstSiblingSafe();
            }
        }
        public static void SetAsFirstSiblingSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetAsFirstSiblingSafe();
            }
        }
        public static void SetAsFirstSiblingSafe(this Transform transform)
        {
            if (!transform.IsNullSafe())
            {
                transform.SetAsFirstSibling();
            }
        }
        public static void SetAsLastSiblingSafe(this Component target)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetAsLastSiblingSafe();
            }
        }
        public static void SetAsLastSiblingSafe(this GameObject target)
        {
            if (!target.IsNullSafe())
            {
                target.transform.SetAsLastSiblingSafe();
            }
        }
        public static void SetAsLastSiblingSafe(this Transform transform)
        {
            if (!transform.IsNullSafe())
            {
                transform.SetAsLastSibling();
            }
        }
    }
    public interface ISetScale
    {
        void OnSetScale(Vector3 scale);
    }
}