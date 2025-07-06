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
    public DialogLine[] kindergardenDialogue;
    public DialogLine[] flowerfairyDialogue;
    public Image key1;
    private ItembarScript _itembar;
    private KindergardenfairyScript _kindergardenfairyScript;
    private CloseUpScript _closeUpScript;
    
    public GameObject kindergardenFairy;
    private Sprite _kindergardenFairySprite;
    private GameObject _clickedObject;
    private Image _cup;
    private Sprite _fullCupSprite;
    private Sprite _emptyCupSprite;
    public MonologScript monologScript;
    private CanopyScript _canopyScript;
    public GameObject _treeButton;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "House1_Scene")
        {
            StartCoroutine(House1Gameplay());
        } else if (SceneManager.GetActiveScene().name == "House2_Scene")
        {
            StopCoroutine(House1Gameplay());
            StartCoroutine(House2Gameplay());
        }
        
        _itembar = GameObject.Find("ItembarManager").GetComponent<ItembarScript>();
        _kindergardenfairyScript = kindergardenFairy.GetComponent<KindergardenfairyScript>();
        _fullCupSprite = Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher_voll");
        _emptyCupSprite = Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher");
        _kindergardenFairySprite = kindergardenFairy.GetComponent<SpriteRenderer>().sprite;
        // _monologScript = GameObject.Find("MonologScript").GetComponent<MonologScript>();
        _closeUpScript = GameObject.Find("CloseUpManager").GetComponent<CloseUpScript>();
        _canopyScript = GameObject.Find("destroyed_flowers").GetComponent<CanopyScript>();
        // _treeButton = GameObject.Find("TreeButton");
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
        _treeButton.SetActive(false);
        // Prolog abspielen --> monologbar mit Cape-Main-Charakter im hintergrund
        yield return StartCoroutine(prologScript.ShowProlog(kindergardenDialogue[0]));
        yield return new WaitUntil(() => !prologScript.prologPanel.activeSelf);
        
        // Direkt dialog mit kindergartenfee die baum abstaubt
        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[1]);
        // main character ab jetzt ohne cape 
        
        // dann frei rumbewegen
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // --> ella danach nicht mehr anklickbar
        _kindergardenfairyScript.StartFairyCycling();
        
        // rock paper scissors kind MUSS angeklickt werden --> checken
        yield return new WaitUntil(() => MinigameScript.MinigamePlayed);
        
        // danach wieder dialog mit ella
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        _kindergardenfairyScript.StopCycleSprites();
        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[2]);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        
        // --> man bekommt LEERE tasse in inventar
        _itembar.add_item(_emptyCupSprite);
        _itembar.fold_itembar_out();
        // yield return new WaitUntil(() => !_itembar._isItembarfoldedOut);
        
        yield return null; // nächstes Frame abwarten
        
        _cup = _itembar.FindItemSlotByName("becher");
        Debug.Log("cup name: " + _cup.sprite.name);
        
        // danach kaffeekanne finden und checken ob kaffeekanne angeklickt wurde
        yield return new WaitUntil(() => _clickedObject.name == "kaffee");
        Debug.Log("you clicked on kaffee"); // wird nie angezeigt
        
        // --> wenn ja, sprite von tasse austauschen
        _itembar.ReplaceItemSprite(_cup, _fullCupSprite);
        
        // wieder dialog mit ella (tasse zurückbringen) --> rübergehen zu raum 2 
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[3]);
        _kindergardenFairySprite =
            Resources.Load<Sprite>("Sprites/CharacterSprites/kindergardenfairy/ella with coffee");
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        
        _treeButton.SetActive(true);
    }

    // raum 2:
    private IEnumerator House2Gameplay()
    {
        _treeButton.SetActive(false);
        monologScript.ShowMonolog(
            "OK I need to find the next person. What was their name again? Fera? Yes I think that was it." +
            "Funny, why are some of the flowerbeds looking like a mess? What happened there?");
        // dialog mit gartenfee
        yield return new WaitUntil(() => _clickedObject != null && _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[0]);
        // rübergehen zu dusche
        // --> auf dusche klicken --> sauber (Animation)
        yield return new WaitUntil(() => _clickedObject.name == "sunflower");

        // zurück zu dialog mit Gartenfee
        yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[1]);
        // frei rumlaufen
        
        // --> canopy kann nur gebaut werden, wenn 2 materialien im inventar vorhanden
        yield return new WaitUntil(() => _itembar.FindItemSlotByName("logs") != null && _itembar.FindItemSlotByName("tuch") != null);
        // yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");

        // canopy bauen 
        yield return new WaitUntil(() => _canopyScript.canopyBuild);
        
        // zurück zu dialog mit gartenfee
        yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[2]);

        // boden fegen - optionald
        
        // wieder dialog mit gartenfee
        yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[3]);
        
        // --> SCHLÜSSEl 1!!!
        _closeUpScript.ShowCloseUpPanel(Resources.Load<Sprite>("Sprites/KeySprites/Gross/Gartenfee_Schluessel_gross"));
        yield return new WaitUntil(() => !InputBlocker.Instance.IsBlocked); // mal schauen ob das so geht
        key1.sprite = Resources.Load<Sprite>("Sprites/KeySprites/Klein/Gartenfee_Schluessel_klein");
        _itembar.fold_itembar_out();
        // _treeButton.SetActive(true);

        // blackscreen und credits --> done
    }
}
