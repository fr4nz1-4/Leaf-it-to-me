using TMPro;
using UnityEngine;
using UnityEngine.UI;

// opens DialogPanel, starts Dialog, contains all Dialog-Logic
public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;

    public void quitDialog()
    {
        dialogPanel.SetActive(false);
    }
}
