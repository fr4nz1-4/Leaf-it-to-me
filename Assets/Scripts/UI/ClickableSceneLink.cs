using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ClickableSceneLink : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject player;
    public GameObject pausePanel;
    [SerializeField] private Animator _transitionAnim;
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
            InputBlocker.Instance.BlockInput(); 
        }
    }

    public void GoBackToGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        pausePanel.SetActive(false);
        InputBlocker.Instance.UnblockInput(); 
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        _transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _transitionAnim.SetTrigger("Start");
    }
}
