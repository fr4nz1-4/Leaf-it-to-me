using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ClickableObject : MonoBehaviour
{
    public MonologScript monologScript;
    public string text;
    private AudioSource audioSource;
    public SpeechbubbleScript SpeechbubbleScript;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        Debug.Log("You clicked on: " + gameObject.name);
        
        if (monologScript != null)
        {
            monologScript.ShowMonolog(text);
        }

        if (audioSource != null)
        {
            Debug.Log("play audiosource");
            audioSource.Play();
        }
        
        if (SpeechbubbleScript != null)
        {
            SpeechbubbleScript.ShowSpeechbubble(text);
        }
    }
}
