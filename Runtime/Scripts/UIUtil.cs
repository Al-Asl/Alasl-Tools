using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlaslTools
{
    public static class UIUtil
    {
        public static RectOffset Offset(int value) => new RectOffset(value, value, value, value);

        public static void Expand(this RectTransform rect, int offset) => Expand(rect, Offset(offset));

        public static void Expand(this RectTransform rect, RectOffset offset)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(offset.left, offset.bottom);
            rect.offsetMax = new Vector2(-offset.right, -offset.top);
        }

        public static void ExpandVertical(this RectTransform rect, float offset) => ExpandVertical(rect, offset, offset);

        public static void ExpandVertical(this RectTransform rect, float bottom, float top)
        {
            rect.anchorMin = rect.anchorMin.SetY(0);
            rect.anchorMax = rect.anchorMax.SetY(1f);
            rect.offsetMin = rect.offsetMin.SetY(bottom);
            rect.offsetMax = rect.offsetMax.SetY(-top);
        }

        public static void ExpandHorizontal(this RectTransform rect, float offset) => ExpandHorizontal(rect, offset, offset);

        public static void ExpandHorizontal(this RectTransform rect, float left, float right)
        {
            rect.anchorMin = rect.anchorMin.SetX(0);
            rect.anchorMax = rect.anchorMax.SetX(1f);
            rect.offsetMin = rect.offsetMin.SetX(left);
            rect.offsetMax = rect.offsetMax.SetX(-right);
        }

        public static void SetPivot(this RectTransform rect, Direction2D direction)
        {
            if (((uint)direction & (uint)VerticalDirection.Bottom) > 0)
            {
                rect.pivot = rect.pivot.SetY(0);
            }
            else if (((uint)direction & (uint)VerticalDirection.Middle) > 0)
            {
                rect.pivot = rect.pivot.SetY(0.5f);
            }
            else if (((uint)direction & (uint)VerticalDirection.Top) > 0)
            {
                rect.pivot = rect.pivot.SetY(1f);
            }

            if (((uint)direction & (uint)HorizontalDirection.Left) > 0)
            {
                rect.pivot = rect.pivot.SetX(0);
            }
            else if (((uint)direction & (uint)HorizontalDirection.Center) > 0)
            {
                rect.pivot = rect.pivot.SetX(0.5f);
            }
            else if (((uint)direction & (uint)HorizontalDirection.Right) > 0)
            {
                rect.pivot = rect.pivot.SetX(1f);
            }
        }

        public static void SnapAnchors(this RectTransform rect, Direction2D direction, float offset = 0f)
        {
            if (((uint)direction & (uint)VerticalDirection.Bottom) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetY(0);
                rect.anchorMin = rect.anchorMin.SetY(0);
                rect.offsetMin = rect.offsetMin.SetY(offset);
            }
            else if (((uint)direction & (uint)VerticalDirection.Middle) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetY(0.5f);
                rect.anchorMin = rect.anchorMin.SetY(0.5f);
            }
            else if (((uint)direction & (uint)VerticalDirection.Top) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetY(1f);
                rect.anchorMin = rect.anchorMin.SetY(1f);
                rect.offsetMax = rect.offsetMax.SetY(-offset);
            }

            if (((uint)direction & (uint)HorizontalDirection.Left) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetX(0);
                rect.anchorMin = rect.anchorMin.SetX(0);
                rect.offsetMin = rect.offsetMin.SetX(offset);
            }
            else if (((uint)direction & (uint)HorizontalDirection.Center) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetX(0.5f);
                rect.anchorMin = rect.anchorMin.SetX(0.5f);
            }
            else if (((uint)direction & (uint)HorizontalDirection.Right) > 0)
            {
                rect.anchorMax = rect.anchorMax.SetX(1f);
                rect.anchorMin = rect.anchorMin.SetX(1f);
                rect.offsetMin = rect.offsetMin.SetX(-offset);
            }
        }

        public static void PreserveAspect(LayoutElement le, Direction2D direction, 
            float aspect, bool HeightControl = true, float offset = 0f)
        {
            var go = le.gameObject;

            var p = new GameObject(le.gameObject.name);
            var prect = p.AddComponent<RectTransform>();
            var ple = p.AddComponent<LayoutElement>();

            ple.preferredHeight = le.preferredHeight;
            ple.preferredWidth = le.preferredWidth;
            ple.minWidth = le.minWidth;
            ple.minHeight = le.minHeight;
            ple.flexibleHeight = le.flexibleHeight;
            ple.flexibleWidth = le.flexibleWidth;
            ple.ignoreLayout = le.ignoreLayout;
            ple.layoutPriority = le.layoutPriority;

            Object.Destroy(le);

            prect.SetParent(go.transform.parent);
            go.transform.SetParent(prect);

            var aspectFitter = go.AddComponent<AspectRatioFitter>();
            if (HeightControl)
                aspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            else
                aspectFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            aspectFitter.aspectRatio = aspect;

            var rect = go.GetComponent<RectTransform>();
            rect.SetPivot(direction);
            rect.SnapAnchors(direction, offset);
            rect.ExpandVertical(offset);
        }
    }
}
