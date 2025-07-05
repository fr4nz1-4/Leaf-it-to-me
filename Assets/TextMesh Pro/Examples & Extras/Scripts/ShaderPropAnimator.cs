using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    
    public class ShaderPropAnimator : MonoBehaviour
    {

        private Renderer _mRenderer;
        private Material _mMaterial;

        [FormerlySerializedAs("GlowCurve")] public AnimationCurve glowCurve;

        [FormerlySerializedAs("m_frame")] public float mFrame;

        void Awake()
        {
            // Cache a reference to object's renderer
            _mRenderer = GetComponent<Renderer>();

            // Cache a reference to object's material and create an instance by doing so.
            _mMaterial = _mRenderer.material;
        }

        void Start()
        {
            StartCoroutine(AnimateProperties());
        }

        IEnumerator AnimateProperties()
        {
            //float lightAngle;
            float glowPower;
            mFrame = Random.Range(0f, 1f);

            while (true)
            {
                //lightAngle = (m_Material.GetFloat(ShaderPropertyIDs.ID_LightAngle) + Time.deltaTime) % 6.2831853f;
                //m_Material.SetFloat(ShaderPropertyIDs.ID_LightAngle, lightAngle);

                glowPower = glowCurve.Evaluate(mFrame);
                _mMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glowPower);

                mFrame += Time.deltaTime * Random.Range(0.2f, 0.3f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
