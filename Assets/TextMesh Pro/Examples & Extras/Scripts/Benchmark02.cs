using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{

    public class Benchmark02 : MonoBehaviour
    {

        [FormerlySerializedAs("SpawnType")] public int spawnType = 0;
        [FormerlySerializedAs("NumberOfNPC")] public int numberOfNpc = 12;

        [FormerlySerializedAs("IsTextObjectScaleStatic")] public bool isTextObjectScaleStatic;
        private TextMeshProFloatingText _floatingTextScript;


        void Start()
        {

            for (int i = 0; i < numberOfNpc; i++)
            {


                if (spawnType == 0)
                {
                    // TextMesh Pro Implementation
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMeshPro textMeshPro = go.AddComponent<TextMeshPro>();

                    textMeshPro.autoSizeTextContainer = true;
                    textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

                    textMeshPro.alignment = TextAlignmentOptions.Bottom;
                    textMeshPro.fontSize = 96;
                    textMeshPro.fontFeatures.Clear();

                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    textMeshPro.text = "!";
                    textMeshPro.isTextObjectScaleStatic = isTextObjectScaleStatic;

                    // Spawn Floating Text
                    _floatingTextScript = go.AddComponent<TextMeshProFloatingText>();
                    _floatingTextScript.spawnType = 0;
                    _floatingTextScript.isTextObjectScaleStatic = isTextObjectScaleStatic;
                }
                else if (spawnType == 1)
                {
                    // TextMesh Implementation
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMesh textMesh = go.AddComponent<TextMesh>();
                    textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
                    textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;

                    textMesh.anchor = TextAnchor.LowerCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "!";

                    // Spawn Floating Text
                    _floatingTextScript = go.AddComponent<TextMeshProFloatingText>();
                    _floatingTextScript.spawnType = 1;
                }
                else if (spawnType == 2)
                {
                    // Canvas WorldSpace Camera
                    GameObject go = new GameObject();
                    Canvas canvas = go.AddComponent<Canvas>();
                    canvas.worldCamera = Camera.main;

                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));

                    TextMeshProUGUI textObject = new GameObject().AddComponent<TextMeshProUGUI>();
                    textObject.rectTransform.SetParent(go.transform, false);

                    textObject.color = new Color32(255, 255, 0, 255);
                    textObject.alignment = TextAlignmentOptions.Bottom;
                    textObject.fontSize = 96;
                    textObject.text = "!";

                    // Spawn Floating Text
                    _floatingTextScript = go.AddComponent<TextMeshProFloatingText>();
                    _floatingTextScript.spawnType = 0;
                }



            }
        }
    }
}
