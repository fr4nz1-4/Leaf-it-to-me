using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RockPaperScissorsScript : MinigameScript
{
    public Image playerHandImage;
    public Image enemyHandImage;

    public Sprite playerRockSprite;
    public Sprite playerPaperSprite;
    public Sprite playerScissorsSprite;
    
    public Sprite enemyRockSprite;
    public Sprite enemyPaperSprite;
    public Sprite enemyScissorsSprite;

    public Sprite[] playerRockAnimation;
    public Sprite[] playerPaperAnimation;
    public Sprite[] playerScissorsAnimation;
    public Sprite[] enemyRockAnimation;
    public Sprite[] enemyPaperAnimation;
    public Sprite[] enemyScissorsAnimation;

    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;
    
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerCounter;
    public TextMeshProUGUI enemyCounter;
    [SerializeField] private Image textBackground;
    
    private int _winCounter = 0;
    private int _enemyWinCounter = 0;
    private bool _isAnimationRunning = false;
    private bool _resetOnNextMove = false;
    
    private void Start()
    {
        playerRockAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy rock");
        playerPaperAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy paper");
        playerScissorsAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy scissors");
        enemyRockAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon rock");
        enemyPaperAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon paper");
        enemyScissorsAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon scissors");
        
        resultText.text = "";
        playerCounter.text = "";
        enemyCounter.text = "";
    }

    private void Update()
    {
        if (resultText.text.Length == 0)
        {
            textBackground.gameObject.SetActive(false);
        }
        else
        {
            textBackground.gameObject.SetActive(true);
        }
    }

    public void PlayerChoice(string playerMove)
    {
        if (_resetOnNextMove)
        {
            _winCounter = 0;
            _enemyWinCounter = 0;
            playerCounter.text = "0";
            enemyCounter.text = "0";
            _resetOnNextMove = false;
        }
        
        Debug.Log("Playermovement= " + player.GetComponent<PlayerMovement>().enabled);
        string[] choices = { "Rock", "Paper", "Scissors" };
        string enemyMove = choices[Random.Range(0, choices.Length)];
        Debug.Log("playerMove: " + playerMove);
        Debug.Log("enemyMove: " + enemyMove);
        
        resultText.text = "";
        SetPlayerHandSprite(playerMove);
        SetEnemyHandSprite(enemyMove);
        
        // string result = "";

        if (playerMove == enemyMove)
        {
            // result = "Draw";
        } else if (
            (playerMove == "Rock" && enemyMove == "Scissors") ||
            (playerMove == "Paper" && enemyMove == "Rock") ||
            (playerMove == "Scissors" && enemyMove == "Paper"))
        {
            // result = "You win!!";
            _winCounter++;
        }
        else
        {
            // result = "You lose!";
            _enemyWinCounter++;
        }
    }

    void SetPlayerHandSprite(string move)
    {
        SetButtonsInteractable(false);
        switch (move)
        {
            case "Rock":
                StartCoroutine(PlayPlayerAnimation(playerRockAnimation, playerRockSprite));
                break;
            case "Paper":
                StartCoroutine(PlayPlayerAnimation(playerPaperAnimation, playerPaperSprite));
                break;
            case "Scissors":
                StartCoroutine(PlayPlayerAnimation(playerScissorsAnimation, playerScissorsSprite));
                break;
        }
    }

    void SetEnemyHandSprite(string move)
    {
        SetButtonsInteractable(false);
        switch (move)
        {
            case "Rock":
                StartCoroutine(PlayEnemyAnimation(enemyRockAnimation, enemyRockSprite));
                break;
            case "Paper":
                StartCoroutine(PlayEnemyAnimation(enemyPaperAnimation, enemyPaperSprite));
                break;
            case "Scissors":
                StartCoroutine(PlayEnemyAnimation(enemyScissorsAnimation, enemyScissorsSprite));
                break;
        }
    }
    
    private IEnumerator PlayPlayerAnimation(Sprite[] animationFrames, Sprite finalSprite)
    {
        _isAnimationRunning = true;
        float frameDuration = 0.1f; // Dauer eines Frames (z.B. 0.1 Sekunden)

        foreach (var frame in animationFrames)
        {
            playerHandImage.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }

        // playerHandImage.sprite = finalSprite;
        playerCounter.text = $"{_winCounter}";
        SetButtonsInteractable(true);
        _isAnimationRunning = false;
        
        if (_winCounter == 3 && _enemyWinCounter < 3)
        {
            // new WaitUntil(() => _isAnimationRunning == false);
            resultText.text = $"You won!\nCongratulations!";
            MinigamePlayed = true;
            _resetOnNextMove = true;
            Debug.Log("minigamePlayed" + MinigamePlayed);
            SetButtonsInteractable(false);
            Debug.Log("Buttons interactable:"  + rockButton.interactable);
        } else if (_enemyWinCounter == 3 && _winCounter < 3)
        {
            resultText.text = $"You lose!\nTry again!";
            _resetOnNextMove = true;
        }
    }
    
    private IEnumerator PlayEnemyAnimation(Sprite[] animationFrames, Sprite finalSprite)
    {
        
        float frameDuration = 0.1f; // Dauer eines Frames (z.B. 0.1 Sekunden)

        foreach (var frame in animationFrames)
        {
            enemyHandImage.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }

        // enemyHandImage.sprite = finalSprite;
        enemyCounter.text = $"{_enemyWinCounter}";
        SetButtonsInteractable(true);
    }
    
    void SetButtonsInteractable(bool interactable)
    {
        rockButton.interactable = interactable;
        paperButton.interactable = interactable;
        scissorsButton.interactable = interactable;
    }

    public new void HideMinigamePanel()
    {
        Debug.Log("hideMinigame button pressed");
        minigamePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput();
        // treeButton.SetActive(true);
        keys.SetActive(true);

        resultText.text = "";
        playerCounter.text = "";
        enemyCounter.text = "";
    }
}
