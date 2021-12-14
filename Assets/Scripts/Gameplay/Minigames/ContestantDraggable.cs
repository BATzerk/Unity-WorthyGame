using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    [RequireComponent(typeof(UIDraggable))]
    public class ContestantDraggable : ContestantView {
        // Components
        //[SerializeField] private Button myButton=null;
        // Properties
        private Vector2 posNeutral;
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        override public void SetMyMinigame(Minigame minigame) {
            base.SetMyMinigame(minigame);
            posNeutral = Pos; // set posNeutral to where we are when the program starts!
        }
        override public void Prep(params Contestant[] contestants) {
            base.Prep(contestants);
            Pos = posNeutral; // start at neutral pos.
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        override public void SetInteractable(bool val) {
            GetComponent<UIDraggable>().enabled = val;
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        //public void GoToPosNeutral() { Pos = posNeutral; }
        public void AnimateToPosNeutral() { AnimateToPos(posNeutral); }
        public void AnimateToPos(Vector3 _pos) {
            LeanTween.cancel(this.gameObject);
            LeanTween.value(this.gameObject, SetPos, Pos,_pos, 0.3f).setEaseOutQuart();
        }
        //public void OnClick() {
        //    myMinigame.OnClickContestantView(this);
        //}
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnBeginDrag() {
            if (myMinigame.IsComplete) { return; } // Minigame over? Ignore drag.
            this.transform.SetAsLastSibling(); // Put it in FRONT of everyone else
            this.Rotation = 0;
            myMinigame.OnBeginDrag_ContDraggable(this);
        }
        public void OnEndDrag() {
            if (myMinigame.IsComplete) { return; } // Minigame over? Ignore drag.
            myMinigame.OnEndDrag_ContDraggable(this);
        }
        
        
    }
}