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
    
    private int _winCounter = 0;
    private int _enemyWinCounter = 0;
    private bool _isAnimationRunning = false;

    private void Start()
    {
        playerRockAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy rock");
        playerPaperAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy paper");
        playerScissorsAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/fairy scissors");
        enemyRockAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon rock");
        enemyPaperAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon paper");
        enemyScissorsAnimation = Resources.LoadAll<Sprite>("Sprites/MinigameSprites/Rock Paper Scissors/AnimationSprites/dragon scissors");
    }

    public void PlayerChoice(string playerMove)
    {
        Debug.Log("Playermovement= " + player.GetComponent<PlayerMovement>().enabled);
        string[] choices = { "Rock", "Paper", "Scissors" };
        string enemyMove = choices[Random.Range(0, choices.Length)];
        Debug.Log("playerMove: " + playerMove);
        Debug.Log("enemyMove: " + enemyMove);
        
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

        // enemyCounter.text = $"{enemy_win_counter}";
        // playerCounter.text = $"{win_counter}";

        // if (_winCounter == 3)
        // {
        //     // new WaitUntil(() => _isAnimationRunning == false);
        //     resultText.text = $"\nYou won 3 times!\n Congratulations!";
        //     MinigamePlayed = true;
        //     Debug.Log("minigamePlayed" + MinigamePlayed);
        //     SetButtonsInteractable(false);
        //     Debug.Log(rockButton.interactable);
        // }
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


    private IEnumerator CloseMinigameWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        HideMinigamePanel();
        _winCounter = 0;
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
        
        if (_winCounter == 3)
        {
            // new WaitUntil(() => _isAnimationRunning == false);
            resultText.text = $"\nYou won 3 times!\n Congratulations!";
            MinigamePlayed = true;
            Debug.Log("minigamePlayed" + MinigamePlayed);
            SetButtonsInteractable(false);
            Debug.Log(rockButton.interactable);
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
}
