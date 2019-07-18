using UnityEngine;

namespace Kosu.UnityLibrary
{
    public static class RectTransformExtensions
    {
        //
        // AnchoredPosition
        //

        /// <summary>
        /// AnchoredPositionを設定する
        /// </summary>
        /// <param name="x">x</param>
        public static void SetAnchoredPosX(this RectTransform t, float x)
        {
            SetAnchoredPos(t, x, t.anchoredPosition.y);
        }

        /// <summary>
        /// AnchoredPositionを設定する
        /// </summary>
        /// <param name="y">y</param>
        public static void SetAnchoredPosY(this RectTransform t, float y)
        {
            SetAnchoredPos(t, t.anchoredPosition.x, y);
        }

        /// <summary>
        /// AnchoredPositionを設定する
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public static void SetAnchoredPos(this RectTransform t, float x, float y)
        {
            t.anchoredPosition = new Vector2(x, y);
        }

        //
        //Translate
        //

        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="x">x</param>
        public static void TranslateX(this RectTransform t, float x)
        {
            Translate(t, x, 0);
        }

        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="y">y</param>
        public static void TranslateY(this RectTransform t, float y)
        {
            Translate(t, 0, y);
        }

        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public static void Translate(this RectTransform t, float x, float y)
        {
            t.anchoredPosition = GetTranslatedPos(t, x, y);
        }

        /// <summary>
        /// 平行移動した場合の座標を取得する
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        ///<returns> 平行移動後の座標</returns>
        public static Vector2 GetTranslatedPos(this RectTransform t, float x, float y)
        {
            return new Vector2(t.anchoredPosition.x + x, t.anchoredPosition.y + y);
        }

        //
        //size
        //

        /// <summary>
        ///幅の変更
        /// </summary>
        /// <param name="x">x</param>
        public static void SetWidth(this RectTransform t, float x)
        {
            SetSize(t, x, t.sizeDelta.y);
        }

        /// <summary>
        ///高さの変更
        /// </summary>
        /// <param name="y">y</param>
        public static void SetHeight(this RectTransform t, float y)
        {
            SetSize(t, t.sizeDelta.x, y);
        }

        /// <summary>
        ///サイズの変更
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public static void SetSize(this RectTransform t, float x, float y)
        {
            t.sizeDelta = new Vector2(x, y);
        }

        //
        // scale
        //

        /// <summary>
        ///スケールの変更
        /// </summary>
        /// <param name="x">x</param>
        public static void SetScaleX(this RectTransform t, float x)
        {
            SetScale(t, x, t.localScale.y);
        }

        /// <summary>
        ///スケールの変更
        /// </summary>
        /// <param name="y">y</param>
        public static void SetScaleY(this RectTransform t, float y)
        {
            SetScale(t, t.localScale.x, y);
        }

        /// <summary>
        ///スケールの変更
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public static void SetScale(this RectTransform t, float x, float y)
        {
            t.localScale = new Vector3(x, y, 1);
        }

        //
        // Pivot
        //

        /// <summary>
        ///Pivotのxを設定する(0 ~ 1)
        /// </summary>
        /// <param name="x">pivotのx</param>
        public static void SetPivotX(this RectTransform t, float x)
        {
            SetPivot(t, x, t.pivot.y);
        }

        /// <summary>
        ///Pivotのyを設定する(0 ~ 1)
        /// </summary>
        /// <param name="y">pivotのy</param>
        public static void SetPivotY(this RectTransform t, float y)
        {
            SetPivot(t, t.pivot.x, y);
        }

        /// <summary>
        ///Pivotを設定する
        /// </summary>
        /// <param name="x">pivotのx</param>
        /// <param name="y">pivotのy</param>
        public static void SetPivot(this RectTransform t, float x, float y)
        {
            t.pivot = new Vector2(x, y);
        }

        public static void SetOffsetMinX(this RectTransform t, float x)
        {
            var offset = t.offsetMin;
            offset.x = x;
            t.offsetMin = offset;
        }

        public static void SetOffsetMinY(this RectTransform t, float y)
        {
            var offset = t.offsetMin;
            offset.y = y;
            t.offsetMin = offset;
        }

        public static void SetOffsetMaxX(this RectTransform t, float x)
        {
            var offset = t.offsetMax;
            offset.x = x;
            t.offsetMax = offset;
        }

        public static void SetOffsetMaxY(this RectTransform t, float y)
        {
            var offset = t.offsetMax;
            offset.y = y;
            t.offsetMax = offset;
        }

    }
}
