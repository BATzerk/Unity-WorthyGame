using MinigameNamespace.UFOAbductionNS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace {
    
    public class UFOAbduction : Minigame {
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Image i_basket=null;
        [SerializeField] private Image i_basketRim=null;
        [SerializeField] private TextMeshProUGUI t_abductedName=null; // the "fake" ContView that we animate for the win.
        [SerializeField] private UFOAbductionShip ship=null;
        private ContViewUFOAbduction[] customContViews; // set in Prep.
        
        // Getters (Public)
        public UFOAbductionShip Ship { get { return ship; } }
        
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            customContViews = GetComponentsInChildren<ContViewUFOAbduction>(true);
            myAnimator.enabled = false;
            
            // Update visuals!
            ship.Prep();
            SetContViewUFOsPhysicsEnabled(false);
        }
        public override void Begin() {
            base.Begin();
            ship.Begin();
            SetContViewUFOsPhysicsEnabled(true);
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetContViewUFOsPhysicsEnabled(bool val) {
            foreach (ContViewUFOAbduction cv in customContViews) { cv.SetPhysicsEnabled(val); }
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnBeamLiftableGetAbducted(BeamLiftable beamLiftable) {
            ContViewUFOAbduction cv = beamLiftable.GetComponent<ContViewUFOAbduction>();
            if (cv != null) {
                SetOutcome(cv.MyContestant);
                ud.AbductedPrio = cv.MyContestant.myPrio;
                ship.OnAbductedCont();
                t_abductedName.text = cv.MyContestant.myPrio.text;
                myAnimator.enabled = true;
                myAnimator.Play("AbductWinner");
                Invoke("OnMinigameComplete", 5f);
            }
        }


        
        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Put basket rim where basket is!
            i_basketRim.rectTransform.anchoredPosition = i_basket.rectTransform.anchoredPosition;
            i_basketRim.rectTransform.localEulerAngles = i_basket.rectTransform.localEulerAngles;
        }



    }
}