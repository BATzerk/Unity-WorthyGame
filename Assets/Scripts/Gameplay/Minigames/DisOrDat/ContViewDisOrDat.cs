using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace.DisOrDatNS {
    public class ContViewDisOrDat : ContestantButton {
        // Components
        [SerializeField] private CanvasGroup myCanvasGroup=null;
        [SerializeField] private Image i_specialImage=null; // some rare ones have images, no text. Specified with "img=SpriteName".
        // Properties
        [SerializeField] private bool isOnLeft=false; // either the left or right contestant.
        
        // Getters (Private)
        private Vector3 posOffscreen { get { return new Vector3(500*(isOnLeft?-1:1), myRT.localPosition.y); } }
        private Vector3 posInPosition { get { return new Vector3(140*(isOnLeft?-1:1), myRT.localPosition.y); } }
        
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Begin() {
            base.Begin();
            
            // Cancel tweens.
            LeanTween.cancel(this.gameObject);
            // Set MyContestant values.
            t_prioName.color = Color.black;
            i_specialImage.color = Color.white;
            this.transform.localScale = Vector3.one;
            this.transform.localPosition = posOffscreen;
            // Make not interactable.
            SetInteractable(false);
            
            // Show image OR text.
            string textStr = MyContestant.myPrio.text;
            if (textStr.StartsWith("img=", System.StringComparison.InvariantCulture)) {
                string spriteName = textStr.Substring(4);
                t_prioName.text = "";
                i_specialImage.enabled = true;
                i_specialImage.sprite = Resources.Load<Sprite>("Images/DisOrDat/"+spriteName);
            }
            else {
                i_specialImage.enabled = false;
            }
            
            // Animate in!
            LeanTween.moveLocal(this.gameObject, posInPosition, 0.5f).setEaseOutQuad();
            myCanvasGroup.alpha = 0;
            LeanTween.alphaCanvas(myCanvasGroup, 1, 0.5f).setEaseOutQuad();
        }



        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void AnimateBattleOver(ContestantView winningView) {
            SetInteractable(false); // make not interactable right away.
            LeanTween.cancel(this.gameObject);
            
            // No one was selected?
            if (winningView == null) {
                t_prioName.color = Color.red;
                i_specialImage.color = Color.red;
                LeanTween.alphaCanvas(myCanvasGroup, 0, 0.4f).setDelay(0.3f).setEaseOutQuint();
            }
            // One WAS selected!
            else {
                // I'm the *winner*.
                if (winningView == this) {
                    LeanTween.alphaCanvas(myCanvasGroup, 0, 0.4f).setEaseOutQuint();
                    LeanTween.scale(this.gameObject, Vector3.one*1.3f, 0.4f).setEaseOutQuint();
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