using System;
using UnityEngine;

public class ClickableSceneLink : MonoBehaviour
{
    public string sceneToLoad;

    public void ChangeScene(string sceneToLoad)
    {
        Debug.Log("called changeScene");
        SceneLoader.LoadSceneByName(sceneToLoad);
    }
}
