using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{

    public class ObjectSpin : MonoBehaviour
    {
        #pragma warning disable 0414
        public enum MotionType { Rotation, SearchLight, Translation };
        [FormerlySerializedAs("Motion")] public MotionType motion;

        [FormerlySerializedAs("TranslationDistance")] public Vector3 translationDistance = new Vector3(5, 0, 0);
        [FormerlySerializedAs("TranslationSpeed")] public float translationSpeed = 1.0f;
        [FormerlySerializedAs("SpinSpeed")] public float spinSpeed = 5;
        [FormerlySerializedAs("RotationRange")] public int rotationRange = 15;
        private Transform _mTransform;

        private float _mTime;
        private Vector3 _mPrevPos;
        private Vector3 _mInitialRotation;
        private Vector3 _mInitialPosition;
        private Color32 _mLightColor;

        void Awake()
        {
            _mTransform = transform;
            _mInitialRotation = _mTransform.rotation.eulerAngles;
            _mInitialPosition = _mTransform.position;

            Light light = GetComponent<Light>();
            _mLightColor = light != null ? light.color : Color.black;
        }


        // Update is called once per frame
        void Update()
        {
            switch (motion)
            {
                case MotionType.Rotation:
                    _mTransform.Rotate(0, spinSpeed * Time.deltaTime, 0);
                    break;
                case MotionType.SearchLight:
                    _mTime += spinSpeed * Time.deltaTime;
                    _mTransform.rotation = Quaternion.Euler(_mInitialRotation.x, Mathf.Sin(_mTime) * rotationRange + _mInitialRotation.y, _mInitialRotation.z);
                    break;
                case MotionType.Translation:
                    _mTime += translationSpeed * Time.deltaTime;

                    float x = translationDistance.x * Mathf.Cos(_mTime);
                    float y = translationDistance.y * Mathf.Sin(_mTime) * Mathf.Cos(_mTime * 1f);
                    float z = translationDistance.z * Mathf.Sin(_mTime);

                    _mTransform.position = _mInitialPosition + new Vector3(x, z, y);

                    // Drawing light patterns because they can be cool looking.
                    //if (Time.frameCount > 1)
                    //    Debug.DrawLine(m_transform.position, m_prevPOS, m_lightColor, 100f);

                    _mPrevPos = _mTransform.position;
                    break;
            }
        }
    }
}