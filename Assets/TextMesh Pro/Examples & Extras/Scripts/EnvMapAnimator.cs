using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class EnvMapAnimator : MonoBehaviour {

    //private Vector3 TranslationSpeeds;
    [FormerlySerializedAs("RotationSpeeds")] public Vector3 rotationSpeeds;
    private TMP_Text _mTextMeshPro;
    private Material _mMaterial;
    

    void Awake()
    {
        //Debug.Log("Awake() on Script called.");
        _mTextMeshPro = GetComponent<TMP_Text>();
        _mMaterial = _mTextMeshPro.fontSharedMaterial;
    }

    // Use this for initialization
	IEnumerator Start ()
    {
        Matrix4x4 matrix = new Matrix4x4(); 
        
        while (true)
        {
            //matrix.SetTRS(new Vector3 (Time.time * TranslationSpeeds.x, Time.time * TranslationSpeeds.y, Time.time * TranslationSpeeds.z), Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y , Time.time * RotationSpeeds.z), Vector3.one);
             matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * rotationSpeeds.x, Time.time * rotationSpeeds.y , Time.time * rotationSpeeds.z), Vector3.one);

            _mMaterial.SetMatrix("_EnvMatrix", matrix);

            yield return null;
        }
	}
}
