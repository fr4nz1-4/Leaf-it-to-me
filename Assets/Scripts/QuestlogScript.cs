using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestlogScript : MonoBehaviour
{
    public GameObject questlogFoldedOut;
    public GameObject questlogFoldedIn;

    public TextMeshProUGUI[] tasks;

    const string STRIKE_START = "<s>";
    const string STRIKE_END = "</s>";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     foreach (var task in tasks)
    //     {
    //         task.enabled = false;
    //     }
    // }
    
    public void FoldQuestlogIn()
    {
        Debug.Log("Itembar einklappen");
        questlogFoldedOut.SetActive(false);
        questlogFoldedIn.SetActive(true);
    }

    public void FoldQuestlogOut()
    {
        Debug.Log("Itembar ausklappen");
        questlogFoldedOut.SetActive(true);
        questlogFoldedIn.SetActive(false);
    }

    public void CompleteTask(int taskID)
    {
        tasks[taskID].color = new Color(0.4431373f, 0.2392157f, 0.2392157f, 0.55f);
        tasks[taskID + 1].gameObject.SetActive(true);
    }
    
    // not working because of custom font
    private string StrikeText(string input)
    {
        string output = STRIKE_START + input + STRIKE_END;
        return output;
    }
}
