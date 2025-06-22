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
        Debug.Log("showCloseUpPanel button pressed");
        closeUpPanel.SetActive(false);
    }
    
    public void ShowCloseUpPanel(Sprite sprite)
    {
        closeUpPanel.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
        InputBlocker.Instance.BlockInput(); 
        StartCoroutine(setSpriteAndWait(sprite));
    }
    
    //jede zeile des dialogs anzeigen lassen und am ende zwei buttons anzeigen lassen
    private IEnumerator setSpriteAndWait(Sprite sprite)
    {
        image.sprite = sprite;
        
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        
        CloseCloseUpPanel();
    }
    
    // dialogfenster ausblenden, textfeld clearen und charakterbewegung aktivieren
    public void CloseCloseUpPanel()
    {
        closeUpPanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        InputBlocker.Instance.UnblockInput();
    }
}
