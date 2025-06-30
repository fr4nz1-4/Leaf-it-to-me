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
            StartCoroutine(House2Gameplay());
        }
    }

    // raum 1:
    private IEnumerator House1Gameplay()
    {
        // Prolog abspielen --> monologbar mit Cape-Main-Charakter im hintergrund
        yield return StartCoroutine(prologScript.ShowProlog(dialogLine[0]));

        yield return new WaitUntil(() => !prologScript.prologPanel.activeSelf);
        // Direkt dialog mit kindergartenfee die baum abstaubt
        dialogScript.ShowDialogueWithoutButtons(dialogLine[1]);
        // kindergardenFairy.GetComponent<SpriteRenderer>().sprite = ;
        // --> ella danach nicht mehr anklickbar --> clickable character script entfernen
        // dann frei rumbewegen
        // rock paper scissors kind MUSS angeklickt werden --> checken
        // danach wieder dialog mit ella --> man bekommt LEERE tasse in inventar
        // dialogScript.ShowDialogueWithoutButtons(dialogLine[2]);
        // GameObject.Find("ItembarManager").GetComponent<ItembarScript>().add_item(/*leere tasse*/);
        // danach kaffeekanne finden und anklicken --> kaffee wird in tasse aufgefüllt
        // wieder dialog mit ella (tasse zurückbringen) --> rübergehen zu raum 2 
        // dialogScript.ShowDialogueWithoutButtons(dialogLine[3]);
    }

    // raum 2:
    private IEnumerator House2Gameplay()
    {
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
        yield break;
    }
}
