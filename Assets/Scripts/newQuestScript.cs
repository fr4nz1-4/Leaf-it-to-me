using System;
using TMPro;
using UnityEngine;

public class newQuestScript : MonoBehaviour
{
    public string[] tasks;
    public TextMeshProUGUI taskText ;
    private int currentIndex = 0;

    private void Awake()
    {
        taskText.text = tasks[0];
    }

    public void nextTask()
    {
        Debug.Log("quest " + currentIndex + "abgeschlossen");
        currentIndex++;
        taskText.text = "";
        taskText.text = tasks[currentIndex];
    }
}

