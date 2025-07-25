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
    [SerializeField] private ItembarScript _itembar;
    private KindergardenfairyScript _kindergardenfairyScript;
    [SerializeField] private CloseUpScript _closeUpScript;
    
    public GameObject kindergardenFairyCycle;
    [SerializeField] private GameObject _kindergardenFairy;
    private GameObject _clickedObject;
    private Image _cup;
    private Sprite _fullCupSprite;
    private Sprite _emptyCupSprite;
    private Animator kindergardenFairyAnimator;
    public MonologScript monologScript;
    [SerializeField] private CanopyScript _canopyScript;
    public GameObject _canopyDone;
    [SerializeField] private GameObject destroyedFlowers;
    public GameObject _treeButton;
    public GameObject credits;
    public GameObject our_credits;
    public GameObject sound_credits;

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
        
        // _itembar = GameObject.Find("ItembarManager").GetComponent<ItembarScript>();
        _kindergardenfairyScript = kindergardenFairyCycle.GetComponent<KindergardenfairyScript>();
        _fullCupSprite = Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher_voll");
        _emptyCupSprite = Resources.Load<Sprite>("Sprites/UISprites/CloseUpSprites/becher");
        kindergardenFairyAnimator = _kindergardenFairy.GetComponent<Animator>();
        kindergardenFairyAnimator.enabled = false;
        if (credits != null)
        {
            credits.SetActive(false);
            our_credits.SetActive(false);
            sound_credits.SetActive(false);
        }
        credits.SetActive(true);

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
		MinigameScript.MinigamePlayed = false;

        // Prolog abspielen --> monologbar mit Cape-Main-Charakter im hintergrund
        yield return StartCoroutine(prologScript.ShowProlog(kindergardenDialogue[0]));
        yield return new WaitUntil(() => !prologScript.prologPanel.activeSelf);
        
        // Direkt dialog mit kindergartenfee die baum abstaubt
        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[1]);
        // main character ab jetzt ohne cape 
        
        // dann frei rumbewegen
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // --> ella danach nicht mehr anklickbar
        _kindergardenFairy.GetComponent<Animator>().enabled = true;
        _kindergardenfairyScript.StartFairyCycling();
        
        // rock paper scissors kind MUSS angeklickt werden --> checken
        yield return new WaitUntil(() => MinigameScript.MinigamePlayed);

        // Sprite anpassen (Ella mit leerer Tasse)
        // _kindergardenFairy.GetComponent<SpriteRenderer>().sprite =
        //     Resources.Load<Sprite>("Sprites/CharacterSprites/kindergardenfairy/ella with cup no coffee");

        // danach wieder dialog mit ella
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");

        // Cycling anhalten
        _kindergardenfairyScript.StopCycleSprites();
        _kindergardenFairy.GetComponent<Animator>().enabled = false;

        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[2]);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);

        // Sprite anpassen (Ella ohne Tasse)
        _kindergardenFairy.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/CharacterSprites/kindergardenfairy/ella no cup");

        _kindergardenFairy.GetComponent<SpriteRenderer>().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
        
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

		// TODO Closeup script für volle tasse
        
        // wieder dialog mit ella (tasse zurückbringen)
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        dialogScript.ShowDialogueWithoutButtons(kindergardenDialogue[3]);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        
        // Sprite anpassen (Ella mit kaffee)
        _kindergardenFairy.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/CharacterSprites/kindergardenfairy/ella with coffee");
        
        // Kaffeetasse aus itembar entfernen
        _itembar.RemoveItem("becher_voll");
        
        // --> rübergehen zu raum 2 
        _treeButton.SetActive(true);
    }

    // raum 2:
    private IEnumerator House2Gameplay()
    {
		_treeButton.SetActive(false);
        destroyedFlowers.GetComponent<PolygonCollider2D>().enabled = false;
        monologScript.ShowMonolog(
            "OK I need to find the next person. What was their name again? Fera? Yes I think that was it. " +
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
        destroyedFlowers.GetComponent<PolygonCollider2D>().enabled = true;

        // frei rumlaufen
        // _clickedObject = null;
        
        // --> canopy kann nur gebaut werden, wenn 2 materialien im inventar vorhanden
        // yield return new WaitUntil(() => _itembar.FindItemSlotByName("logs") != null && _itembar.FindItemSlotByName("tuch") != null);
        // yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");

        // yield return new WaitUntil(() => _clickedObject.name == "logs");
        // _clickedObject = null;
        // canopy bauen 
        yield return new WaitUntil(() => _canopyScript.canopyBuild);
        
        // zurück zu dialog mit gartenfee
        // yield return new WaitUntil(() => _clickedObject.name != null || _clickedObject.name == "flower_fairy");
        yield return WaitForClick("flower_fairy");
        dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[2]);

        // boden fegen - optional
        // yield return new WaitUntil(() => _clickedObject.name == "broom");
        
        // wieder dialog mit gartenfee
        // yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        // dialogScript.ShowDialogueWithoutButtons(flowerfairyDialogue[3]);
        
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // --> SCHLÜSSEl 1!!!
        _closeUpScript.ShowCloseUpPanel(Resources.Load<Sprite>("Sprites/KeySprites/Gross/Gartenfee_Schluessel_gross"));
        yield return new WaitUntil(() => !InputBlocker.Instance.IsBlocked); // mal schauen ob das so geht
        key1.sprite = Resources.Load<Sprite>("Sprites/KeySprites/Klein/Gartenfee_Schluessel_klein");
        // _itembar.fold_itembar_out();
        // _treeButton.SetActive(true);
        
        // blackscreen und credits --> done
        InputBlocker.Instance.BlockInput();
        yield return new WaitForSeconds(2);
        credits.SetActive(true);
        yield return new WaitForSeconds(5);
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space));
        our_credits.SetActive(true);
        Debug.Log("our credits aktiv? " + our_credits.activeSelf);
        yield return new WaitForSeconds(7);
        our_credits.SetActive(false);
        sound_credits.SetActive(true);
        Debug.Log("sound credits aktiv? " + sound_credits.activeSelf);

        float timer = 0f;
        float timeout = 20f; // 20 Sekunden

        while (timer < timeout && !Input.GetKeyDown(KeyCode.T))
        {
            timer += Time.deltaTime;
            yield return null; // warte bis zum nächsten Frame
        }
        // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.T));
        SceneManager.LoadScene("TitleScene"); // zu titlescreen zurück
    }
    
    private IEnumerator WaitForClick(string objectName)
    {
        yield return new WaitUntil(() => _clickedObject != null && _clickedObject.name == objectName);
        _clickedObject = null;
    }
}
