using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace.KingOfHillNS {
    public class ContViewKingOfHill : ContestantButton {
        // Components
        [SerializeField] private CanvasGroup myCanvasGroup=null;
        // Properties
        [SerializeField] private bool isOnLeft=false; // either the left or right contestant.
        private bool didJustWin=false;
        
        // Getters (Private)
        private Vector3 posOffscreen { get { return new Vector3(500*(isOnLeft?-1:1), myRT.localPosition.y); } }
        private Vector3 posInPosition { get { return new Vector3(140*(isOnLeft?-1:1), myRT.localPosition.y); } }
        
        
        
        // ----------------------------------------------------------------
        //  Init
        // ----------------------------------------------------------------
        override public void Begin() {
            base.Begin();
            
            if (didJustWin) { AnimateInWinner(); }
            else { AnimateInNewcomer(); }
        }
        
        private void AnimateInWinner() {
            // Make not interactable.
            SetInteractable(false);
        }
        private void AnimateInNewcomer() {
            // Cancel tweens.
            LeanTween.cancel(this.gameObject);
            // Set MyContestant values.
            t_prioName.color = Color.black;
            this.transform.localScale = Vector3.one;
            this.transform.localPosition = posOffscreen;
            // Make not interactable.
            SetInteractable(false);
            // Animate in!
            LeanTween.moveLocal(this.gameObject, posInPosition, 0.5f).setEaseOutQuad();
            myCanvasGroup.alpha = 0;
            LeanTween.alphaCanvas(myCanvasGroup, 1, 0.5f).setEaseOutQuad();
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void AnimateBattleOver(ContestantView winnerView) {
            SetInteractable(false); // make not interactable right away.
            LeanTween.cancel(this.gameObject);
            didJustWin = winnerView == this;
            
            // No one was selected?
            if (winnerView == null) {
                t_prioName.color = Color.red;
                LeanTween.alphaCanvas(myCanvasGroup, 0, 0.4f).setDelay(0.3f).setEaseOutQuint();
            }
            // One WAS selected!
            else {
                // I'm the *winner*.
                if (didJustWin) {
                    LeanTween.scale(this.gameObject, Vector3.one*1.3f, 0.2f).setEaseOutQuint();
                    LeanTween.scale(this.gameObject, Vector3.one, 0.4f).setDelay(0.2f).setEaseOutQuint();
                }
                // I'm the *loser*.
                else {
                    LeanTween.alphaCanvas(myCanvasGroup, 0, 0.2f).setEaseOutQuint();
                    LeanTween.scale(this.gameObject, Vector3.one*0.88f, 0.2f).setEaseOutQuint();
                }
            }
        }
        
        
        
    }
}