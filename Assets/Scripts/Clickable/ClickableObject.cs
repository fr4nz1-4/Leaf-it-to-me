using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class ClickableObject : MonoBehaviour
{
    public MonologScript monologScript;
    public string text;
    private AudioSource _audioSource;
    [FormerlySerializedAs("SpeechbubbleScript")] public SpeechbubbleScript speechbubbleScript;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _standardScale;
    private Vector3 _transformedScale;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _standardScale = gameObject.transform.localScale;
    }

    private void OnMouseDown()
    {
        if (InputBlocker.Instance.IsBlocked) return;
        
        Debug.Log("You clicked on: " + gameObject.name);
        
        if (monologScript != null)
        {
            monologScript.ShowMonolog(text);
        }

        if (_audioSource != null)
        {
            Debug.Log("play audiosource");
            _audioSource.Play();
        }
        
        if (speechbubbleScript != null)
        {
            speechbubbleScript.ShowSpeechbubble(text);
        }
    }
    
    private void OnMouseOver()
    {
        if (InputBlocker.Instance.IsBlocked) return;

        Debug.Log("You hovered over: " + gameObject.name);
        ColorUtility.TryParseHtmlString("#BCBCBC", out Color color);
        _spriteRenderer.color = color;

        // if (gameObject.name == "crayons" || gameObject.name == "paper on floor")
        // {
        //     return;
        // }
        // gameObject.transform.localScale = standardScale * 1.02f;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = Color.white;
        // gameObject.transform.localScale = standardScale;
    }
}
