using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoClipPlayer : MonoBehaviour {
    // Components
    [SerializeField] private VideoPlayer myVideoPlayer;
    // Properties
    private bool isPlaying;
    // References
    private VideoClipController myClipController;



    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public void Initialize(VideoClipController _clipController) {
        myClipController = _clipController;
        Hide();
    }


    // ----------------------------------------------------------------
    //  Play / Pause
    // ----------------------------------------------------------------
    public void Hide() {
        this.gameObject.SetActive(false);
    }
    public void Play() {
        this.gameObject.SetActive(true);
        myVideoPlayer.Play();
        isPlaying = true;
    }



    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    private void OnVideoFinished() {
        isPlaying = false;
        this.gameObject.SetActive(false);
        myClipController.OnVideoFinished();
    }



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        if (isPlaying) {
            if (myVideoPlayer.time >= myVideoPlayer.length) {
                OnVideoFinished();
            }
        }
    }
}
