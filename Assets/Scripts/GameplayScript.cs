using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameplayScript : MonoBehaviour
{
    public PrologScript prologScript;
    public DialogUIScript dialogScript;
    public DialogLine[] dialogLine;

    public GameObject kindergardenFairy;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "House1_Scene")
        {
            StartCoroutine(House1Gameplay());
        } else if (SceneManager.GetActiveScene().name == "House2_Scene")
        {
            
        }
    }

    private IEnumerator House1Gameplay()
    {
        prologScript.prologPanel.SetActive(true);
        // Prolog abspielen --> monologbar mit Cape-Main-Charakter im hintergrund
        yield return StartCoroutine(prologScript.ShowProlog(dialogLine[0]));

        yield return new WaitUntil(() => !prologScript.prologPanel.activeSelf);
        // Direkt dialog mit kindergartenfee die baum abstaubt
        dialogScript.ShowDialogueWithoutButtons(dialogLine[1]);
        // kindergardenFairy.GetComponent<SpriteRenderer>().sprite = ;
        // InputBlocker.Instance.BlockInput(); // --> evtl nicht nötig, kann auch durch monolog gesperrt werden

    }
    // raum 1:
    // --> ella danach nicht mehr anklickbar
    // dann frei rumbewegen
    // rock paper scissors kind MUSS angeklickt werden+
    // danach wieder dialog mit ella --> man bekommt LEERE tasse in inventar
    // danach kaffeekanne finden und anklicken --> kaffee wird in tasse aufgefüllt
    // wieder dialog mit ella (tasse zurückbringen) --> rübergehen zu raum 2 

    // raum 2:
    // dialog mit gartenfee
    // rübergehen zu dusche
    // --> auf dusche klicken --> sauber
    // zurück zu dialog mit Gartenfee
    // frei rumlaufen
    // --> canopy kann nur gebaut werden, wenn 2 materialien im inventar vorhanden
    // canopy bauen 
    // zurück zu dialog mit gartenfee
    // boden fegen
    // wieder dialog mit gartenfee --> SCHLÜSSEl 1!!!
    // blackscreen und credits --> done
}
