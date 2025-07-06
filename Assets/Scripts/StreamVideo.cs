using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideoOnClick : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject videoPanel;

    void Start()
    {
        // videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();
        // videoPanel.SetActive(false);
        videoPlayer.loopPointReached += OnVideoFinished; // ⬅ Event bei Video-Ende
    }

    // void OnPrepared(VideoPlayer vp)
    // {
    //     float aspectRatio = (float)vp.texture.width / vp.texture.height;
    //     rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(500f * aspectRatio, 500f); // z. B. 500 px Höhe
    //     rawImage.GetComponent<RawImage>().SetNativeSize(); // Optional
    // }

    
    public void PlayVideo()
    {
        videoPanel.SetActive(true);
        if (videoPlayer != null)
        {
            rawImage.gameObject.SetActive(true);
            videoPlayer.Play();
            Debug.Log("video played");
        }
    }
    
    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPanel.SetActive(false);
        Debug.Log("Video finished and panel hidden.");
    }
}