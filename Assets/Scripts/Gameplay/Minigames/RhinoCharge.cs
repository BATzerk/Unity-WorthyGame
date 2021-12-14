using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class RhinoCharge : Minigame {
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Button b_swap=null;
        //[SerializeField] private GameObject go_rideables=null;
        private ContViewRhinoCharge contView0; // set in Prep.
        private ContViewRhinoCharge contView1; // set in Prep.
        // Properties
        private Vector2 contPosSafe = new Vector2(430, 260);
        private Vector2 contPosInDanger = new Vector2(430, -30);
        // References
        private ContViewRhinoCharge cv_inDanger;
        private ContViewRhinoCharge cv_safe;
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            contView0 = ContViews[0] as ContViewRhinoCharge;
            contView1 = ContViews[1] as ContViewRhinoCharge;
            base.Prep(contestants);
            myAnimator.Play("PreIntro");
            SetCVInDanger(contView0); // default someone to being in danger, yo.
        }
        public override void Begin() {
            base.Begin();
            // Play intro anim!
            myAnimator.Play("Intro");
        }
        
        
        //private IEnumerator Coroutine_SharkAttack() {
        //    rt_shark.gameObject.SetActive(true);
        //    rt_shark.anchoredPosition = new Vector2(0, 1000);
            
        //    for (
        //}
        private ContViewRhinoCharge OtherCV(ContViewRhinoCharge thisCV) {
            return thisCV==contView0 ? contView1 : contView0;
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnAnimDone_Intro() {
            myAnimator.Play("RhinoCharge");
        }
        public void OnRhinoReachTarget() {
            SetOutcomeLoser(cv_inDanger.MyContestant);
            cv_inDanger.PlayAnim("Pop");
            b_swap.gameObject.SetActive(false);
            myAnimator.Play("Outro");
            Invoke("OnMinigameComplete", 3.2f);
        }
        
        public void OnClick_Swap() {
            if (DidSetOutcome) { return; } // Safety check.
            SetCVInDanger(contView0==cv_inDanger ? contView1 : contView0);
        }
        private void SetCVInDanger(ContViewRhinoCharge cv) {
            cv_inDanger = cv;
            cv_safe = OtherCV(cv);
            // Tween to positions!
            cv_safe.TweenToPos(contPosSafe);
            cv_inDanger.TweenToPos(contPosInDanger);
        }


        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        //private void UpdateSharkTarget() {
        //    // Who's for dinner??
        //    ContestantView _target = DiscRotation>180 ? contView0 : contView1;
        //    if (cv_inDanger != _target) {
        //        cv_inDanger = _target;
        //        contView0.OnSetSharkTarget(cv_inDanger);
        //        contView1.OnSetSharkTarget(cv_inDanger);
        //    }
        //    // Update shark x pos!
        //    Vector3 sharkPosTarget = new Vector3(cv_inDanger.transform.position.x, go_shark.transform.position.y);
        //    go_shark.transform.position += (sharkPosTarget-go_shark.transform.position) * 0.2f;
        //}
        
    }
}