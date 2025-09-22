using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameplayScript : MonoBehaviour
{
    // --------------------------- allgemein ---------------------------
    public DialogUIScript dialogScript;
    public MonologScript monologScript;
    [SerializeField] private ItembarScript _itembar;
    [SerializeField] private CloseUpScript _closeUpScript;
    [SerializeField] private QuestlogScript questlogScript;
    [SerializeField] private newQuestScript questScript;
    public GameObject _treeButton;
    private GameObject _clickedObject;

    // --------------------------- room 1 ---------------------------
    public PrologScript prologScript;
    public DialogLine[] kindergardenDialogue;
    private KindergardenfairyScript _kindergardenfairyScript;
    [SerializeField] private GameObject _kindergardenFairy;
    [SerializeField] private Sprite kindergardenFairyNoCup;
    [SerializeField] private Sprite kindergardenFairyWithCoffee;
    public GameObject kindergardenFairyCycle;
    [SerializeField] private GameObject coffee;
    [SerializeField] private Sprite _fullCupSprite;
    [SerializeField] private Sprite _emptyCupSprite;
    private Image _cup;
    private Animator kindergardenFairyAnimator;
    
    // --------------------------- room 2 ---------------------------
    public DialogLine[] flowerfairyDialogue;
    public GameObject flowerfairy;
    public Sprite flowerfairySprite2;
    [SerializeField] private CanopyScript _canopyScript;
    public GameObject _canopyDone;
    [SerializeField] private GameObject destroyedFlowers;
    public Image key1Image;
    [SerializeField] private Sprite key1SpriteSmall;
    [SerializeField] private Sprite key1SpriteCloseUp;
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
		RockPaperScissorsScript.MinigamePlayed = false;
        TicTacToeScript.MinigamePlayed = false;
        coffee.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        // Prolog abspielen --> monologbar mit Cape-Main-Charakter im hintergrund
        yield return StartCoroutine(prologScript.ShowProlog(kindergardenDialogue[0]));
        yield return new WaitUntil(() => !prologScript.prologPanel.activeSelf);
        
        // Direkt dialog mit kindergartenfee die baum abstaubt
        dialogScript.ShowDialogue(kindergardenDialogue[1], false, false);
        // main character ab jetzt ohne cape 
        // dann frei rumbewegen
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);

        // questlogScript.FoldQuestlogOut();

        // --> ella danach nicht mehr anklickbar
        _kindergardenFairy.GetComponent<Animator>().enabled = true;
        _kindergardenfairyScript.StartFairyCycling();
        
        // rock paper scissors kind MUSS angeklickt werden --> checken
        yield return new WaitUntil(() => RockPaperScissorsScript.MinigamePlayed);
        questScript.nextTask();

        // questlogScript.CompleteTask(0);
        // questlogScript.FoldQuestlogOut();
        
        yield return new WaitUntil(() => TicTacToeScript.MinigamePlayed);
        // questlogScript.CompleteTask(1);
        questScript.nextTask();

        // Sprite anpassen (Ella mit leerer Tasse)
        // _kindergardenFairy.GetComponent<SpriteRenderer>().sprite =
        //     Resources.Load<Sprite>("Sprites/CharacterSprites/kindergardenfairy/ella with cup no coffee");

        // danach wieder dialog mit ella
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");

        // Cycling anhalten
        _kindergardenfairyScript.StopCycleSprites();
        _kindergardenFairy.GetComponent<Animator>().enabled = false;

        dialogScript.ShowDialogue(kindergardenDialogue[2], false, false);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        coffee.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        // questlogScript.CompleteTask(2);
        questScript.nextTask();

        // Sprite anpassen (Ella ohne Tasse)
        _kindergardenFairy.GetComponent<SpriteRenderer>().sprite = kindergardenFairyNoCup;

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
        
        // questlogScript.CompleteTask(3);
        questScript.nextTask();

		// Closeup script für volle tasse
        // _closeUpScript.ShowCloseUpPanel(_fullCupSprite);
        
        // wieder dialog mit ella (tasse zurückbringen)
        yield return new WaitUntil(() => _clickedObject.name == "kindergarden_fairy");
        dialogScript.ShowDialogue(kindergardenDialogue[3], false, false);
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        
        // questlogScript.CompleteTask(4);
        questScript.nextTask();

        // Sprite anpassen (Ella mit kaffee)
        _kindergardenFairy.GetComponent<SpriteRenderer>().sprite = kindergardenFairyWithCoffee;
        
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
        // flowerfairy.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        
        monologScript.ShowMonolog(
            "OK I need to find the next person. What was their name again? Fera? Yes I think that was it. " +
            "Funny, why are some of the flowerbeds looking like a mess? What happened there?");
        
        yield return new WaitUntil(() => !monologScript.monologPanel.activeSelf);
        
        // questlogScript.FoldQuestlogOut();

        // dialog mit gartenfee
        yield return new WaitUntil(() => _clickedObject != null && _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogue(flowerfairyDialogue[0], false, false);
        
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // questlogScript.CompleteTask(0);
        // questlogScript.FoldQuestlogOut();
        questScript.nextTask();

        // rübergehen zu dusche
        // --> auf dusche klicken --> sauber (Animation)
        yield return new WaitUntil(() => _clickedObject.name == "sunflower");

        // questlogScript.CompleteTask(1);
        // questlogScript.FoldQuestlogOut();
        questScript.nextTask();

        // zurück zu dialog mit Gartenfee
        yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        dialogScript.ShowDialogue(flowerfairyDialogue[1], false, false);
        destroyedFlowers.GetComponent<PolygonCollider2D>().enabled = true;
        
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // questlogScript.CompleteTask(2);
        // questlogScript.FoldQuestlogOut();
        questScript.nextTask();

        // Sprite der Flowerfairy ändern
        flowerfairy.gameObject.GetComponent<SpriteRenderer>().sprite = flowerfairySprite2;
        flowerfairy.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        // flowerfairy.gameObject.GetComponent<Animator>().
        Destroy(flowerfairy.gameObject.GetComponent<PolygonCollider2D>());
        flowerfairy.gameObject.AddComponent<PolygonCollider2D>();
        // flowerfairy.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
        
        // anleitung was als nächstes tun (canopy bauen)
        // monologScript.ShowMonolog("I have to get the problem fixed to get the key but how? " +
        //                           "Maybe I could build a contraption to block the potions falling on the flowerbeds.\n" +
        //                           "I need to look for some materials around here to build some kind of canopy.");
        
        // frei rumlaufen
        // _clickedObject = null;
        
        // --> canopy kann nur gebaut werden, wenn 2 materialien im inventar vorhanden
        // yield return new WaitUntil(() => _itembar.FindItemSlotByName("logs") != null && _itembar.FindItemSlotByName("tuch") != null);
        // yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");

        // yield return new WaitUntil(() => _clickedObject.name == "logs");
        // _clickedObject = null;
        // canopy bauen 
        yield return new WaitUntil(() => _canopyScript.canopyBuild);
        yield return new WaitUntil(() => !_canopyScript.minigamePanel.activeSelf);
        
        // questlogScript.CompleteTask(3);
        // questlogScript.FoldQuestlogOut();
        questScript.nextTask();

        // zurück zu dialog mit gartenfee
        // yield return new WaitUntil(() => _clickedObject.name != null || _clickedObject.name == "flower_fairy");
        yield return WaitForClick("flower_fairy");
        dialogScript.ShowDialogue(flowerfairyDialogue[2], false, false);

        // boden fegen - optional
        // yield return new WaitUntil(() => _clickedObject.name == "broom");
        
        // wieder dialog mit gartenfee
        // yield return new WaitUntil(() => _clickedObject.name == "flower_fairy");
        // dialogScript.ShowDialogue(flowerfairyDialogue[3]);
        
        yield return new WaitUntil(() => !dialogScript.dialogPanel.activeSelf);
        // questlogScript.FoldQuestlogIn();

        // --> SCHLÜSSEl 1!!!
        _closeUpScript.ShowCloseUpPanel(key1SpriteCloseUp);
        yield return new WaitUntil(() => !InputBlocker.Instance.IsBlocked); // mal schauen ob das so geht
        key1Image.sprite = key1SpriteSmall;
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
