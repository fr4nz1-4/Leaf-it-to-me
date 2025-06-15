using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableSceneLink : MonoBehaviour
{
    public string sceneToLoad;

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
    
}
