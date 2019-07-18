using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kosu.UnityLibrary
{
    public static class GraphicExtensions
    {

        public static void SetActive(this Graphic g, bool active)
        {
            g.gameObject.SetActive(active);
        }

        public static void SetAlpha(this Graphic g, float a)
        {
            var col = g.color;
            col.a = a;
            g.color = col;
        }

        public static void SetAnchoredPositionX(this Graphic g, float x)
        {
            var pos = g.rectTransform.anchoredPosition;
            pos.x = x;
            g.rectTransform.anchoredPosition = pos;
        }

        public static void SetAnchoredPositionY(this Graphic g, float y)
        {
            var pos = g.rectTransform.anchoredPosition;
            pos.y = y;
            g.rectTransform.anchoredPosition = pos;
        }

    }
}
