using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 30f;
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        Debug.Log("in TypewriterEffect.run method");
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        Debug.Log("in type text method");
        textLabel.text = string.Empty;
        
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime;
			//fast forward the text animation on mouse click / space
            if (t > 0.3f && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
            {
                break;
            }
            charIndex = Mathf.FloorToInt(t * typewriterSpeed);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);
            
            yield return null;
        }
        
        textLabel.text = textToType;
        //prevent skipping text too fast
        yield return new WaitForSeconds(0.3f);
    }
}
