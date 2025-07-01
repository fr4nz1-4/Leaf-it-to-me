using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameplayScript : MonoBehaviour
{
    public PrologScript prologScript;
    public DialogUIScript dialogScript;
    public DialogLine[] dialogLine;
    private ItembarScript _itembar;
    private Image _cup;
    private GameObject _clickedObject;

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

        _itembar = GameObject.Find("ItembarManager").GetComponent<ItembarScript>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                _clickedObject = hit.collider.gameObject;
                Debug.Log("Angeklicktes Objekt: " + _clickedObject.name);
            }
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
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // rock paper scissors kind MUSS angeklickt werden --> checken
        yield return new WaitUntil(() => MinigameScript.minigamePlayed);
        // danach wieder dialog mit ella
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        dialogScript.ShowDialogueWithoutButtons(dialogLine[2]);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // --> man bekommt LEERE tasse in inventar
        _itembar.add_item(Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher"));
        foreach (var slot in _itembar.itemSlots)
        {
            if (slot.sprite.name == "tasse")
            {
                _cup = slot;
            }
        }
        // danach kaffeekanne finden und checken ob kaffeekanne angeklickt wurde
        yield return new WaitUntil(() => _clickedObject.name == "kaffee");
        // --> wenn ja, sprite von tasse austauschen
        _cup.sprite = Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher_voll");
        // wieder dialog mit ella (tasse zurückbringen) --> rübergehen zu raum 2 
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        dialogScript.ShowDialogueWithoutButtons(dialogLine[3]);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        SceneManager.LoadScene("House2_Scene");
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
