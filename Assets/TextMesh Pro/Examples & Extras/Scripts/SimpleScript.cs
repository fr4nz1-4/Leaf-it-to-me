using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

    public class SimpleScript : MonoBehaviour
    {

        private TextMeshPro _mTextMeshPro;
        //private TMP_FontAsset m_FontAsset;

        private const string Label = "The <#0050FF>count is: </color>{0:2}";
        private float _mFrame;


        void Start()
        {
            // Add new TextMesh Pro Component
            _mTextMeshPro = gameObject.AddComponent<TextMeshPro>();

            _mTextMeshPro.autoSizeTextContainer = true;

            // Load the Font Asset to be used.
            //m_FontAsset = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            //m_textMeshPro.font = m_FontAsset;

            // Assign Material to TextMesh Pro Component
            //m_textMeshPro.fontSharedMaterial = Resources.Load("Fonts & Materials/LiberationSans SDF - Bevel", typeof(Material)) as Material;
            //m_textMeshPro.fontSharedMaterial.EnableKeyword("BEVEL_ON");

            // Set various font settings.
            _mTextMeshPro.fontSize = 48;

            _mTextMeshPro.alignment = TextAlignmentOptions.Center;

            //m_textMeshPro.anchorDampening = true; // Has been deprecated but under consideration for re-implementation.
            //m_textMeshPro.enableAutoSizing = true;

            //m_textMeshPro.characterSpacing = 0.2f;
            //m_textMeshPro.wordSpacing = 0.1f;

            //m_textMeshPro.enableCulling = true;
            _mTextMeshPro.textWrappingMode = TextWrappingModes.NoWrap;

            //textMeshPro.fontColor = new Color32(255, 255, 255, 255);
        }


        void Update()
        {
            _mTextMeshPro.SetText(Label, _mFrame % 1000);
            _mFrame += 1 * Time.deltaTime;
        }

    }
}
