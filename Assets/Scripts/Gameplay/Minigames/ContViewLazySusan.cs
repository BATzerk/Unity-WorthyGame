using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigameNamespace {
    public class ContViewLazySusan : ContestantView {
        // Components
        [SerializeField] private Animator myAnimator=null;
        // Properties
        private bool isSharkTarget;
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnSetSharkTarget(ContestantView cv) {
            isSharkTarget = cv == this;
            if (isSharkTarget) {
                myAnimator.Play("IsTarget");
            }
            else {
                myAnimator.Play("IsNotTarget");
            }
        }
        
    }
}