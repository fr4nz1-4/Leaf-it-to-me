using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public GameObject minigamePanel;
    public TextMeshProUGUI resultText;

    public Image playerHandImage;
    public Image enemyHandImage;

    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    
    public bool isMinigameActive = false;
    private int win_counter = 0;

    public void ShowMinigamePanel()
    {
        Debug.Log("showMinigame button pressed");
        minigamePanel.SetActive(true);
        resultText.text = "";
        isMinigameActive = true;
    }

    public void HideMinigamePanel()
    {
        Debug.Log("hideMinigame button pressed");
        minigamePanel.SetActive(false);
        isMinigameActive = false;
    }

    public void PlayerChoice(string playerMove)
    {
        string[] choices = { "Rock", "Paper", "Scissors" };
        string enemyMove = choices[Random.Range(0, choices.Length)];
        Debug.Log("playerMove: " + playerMove);
        Debug.Log("enemyMove: " + enemyMove);
        
        SetHandSprite(playerHandImage, playerMove);
        SetHandSprite(enemyHandImage, enemyMove);
        
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
        if (win_counter == 3)
        {
            resultText.text = "You won 3 times!";
            StartCoroutine(CloseMinigameWithDelay());
        }
    }

    void SetHandSprite(Image image, string move)
    {
        switch (move)
        {
            case "Rock":
                image.sprite = rockSprite;
                break;
            case "Paper":
                image.sprite = paperSprite;
                break;
            case "Scissors":
                image.sprite = scissorsSprite;
                break;
        }
    }

    private IEnumerator CloseMinigameWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        isMinigameActive = false;
        minigamePanel.SetActive(false);
        win_counter = 0;
    }
}
