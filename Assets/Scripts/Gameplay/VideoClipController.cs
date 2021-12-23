using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoClipController : MonoBehaviour {
    // Components
    [SerializeField] private Image i_scrim;
    private Dictionary<string, VideoClipPlayer> clipsDict;



    private VideoClipPlayer GetClip(string vidName) {
        //    // Note: Deciding to do this at runtime, to lighten the initialization load.
        //    VideoClipPlayer[] myClips = GetComponentsInChildren<VideoClipPlayer>();
        //    foreach (VideoClipPlayer clip in myClips) {
        //        if (clip.gameObject.name.Contains(vidName)) {
        //            return clip;
        //        }
        //    }
        //    return null;
        return clipsDict[vidName];
    }


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    void Awake() {
        // Set my clipsDict! And hide 'em all.
        clipsDict = new Dictionary<string, VideoClipPlayer>();
        VideoClipPlayer[] myClips = GetComponentsInChildren<VideoClipPlayer>();
        foreach (VideoClipPlayer clip in myClips) {
            clipsDict.Add(clip.gameObject.name.Substring(2), clip); // cut the "v_".
            clip.Initialize(this);
        }
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void PlayVideo(string vidName) {
        // Show me in case I'm hidden in the editor.
        this.gameObject.SetActive(true);

        // Play the requested clip!
        VideoClipPlayer clip = GetClip(vidName);
        if (clip == null) { Debug.LogError("Video clip not found: \"" + vidName + "\""); }
        else { clip.Play(); }


        i_scrim.enabled = true;
    }


    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnVideoFinished() {
        i_scrim.enabled = false;
    }


}
