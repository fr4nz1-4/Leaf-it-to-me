using System;
using UnityEngine;

public class ClickableSceneLink : MonoBehaviour
{
    public string sceneToLoad;

    public void OnMouseDown()
    {
        SceneLoader.LoadSceneByName(sceneToLoad);
    }
}
