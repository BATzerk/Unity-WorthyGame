using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace MinigameNamespace {

    public class KnifeToMurderContView : ContestantView {
        // Components
        [SerializeField] private Animator myAnimator=null;
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        override public void Prep(params Contestant[] contestants) {
            base.Prep(contestants);
            PlayAnim("Hello");
            GameUtils.SetUIGraphicAlpha(t_prioName, 0.2f);
        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        //public override void SetInteractable(bool val) { }

        public void PlayAnim_HandOpenClose() {
            GameUtils.SetUIGraphicAlpha(t_prioName, 1);
            PlayAnim("HandOpenClose");
        }
        public void PlayAnim(string clipName) { myAnimator.Play(clipName); }
        public void StopAnimations() { myAnimator.StopPlayback(); }
        
        
        public void SetVisualsAsMurderer() {
            myAnimator.Play("Murderer");
        }
        public void SetVisualsAsVictim() {
            myAnimator.Play("Victim");
        }
        
        
        
    }
}