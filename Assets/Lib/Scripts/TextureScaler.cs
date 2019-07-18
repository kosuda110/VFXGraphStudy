// Modified from: http://wiki.unity3d.com/index.php/TextureScale#TextureScale.cs
// Only works on ARGB32, RGB24 and Alpha8 textures that are marked readable

using UnityEngine;


namespace Kosu.UnityLibrary
{
    public static class TextureScaler
    {

        private static Color[] _texColors;
        private static Color[] _newColors;
        private static int _w;
        private static float _ratioX;
        private static float _ratioY;
        private static int _w2;

        public static Texture2D Scale(Texture2D tex, int newWidth, int newHeight, bool useBilinear = false)
        {
            var res = new Texture2D(newWidth, newHeight);
            _texColors = tex.GetPixels();
            _newColors = new Color[newWidth * newHeight];

            if (useBilinear)
            {
                _ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
                _ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
            }
            else
            {
                _ratioX = ((float)tex.width) / newWidth;
                _ratioY = ((float)tex.height) / newHeight;
            }

            _w = tex.width;
            _w2 = newWidth;

            if (useBilinear)
            {
                BilinearScale(0, newHeight);
            }
            else
            {
                PointScale(0, newHeight);
            }

            res.SetPixels(_newColors);
            res.Apply();
            return res;
        }

        private static void BilinearScale(int start, int end)
        {
            for (var y = start; y < end; y++)
            {
                var yFloor = Mathf.FloorToInt(y * _ratioY);
                var y1 = yFloor * _w;
                var y2 = (yFloor + 1) * _w;
                var yw = y * _w2;

                for (var x = 0; x < _w2; x++)
                {
                    var xFloor = Mathf.FloorToInt(x * _ratioX);
                    var xLerp = x * _ratioX - xFloor;
                    _newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(_texColors[y1 + xFloor], _texColors[y1 + xFloor + 1], xLerp),
                                                            ColorLerpUnclamped(_texColors[y2 + xFloor], _texColors[y2 + xFloor + 1], xLerp),
                                                            y * _ratioY - yFloor);
                }
            }
        }

        private static void PointScale(int start, int end)
        {
            for (var y = start; y < end; y++)
            {
                var thisY = (int)(_ratioY * y) * _w;
                var yw = y * _w2;

                for (var x = 0; x < _w2; x++)
                {
                    _newColors[yw + x] = _texColors[(int)(thisY + _ratioX * x)];
                }
            }
        }

        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value,
                             c1.g + (c2.g - c1.g) * value,
                             c1.b + (c2.b - c1.b) * value,
                             c1.a + (c2.a - c1.a) * value);
        }

    }
}
