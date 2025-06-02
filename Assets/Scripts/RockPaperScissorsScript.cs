using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
    
    private int win_counter = 0;

    public void PlayerChoice(string playerMove)
    {
        Debug.Log("Playermovement= " + player.GetComponent<PlayerMovement>().enabled);
        string[] choices = { "Rock", "Paper", "Scissors" };
        string enemyMove = choices[Random.Range(0, choices.Length)];
        Debug.Log("playerMove: " + playerMove);
        Debug.Log("enemyMove: " + enemyMove);
        
        SetPlayerHandSprite(playerMove);
        SetEnemyHandSprite(enemyMove);
        
        string result = "";

        if (playerMove == enemyMove)
        {
            result = "Draw";
        } else if (
            (playerMove == "Rock" && enemyMove == "Scissors") ||
            (playerMove == "Paper" && enemyMove == "Rock") ||
            (playerMove == "Scissors" && enemyMove == "Paper"))
        {
            result = "You win!!";
            win_counter++;
        }
        else
        {
            result = "You lose!";
        }
        resultText.text = $"You: {playerMove}\n\n Enemy: {enemyMove}\n\n{result}";
        // if (win_counter == 3)
        // {
        //     resultText.text = $"You: {playerMove}\n\n Enemy: {enemyMove}\n\n{result}\nYou won 3 times!";
        //     StartCoroutine(CloseMinigameWithDelay());
        // }
    }

    void SetPlayerHandSprite(string move)
    {
        switch (move)
        {
            case "Rock":
                playerHandImage.sprite = playerRockSprite;
                break;
            case "Paper":
                playerHandImage.sprite = playerPaperSprite;
                break;
            case "Scissors":
                playerHandImage.sprite = playerScissorsSprite;
                break;
        }
    }

    void SetEnemyHandSprite(string move)
    {
        switch (move)
        {
            case "Rock":
                enemyHandImage.sprite = enemyRockSprite;
                break;
            case "Paper":
                enemyHandImage.sprite = enemyPaperSprite;
                break;
            case "Scissors":
                enemyHandImage.sprite = enemyScissorsSprite;
                break;
        }
    }


    private IEnumerator CloseMinigameWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        HideMinigamePanel();
        win_counter = 0;
    }
}
