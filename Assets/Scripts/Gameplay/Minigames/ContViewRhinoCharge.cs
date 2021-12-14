using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigameNamespace {
    public class ContViewRhinoCharge : ContestantView {
        // Components
        [SerializeField] private Animator myAnimator=null;
        // Properties
        private bool isRhinoTarget;
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        //public void OnSetRhinoTarget(ContestantView cv) {
        //    isRhinoTarget = cv == this;
        //    if (isRhinoTarget) {
        //        myAnimator.Play("IsTarget");
        //    }
        //    else {
        //        myAnimator.Play("IsNotTarget");
        //    }
        //}
        
        public void TweenToPos(Vector2 _pos) {
            LeanTween.value(this.gameObject, SetAnchoredPos, AnchoredPos,_pos, 0.3f).setEaseOutQuart();
        }
        public void PlayAnim(string animName) {
            myAnimator.Play(animName);
        }
        
    }
}