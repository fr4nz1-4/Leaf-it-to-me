using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableSceneLink : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject player;
    public GameObject pausePanel;
    
    private InputBlocker _inputBlocker;
    public void ChangeScene(string sceneToLoad)
    {
        Debug.Log("called changeScene");
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void QuitGame()
    {
        Debug.Log("Spiel wird beendet..."); // Funktioniert nur im Editor
        Application.Quit();
    }

    private void Update()
    {
        _inputBlocker = GetComponent<InputBlocker>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            pausePanel.SetActive(true);
        }
    }

    public void GoBackToGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        pausePanel.SetActive(false);
    }
}
