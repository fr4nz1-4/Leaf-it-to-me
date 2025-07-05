using System.Collections;
using UnityEngine;

public class kindergardenfairyScript : MonoBehaviour
{
    public GameObject[] kindergardenFairySprites;
    public float interval = 5f;
    private int currentIndex = 0;
    
    public void StartFairyCycling()
    {
        StartCoroutine(CycleSprites());
    }

    private IEnumerator CycleSprites()
    {
        while (true)
        {
            // Alle ausblenden
            for (int i = 0; i < kindergardenFairySprites.Length; i++)
            {
                kindergardenFairySprites[i].SetActive(false);
            }

            // Aktuelles anzeigen
            kindergardenFairySprites[currentIndex].SetActive(true);

            // Nächster Index
            currentIndex = (currentIndex + 1) % kindergardenFairySprites.Length;

            // Warten
            yield return new WaitForSeconds(interval);
        }
    }

    public void StopCycleSprites()
    {
        StopCoroutine(CycleSprites());
        for (int i = 0; i < kindergardenFairySprites.Length; i++)
        {
            kindergardenFairySprites[i].SetActive(false);
        }
        // kindergardenFairySprites[ ].SetActive(true);
    }
}
