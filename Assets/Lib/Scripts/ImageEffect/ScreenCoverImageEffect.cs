using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    [ExecuteInEditMode]
    public class ScreenCoverImageEffect : MyImageEffectBase
    {
        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        [Range(0, 800)]
        private float _radius = 0;

        public enum ScreenCoverType
        {
            ADD_COVER,
            MUL_COVER,
            SCREEN_COVER,
            OVERLAY_COVER,
            CIRCLE,
        }

        [SerializeField]
        private ScreenCoverType _type;

        private int _colorId;

        private int _radiusId;

        private static readonly string COVER_COLOR = "_CoverColor";

        private static readonly string RADIUS = "_Radius";

        private void Awake()
        {
            _colorId = Shader.PropertyToID(COVER_COLOR);
            _radiusId = Shader.PropertyToID(RADIUS);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Mat.SetColor(_colorId, _color);

            switch (_type)
            {
                case ScreenCoverType.ADD_COVER:
                    ScreenCoverAdd(src, dest);
                    break;

                case ScreenCoverType.MUL_COVER:
                    ScreenCoverMul(src, dest);
                    break;

                case ScreenCoverType.SCREEN_COVER:
                    ScreenCoverScreen(src, dest);
                    break;

                case ScreenCoverType.OVERLAY_COVER:
                    ScreenCoverOverlay(src, dest);
                    break;

                case ScreenCoverType.CIRCLE:
                    CircleScreenCover(src, dest);
                    break;
            }
        }

        private void CircleScreenCover(RenderTexture src, RenderTexture dest)
        {
            Mat.SetFloat(_radiusId, _radius);
            Graphics.Blit(src, dest, Mat, 0);
        }

        private void ScreenCoverAdd(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, Mat, 1);
        }

        private void ScreenCoverMul(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, Mat, 2);
        }

        private void ScreenCoverScreen(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, Mat, 3);
        }

        private void ScreenCoverOverlay(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, Mat, 4);
        }

        public void ChangeScreenCoverType(ScreenCoverType type)
        {
            _type = type;
        }
    }
}
