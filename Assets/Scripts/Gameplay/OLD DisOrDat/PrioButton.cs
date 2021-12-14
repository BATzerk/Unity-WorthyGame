//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//namespace DisOrDatNamespace {
//    public class PrioButton : BaseViewElement {
//        // Components
//        [SerializeField] private Button myButton=null;
//        [SerializeField] private CanvasGroup myCanvasGroup=null;
//        [SerializeField] private TextMeshProUGUI t_prioName=null;
//        // Properties
//        [SerializeField] private bool isOnLeft=false; // either the left or right contestant.
//        // References
//        [SerializeField] private DisOrDatController dodCont=null;
//        public Contestant MyContestant { get; private set; }
        
//        // Getters (Private)
//        private Vector3 posOffscreen { get { return new Vector3(500*(isOnLeft?-1:1), myRT.localPosition.y); } }
//        private Vector3 posInPosition { get { return new Vector3(140*(isOnLeft?-1:1), myRT.localPosition.y); } }
        
        
        
//        // ----------------------------------------------------------------
//        //  Doers
//        // ----------------------------------------------------------------
//        public void SetInteractable(bool val) {
//            myButton.interactable = val;
//        }
//        //private SetMyCanvasGroupAlpha(float val) {
//        //    myCanvasGroup.alpha = val;
//        //}
        
//        public void ResetAndAnimateIn(Contestant contestant) {
//            // Cancel tweens.
//            LeanTween.cancel(this.gameObject);
//            // Set MyContestant values.
//            this.MyContestant = contestant;
//            t_prioName.color = Color.black;
//            t_prioName.text = MyContestant.myPrio.text;
//            this.transform.localScale = Vector3.one;
//            this.transform.localPosition = posOffscreen;
//            // Make not interactable.
//            SetInteractable(false);
//            // Animate in!
//            LeanTween.moveLocal(this.gameObject, posInPosition, 0.5f).setEaseOutQuad();
//            myCanvasGroup.alpha = 0;
//            LeanTween.alphaCanvas(myCanvasGroup, 1, 0.5f).setEaseOutQuad();
//        }
        
//        public void AnimateBattleOver(PrioButton winningButton) {
//            SetInteractable(false); // make not interactable right away.
//            LeanTween.cancel(this.gameObject);
            
//            // No one was selected?
//            if (winningButton == null) {
//                t_prioName.color = Color.red;
//                LeanTween.alphaCanvas(myCanvasGroup, 0, 0.4f).setDelay(0.3f).setEaseOutQuint();
//            }
//            // One WAS selected!
//            else {
//                // I'm the *winner*.
//                if (winningButton == this) {
//                    LeanTween.alphaCanvas(myCanvasGroup, 0, 0.4f).setEaseOutQuint();
//                    LeanTween.scale(this.gameObject, Vector3.one*1.3f, 0.4f).setEaseOutQuint();
//                }
//                // I'm the *loser*.
//                else {
//                    LeanTween.alphaCanvas(myCanvasGroup, 0, 0.2f).setEaseOutQuint();
//                    LeanTween.scale(this.gameObject, Vector3.one*0.88f, 0.2f).setEaseOutQuint();
//                }
//            }
//        }
        
        
//        // ----------------------------------------------------------------
//        //  Button Events
//        // ----------------------------------------------------------------
//        public void OnClickMe() {
//            dodCont.OnChoosePrioButton(this);
//        }
        
        
        
//    }
//}