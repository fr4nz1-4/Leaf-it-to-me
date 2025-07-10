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
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex) + Invisible(textToType.Substring(charIndex));

            yield return null;
        }
        
        textLabel.text = textToType;
    }

    /*
     * Creates transparent text to keep a paragraphs width "stable" during typing animation
     */
    private string Invisible(string text)
    {
        return "<color=#00000000>" + text + "</color>";
    }
}
