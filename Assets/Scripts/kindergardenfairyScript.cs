using System.Collections;
using UnityEngine;

public class KindergardenfairyScript : MonoBehaviour
{
    public GameObject[] kindergardenFairySprites;
    public float interval = 5f;
    private int _currentIndex = 0;
    private Coroutine _spriteCycleRoutine; 
    
    public void StartFairyCycling()
    {
        _spriteCycleRoutine = StartCoroutine(CycleSprites());
    }

    private IEnumerator CycleSprites()
    {
        while (true)
        {
            Debug.Log("start cycling sprites");
            // Alle ausblenden
            foreach (var fairy in kindergardenFairySprites)
            {
                fairy.SetActive(false);
                Debug.Log("alle cycle sprites deaktiviert");
            }

            // Aktuelles anzeigen
            if (kindergardenFairySprites.Length > 0)
            {
                kindergardenFairySprites[_currentIndex].SetActive(true);
            }
            Debug.Log("active Fairy: " + _currentIndex);

            // Nächster Index
            _currentIndex = (++_currentIndex) % kindergardenFairySprites.Length;
            Debug.Log("current index: " + _currentIndex);
            // Warten
            yield return new WaitForSeconds(interval);
        }
    }

    public void StopCycleSprites()
    {
        if (_spriteCycleRoutine != null)
        {
            StopCoroutine(_spriteCycleRoutine);
            _spriteCycleRoutine = null; // optional: zurücksetzen
            Debug.Log("CycleSprites gestoppt.");
        }
        foreach (var fairy in kindergardenFairySprites)
        {
            fairy.SetActive(false);
        }
        kindergardenFairySprites[0].SetActive(true);
    }
}
