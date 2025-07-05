using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class TMProInstructionOverlay : MonoBehaviour
    {

        public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        [FormerlySerializedAs("AnchorPosition")] public FpsCounterAnchorPositions anchorPosition = FpsCounterAnchorPositions.BottomLeft;

        private const string Instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

        private TextMeshPro _mTextMeshPro;
        private TextContainer _mTextContainer;
        private Transform _mFrameCounterTransform;
        private Camera _mCamera;

        //private FpsCounterAnchorPositions last_AnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            _mCamera = Camera.main;

            GameObject frameCounter = new GameObject("Frame Counter");
            _mFrameCounterTransform = frameCounter.transform;
            _mFrameCounterTransform.parent = _mCamera.transform;
            _mFrameCounterTransform.localRotation = Quaternion.identity;


            _mTextMeshPro = frameCounter.AddComponent<TextMeshPro>();
            _mTextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            _mTextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            _mTextMeshPro.fontSize = 30;

            _mTextMeshPro.isOverlay = true;
            _mTextContainer = frameCounter.GetComponent<TextContainer>();

            Set_FrameCounter_Position(anchorPosition);
            //last_AnchorPosition = AnchorPosition;

            _mTextMeshPro.text = Instructions;

        }




        void Set_FrameCounter_Position(FpsCounterAnchorPositions anchorPosition)
        {

            switch (anchorPosition)
            {
                case FpsCounterAnchorPositions.TopLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.TopLeft;
                    _mTextContainer.anchorPosition = TextContainerAnchors.TopLeft;
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(0, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomLeft;
                    _mTextContainer.anchorPosition = TextContainerAnchors.BottomLeft;
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(0, 0, 100.0f));
                    break;
                case FpsCounterAnchorPositions.TopRight:
                    //m_TextMeshPro.anchor = AnchorPositions.TopRight;
                    _mTextContainer.anchorPosition = TextContainerAnchors.TopRight;
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(1, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomRight:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomRight;
                    _mTextContainer.anchorPosition = TextContainerAnchors.BottomRight;
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(1, 0, 100.0f));
                    break;
            }
        }
    }
}
