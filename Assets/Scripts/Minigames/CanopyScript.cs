using System;
using UnityEngine;

public class CanopyScript : MinigameScript
{
    public GameObject plane;
    public GameObject barren1;
    public GameObject barren2;
    public GameObject barren3;
    public GameObject barren4;

    public GameObject canopyFinal;
    
    private void Update()
    {
        if (plane.activeSelf && barren1.activeSelf && barren2.activeSelf && barren3.activeSelf && barren4.activeSelf)
        {
            canopyFinal.SetActive(true);
            MinigamePlayed = true;
        }
    }
}
