using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public MonologScript monologScript;
    public string text;
    private AudioSource audioSource;

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
    }
}
