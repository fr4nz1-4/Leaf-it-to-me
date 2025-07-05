using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloseUpScript : MonoBehaviour
{
    public GameObject closeUpPanel;
    public Image image;
    public GameObject player;

    public void Start()
    {
        closeUpPanel.SetActive(false);
    }
    
    public void ShowCloseUpPanel(Sprite sprite)
    {
        Debug.Log("showCloseUpPanel button pressed");
        closeUpPanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        InputBlocker.Instance.BlockInput(); 
        StartCoroutine(SetSpriteAndWait(sprite));
    }
    
    private IEnumerator SetSpriteAndWait(Sprite sprite)
    {
        image.sprite = sprite;
        // yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitForSeconds(1.5f);
        CloseCloseUpPanel();
    }
    
    // closeuppanel ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseCloseUpPanel()
    {
        closeUpPanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput();
    }
}
