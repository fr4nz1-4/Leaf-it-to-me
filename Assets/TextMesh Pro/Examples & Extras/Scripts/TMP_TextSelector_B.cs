using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;


#pragma warning disable 0618 // Disabled warning due to SetVertices being deprecated until new release with SetMesh() is available.

namespace TMPro.Examples
{

    public class TMPTextSelectorB : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
    {
        [FormerlySerializedAs("TextPopup_Prefab_01")] public RectTransform textPopupPrefab01;

        private RectTransform _mTextPopupRectTransform;
        private TextMeshProUGUI _mTextPopupTMPComponent;
        private const string KLinkText = "You have selected link <#ffff00>";
        private const string KWordText = "Word Index: <#ffff00>";


        private TextMeshProUGUI _mTextMeshPro;
        private Canvas _mCanvas;
        private Camera _mCamera;

        // Flags
        private bool _isHoveringObject;
        private int _mSelectedWord = -1;
        private int _mSelectedLink = -1;
        private int _mLastIndex = -1;

        private Matrix4x4 _mMatrix;

        private TMP_MeshInfo[] _mCachedMeshInfoVertexData;

        void Awake()
        {
            _mTextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();


            _mCanvas = gameObject.GetComponentInParent<Canvas>();

            // Get a reference to the camera if Canvas Render Mode is not ScreenSpace Overlay.
            if (_mCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                _mCamera = null;
            else
                _mCamera = _mCanvas.worldCamera;

            // Create pop-up text object which is used to show the link information.
            _mTextPopupRectTransform = Instantiate(textPopupPrefab01) as RectTransform;
            _mTextPopupRectTransform.SetParent(_mCanvas.transform, false);
            _mTextPopupTMPComponent = _mTextPopupRectTransform.GetComponentInChildren<TextMeshProUGUI>();
            _mTextPopupRectTransform.gameObject.SetActive(false);
        }


        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            // UnSubscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }


        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == _mTextMeshPro)
            {
                // Update cached vertex data.
                _mCachedMeshInfoVertexData = _mTextMeshPro.textInfo.CopyMeshInfoVertexData();
            }
        }


        void LateUpdate()
        {
            if (_isHoveringObject)
            {
                // Check if Mouse Intersects any of the characters. If so, assign a random color.
                #region Handle Character Selection
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(_mTextMeshPro, Input.mousePosition, _mCamera, true);

                // Undo Swap and Vertex Attribute changes.
                if (charIndex == -1 || charIndex != _mLastIndex)
                {
                    RestoreCachedVertexAttributes(_mLastIndex);
                    _mLastIndex = -1;
                }

                if (charIndex != -1 && charIndex != _mLastIndex && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    _mLastIndex = charIndex;

                    // Get the index of the material / sub text object used by this character.
                    int materialIndex = _mTextMeshPro.textInfo.characterInfo[charIndex].materialReferenceIndex;

                    // Get the index of the first vertex of the selected character.
                    int vertexIndex = _mTextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                    // Get a reference to the vertices array.
                    Vector3[] vertices = _mTextMeshPro.textInfo.meshInfo[materialIndex].vertices;

                    // Determine the center point of the character.
                    Vector2 charMidBasline = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2;

                    // Need to translate all 4 vertices of the character to aligned with middle of character / baseline.
                    // This is needed so the matrix TRS is applied at the origin for each character.
                    Vector3 offset = charMidBasline;

                    // Translate the character to the middle baseline.
                    vertices[vertexIndex + 0] = vertices[vertexIndex + 0] - offset;
                    vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - offset;
                    vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - offset;
                    vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - offset;

                    float zoomFactor = 1.5f;

                    // Setup the Matrix for the scale change.
                    _mMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * zoomFactor);

                    // Apply Matrix operation on the given character.
                    vertices[vertexIndex + 0] = _mMatrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                    vertices[vertexIndex + 1] = _mMatrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = _mMatrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = _mMatrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                    // Translate the character back to its original position.
                    vertices[vertexIndex + 0] = vertices[vertexIndex + 0] + offset;
                    vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + offset;
                    vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + offset;
                    vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + offset;

                    // Change Vertex Colors of the highlighted character
                    Color32 c = new Color32(255, 255, 192, 255);

                    // Get a reference to the vertex color
                    Color32[] vertexColors = _mTextMeshPro.textInfo.meshInfo[materialIndex].colors32;

                    vertexColors[vertexIndex + 0] = c;
                    vertexColors[vertexIndex + 1] = c;
                    vertexColors[vertexIndex + 2] = c;
                    vertexColors[vertexIndex + 3] = c;


                    // Get a reference to the meshInfo of the selected character.
                    TMP_MeshInfo meshInfo = _mTextMeshPro.textInfo.meshInfo[materialIndex];

                    // Get the index of the last character's vertex attributes.
                    int lastVertexIndex = vertices.Length - 4;

                    // Swap the current character's vertex attributes with those of the last element in the vertex attribute arrays.
                    // We do this to make sure this character is rendered last and over other characters.
                    meshInfo.SwapVertexData(vertexIndex, lastVertexIndex);

                    // Need to update the appropriate
                    _mTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                }
                #endregion


                #region Word Selection Handling
                //Check if Mouse intersects any words and if so assign a random color to that word.
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(_mTextMeshPro, Input.mousePosition, _mCamera);

                // Clear previous word selection.
                if (_mTextPopupRectTransform != null && _mSelectedWord != -1 && (wordIndex == -1 || wordIndex != _mSelectedWord))
                {
                    TMP_WordInfo wInfo = _mTextMeshPro.textInfo.wordInfo[_mSelectedWord];

                    // Iterate through each of the characters of the word.
                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int characterIndex = wInfo.firstCharacterIndex + i;

                        // Get the index of the material / sub text object used by this character.
                        int meshIndex = _mTextMeshPro.textInfo.characterInfo[characterIndex].materialReferenceIndex;

                        // Get the index of the first vertex of this character.
                        int vertexIndex = _mTextMeshPro.textInfo.characterInfo[characterIndex].vertexIndex;

                        // Get a reference to the vertex color
                        Color32[] vertexColors = _mTextMeshPro.textInfo.meshInfo[meshIndex].colors32;

                        Color32 c = vertexColors[vertexIndex + 0].Tint(1.33333f);

                        vertexColors[vertexIndex + 0] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }

                    // Update Geometry
                    _mTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

                    _mSelectedWord = -1;
                }


                // Word Selection Handling
                if (wordIndex != -1 && wordIndex != _mSelectedWord && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    _mSelectedWord = wordIndex;

                    TMP_WordInfo wInfo = _mTextMeshPro.textInfo.wordInfo[wordIndex];

                    // Iterate through each of the characters of the word.
                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int characterIndex = wInfo.firstCharacterIndex + i;

                        // Get the index of the material / sub text object used by this character.
                        int meshIndex = _mTextMeshPro.textInfo.characterInfo[characterIndex].materialReferenceIndex;

                        int vertexIndex = _mTextMeshPro.textInfo.characterInfo[characterIndex].vertexIndex;

                        // Get a reference to the vertex color
                        Color32[] vertexColors = _mTextMeshPro.textInfo.meshInfo[meshIndex].colors32;

                        Color32 c = vertexColors[vertexIndex + 0].Tint(0.75f);

                        vertexColors[vertexIndex + 0] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }

                    // Update Geometry
                    _mTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

                }
                #endregion


                #region Example of Link Handling
                // Check if mouse intersects with any links.
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(_mTextMeshPro, Input.mousePosition, _mCamera);

                // Clear previous link selection if one existed.
                if ((linkIndex == -1 && _mSelectedLink != -1) || linkIndex != _mSelectedLink)
                {
                    _mTextPopupRectTransform.gameObject.SetActive(false);
                    _mSelectedLink = -1;
                }

                // Handle new Link selection.
                if (linkIndex != -1 && linkIndex != _mSelectedLink)
                {
                    _mSelectedLink = linkIndex;

                    TMP_LinkInfo linkInfo = _mTextMeshPro.textInfo.linkInfo[linkIndex];

                    // Debug.Log("Link ID: \"" + linkInfo.GetLinkID() + "\"   Link Text: \"" + linkInfo.GetLinkText() + "\""); // Example of how to retrieve the Link ID and Link Text.

                    Vector3 worldPointInRectangle;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(_mTextMeshPro.rectTransform, Input.mousePosition, _mCamera, out worldPointInRectangle);

                    switch (linkInfo.GetLinkID())
                    {
                        case "id_01": // 100041637: // id_01
                            _mTextPopupRectTransform.position = worldPointInRectangle;
                            _mTextPopupRectTransform.gameObject.SetActive(true);
                            _mTextPopupTMPComponent.text = KLinkText + " ID 01";
                            break;
                        case "id_02": // 100041638: // id_02
                            _mTextPopupRectTransform.position = worldPointInRectangle;
                            _mTextPopupRectTransform.gameObject.SetActive(true);
                            _mTextPopupTMPComponent.text = KLinkText + " ID 02";
                            break;
                    }
                }
                #endregion

            }
            else
            {
                // Restore any character that may have been modified
                if (_mLastIndex != -1)
                {
                    RestoreCachedVertexAttributes(_mLastIndex);
                    _mLastIndex = -1;
                }
            }

        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter()");
            _isHoveringObject = true;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit()");
            _isHoveringObject = false;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("Click at POS: " + eventData.position + "  World POS: " + eventData.worldPosition);

            // Check if Mouse Intersects any of the characters. If so, assign a random color.
            #region Character Selection Handling
            /*
            int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, m_Camera, true);
            if (charIndex != -1 && charIndex != m_lastIndex)
            {
                //Debug.Log("Character [" + m_TextMeshPro.textInfo.characterInfo[index].character + "] was selected at POS: " + eventData.position);
                m_lastIndex = charIndex;

                Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                int vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                UIVertex[] uiVertices = m_TextMeshPro.textInfo.meshInfo.uiVertices;

                uiVertices[vertexIndex + 0].color = c;
                uiVertices[vertexIndex + 1].color = c;
                uiVertices[vertexIndex + 2].color = c;
                uiVertices[vertexIndex + 3].color = c;

                m_TextMeshPro.canvasRenderer.SetVertices(uiVertices, uiVertices.Length);
            }
            */
            #endregion


            #region Word Selection Handling
            //Check if Mouse intersects any words and if so assign a random color to that word.
            /*
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, Input.mousePosition, m_Camera);

            // Clear previous word selection.
            if (m_TextPopup_RectTransform != null && m_selectedWord != -1 && (wordIndex == -1 || wordIndex != m_selectedWord))
            {
                TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[m_selectedWord];

                // Get a reference to the uiVertices array.
                UIVertex[] uiVertices = m_TextMeshPro.textInfo.meshInfo.uiVertices;

                // Iterate through each of the characters of the word.
                for (int i = 0; i < wInfo.characterCount; i++)
                {
                    int vertexIndex = m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex + i].vertexIndex;

                    Color32 c = uiVertices[vertexIndex + 0].color.Tint(1.33333f);

                    uiVertices[vertexIndex + 0].color = c;
                    uiVertices[vertexIndex + 1].color = c;
                    uiVertices[vertexIndex + 2].color = c;
                    uiVertices[vertexIndex + 3].color = c;
                }

                m_TextMeshPro.canvasRenderer.SetVertices(uiVertices, uiVertices.Length);

                m_selectedWord = -1;
            }

            // Handle word selection
            if (wordIndex != -1 && wordIndex != m_selectedWord)
            {
                m_selectedWord = wordIndex;

                TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];

                // Get a reference to the uiVertices array.
                UIVertex[] uiVertices = m_TextMeshPro.textInfo.meshInfo.uiVertices;

                // Iterate through each of the characters of the word.
                for (int i = 0; i < wInfo.characterCount; i++)
                {
                    int vertexIndex = m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex + i].vertexIndex;

                    Color32 c = uiVertices[vertexIndex + 0].color.Tint(0.75f);

                    uiVertices[vertexIndex + 0].color = c;
                    uiVertices[vertexIndex + 1].color = c;
                    uiVertices[vertexIndex + 2].color = c;
                    uiVertices[vertexIndex + 3].color = c;
                }

                m_TextMeshPro.canvasRenderer.SetVertices(uiVertices, uiVertices.Length);
            }
            */
            #endregion


            #region Link Selection Handling
            /*
            // Check if Mouse intersects any words and if so assign a random color to that word.
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
                int linkHashCode = linkInfo.hashCode;

                //Debug.Log(TMP_TextUtilities.GetSimpleHashCode("id_02"));

                switch (linkHashCode)
                {
                    case 291445: // id_01
                        if (m_LinkObject01 == null)
                            m_LinkObject01 = Instantiate(Link_01_Prefab);
                        else
                        {
                            m_LinkObject01.gameObject.SetActive(true);
                        }

                        break;
                    case 291446: // id_02
                        break;

                }

                // Example of how to modify vertex attributes like colors
                #region Vertex Attribute Modification Example
                UIVertex[] uiVertices = m_TextMeshPro.textInfo.meshInfo.uiVertices;

                Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                for (int i = 0; i < linkInfo.characterCount; i++)
                {
                    TMP_CharacterInfo cInfo = m_TextMeshPro.textInfo.characterInfo[linkInfo.firstCharacterIndex + i];

                    if (!cInfo.isVisible) continue; // Skip invisible characters.

                    int vertexIndex = cInfo.vertexIndex;

                    uiVertices[vertexIndex + 0].color = c;
                    uiVertices[vertexIndex + 1].color = c;
                    uiVertices[vertexIndex + 2].color = c;
                    uiVertices[vertexIndex + 3].color = c;
                }

                m_TextMeshPro.canvasRenderer.SetVertices(uiVertices, uiVertices.Length);
                #endregion
            }
            */
            #endregion
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log("OnPointerUp()");
        }


        void RestoreCachedVertexAttributes(int index)
        {
            if (index == -1 || index > _mTextMeshPro.textInfo.characterCount - 1) return;

            // Get the index of the material / sub text object used by this character.
            int materialIndex = _mTextMeshPro.textInfo.characterInfo[index].materialReferenceIndex;

            // Get the index of the first vertex of the selected character.
            int vertexIndex = _mTextMeshPro.textInfo.characterInfo[index].vertexIndex;

            // Restore Vertices
            // Get a reference to the cached / original vertices.
            Vector3[] srcVertices = _mCachedMeshInfoVertexData[materialIndex].vertices;

            // Get a reference to the vertices that we need to replace.
            Vector3[] dstVertices = _mTextMeshPro.textInfo.meshInfo[materialIndex].vertices;

            // Restore / Copy vertices from source to destination
            dstVertices[vertexIndex + 0] = srcVertices[vertexIndex + 0];
            dstVertices[vertexIndex + 1] = srcVertices[vertexIndex + 1];
            dstVertices[vertexIndex + 2] = srcVertices[vertexIndex + 2];
            dstVertices[vertexIndex + 3] = srcVertices[vertexIndex + 3];

            // Restore Vertex Colors
            // Get a reference to the vertex colors we need to replace.
            Color32[] dstColors = _mTextMeshPro.textInfo.meshInfo[materialIndex].colors32;

            // Get a reference to the cached / original vertex colors.
            Color32[] srcColors = _mCachedMeshInfoVertexData[materialIndex].colors32;

            // Copy the vertex colors from source to destination.
            dstColors[vertexIndex + 0] = srcColors[vertexIndex + 0];
            dstColors[vertexIndex + 1] = srcColors[vertexIndex + 1];
            dstColors[vertexIndex + 2] = srcColors[vertexIndex + 2];
            dstColors[vertexIndex + 3] = srcColors[vertexIndex + 3];

            // Restore UV0S
            // UVS0
            Vector4[] srcUV0S = _mCachedMeshInfoVertexData[materialIndex].uvs0;
            Vector4[] dstUV0S = _mTextMeshPro.textInfo.meshInfo[materialIndex].uvs0;
            dstUV0S[vertexIndex + 0] = srcUV0S[vertexIndex + 0];
            dstUV0S[vertexIndex + 1] = srcUV0S[vertexIndex + 1];
            dstUV0S[vertexIndex + 2] = srcUV0S[vertexIndex + 2];
            dstUV0S[vertexIndex + 3] = srcUV0S[vertexIndex + 3];

            // UVS2
            Vector2[] srcUV2S = _mCachedMeshInfoVertexData[materialIndex].uvs2;
            Vector2[] dstUV2S = _mTextMeshPro.textInfo.meshInfo[materialIndex].uvs2;
            dstUV2S[vertexIndex + 0] = srcUV2S[vertexIndex + 0];
            dstUV2S[vertexIndex + 1] = srcUV2S[vertexIndex + 1];
            dstUV2S[vertexIndex + 2] = srcUV2S[vertexIndex + 2];
            dstUV2S[vertexIndex + 3] = srcUV2S[vertexIndex + 3];


            // Restore last vertex attribute as we swapped it as well
            int lastIndex = (srcVertices.Length / 4 - 1) * 4;

            // Vertices
            dstVertices[lastIndex + 0] = srcVertices[lastIndex + 0];
            dstVertices[lastIndex + 1] = srcVertices[lastIndex + 1];
            dstVertices[lastIndex + 2] = srcVertices[lastIndex + 2];
            dstVertices[lastIndex + 3] = srcVertices[lastIndex + 3];

            // Vertex Colors
            srcColors = _mCachedMeshInfoVertexData[materialIndex].colors32;
            dstColors = _mTextMeshPro.textInfo.meshInfo[materialIndex].colors32;
            dstColors[lastIndex + 0] = srcColors[lastIndex + 0];
            dstColors[lastIndex + 1] = srcColors[lastIndex + 1];
            dstColors[lastIndex + 2] = srcColors[lastIndex + 2];
            dstColors[lastIndex + 3] = srcColors[lastIndex + 3];

            // UVS0
            srcUV0S = _mCachedMeshInfoVertexData[materialIndex].uvs0;
            dstUV0S = _mTextMeshPro.textInfo.meshInfo[materialIndex].uvs0;
            dstUV0S[lastIndex + 0] = srcUV0S[lastIndex + 0];
            dstUV0S[lastIndex + 1] = srcUV0S[lastIndex + 1];
            dstUV0S[lastIndex + 2] = srcUV0S[lastIndex + 2];
            dstUV0S[lastIndex + 3] = srcUV0S[lastIndex + 3];

            // UVS2
            srcUV2S = _mCachedMeshInfoVertexData[materialIndex].uvs2;
            dstUV2S = _mTextMeshPro.textInfo.meshInfo[materialIndex].uvs2;
            dstUV2S[lastIndex + 0] = srcUV2S[lastIndex + 0];
            dstUV2S[lastIndex + 1] = srcUV2S[lastIndex + 1];
            dstUV2S[lastIndex + 2] = srcUV2S[lastIndex + 2];
            dstUV2S[lastIndex + 3] = srcUV2S[lastIndex + 3];

            // Need to update the appropriate
            _mTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
