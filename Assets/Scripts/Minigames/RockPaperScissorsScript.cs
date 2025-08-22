using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RockPaperScissorsScript : MinigameScript
{
    [SerializeField] private Image playerHandImage;
    [SerializeField] private Image enemyHandImage;

    [SerializeField] private Sprite playerRockSprite;
    [SerializeField] private Sprite playerPaperSprite;
    [SerializeField] private Sprite playerScissorsSprite;
    
    [SerializeField] private Sprite enemyRockSprite;
    [SerializeField] private Sprite enemyPaperSprite;
    [SerializeField] private Sprite enemyScissorsSprite;

    [SerializeField] private Sprite[] playerRockAnimation;
    [SerializeField] private Sprite[] playerPaperAnimation;
    [SerializeField] private Sprite[] playerScissorsAnimation;
    [SerializeField] private Sprite[] enemyRockAnimation;
    [SerializeField] private Sprite[] enemyPaperAnimation;
    [SerializeField] private Sprite[] enemyScissorsAnimation;

    [SerializeField] private Button rockButton;
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;
    
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI playerCounter;
    [SerializeField] private TextMeshProUGUI enemyCounter;
    [SerializeField] private Image textBackground;
    
    private int _winCounter;
    private int _enemyWinCounter;
    private bool _resetOnNextMove;

    private readonly float _frameDuration = 0.1f; // Dauer eines Frames (z.B. 0.1 Sekunden)

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
        textBackground.gameObject.SetActive(resultText.text.Length != 0);
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
                StartCoroutine(PlayPlayerAnimation(playerRockAnimation));
                break;
            case "Paper":
                StartCoroutine(PlayPlayerAnimation(playerPaperAnimation));
                break;
            case "Scissors":
                StartCoroutine(PlayPlayerAnimation(playerScissorsAnimation));
                break;
        }
    }

    void SetEnemyHandSprite(string move)
    {
        SetButtonsInteractable(false);
        switch (move)
        {
            case "Rock":
                StartCoroutine(PlayEnemyAnimation(enemyRockAnimation));
                break;
            case "Paper":
                StartCoroutine(PlayEnemyAnimation(enemyPaperAnimation));
                break;
            case "Scissors":
                StartCoroutine(PlayEnemyAnimation(enemyScissorsAnimation));
                break;
        }
    }
    
    private IEnumerator PlayPlayerAnimation(Sprite[] animationFrames)
    {
        foreach (var frame in animationFrames)
        {
            playerHandImage.sprite = frame;
            yield return new WaitForSeconds(_frameDuration);
        }

        playerCounter.text = $"{_winCounter}";
        SetButtonsInteractable(true);
        
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
    
    private IEnumerator PlayEnemyAnimation(Sprite[] animationFrames)
    {
        foreach (var frame in animationFrames)
        {
            enemyHandImage.sprite = frame;
            yield return new WaitForSeconds(_frameDuration);
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
