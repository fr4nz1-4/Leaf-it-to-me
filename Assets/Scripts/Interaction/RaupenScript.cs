using System;
using UnityEngine;

public class RaupenScript : MonoBehaviour
{
    [SerializeField] private AudioSource defaultSound;
    [SerializeField] private AudioSource specialSound;
    private int _counter = 0;
    private void OnMouseDown()
    {
        _counter++;
        
        if (_counter % 6 == 0)
        {
            specialSound.Play();
        }
        else
        {
            defaultSound.Play();
        }
    }
}
