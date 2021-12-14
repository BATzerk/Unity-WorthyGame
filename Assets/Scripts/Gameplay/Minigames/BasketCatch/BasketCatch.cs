using MinigameNamespace.BasketCatchNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class BasketCatch : Minigame {
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Basket basket=null;
        [SerializeField] private GameObject go_topUI=null;
        [SerializeField] private ParticleSystem ps_winnerShower=null;
        private ContViewBasketCatch[] customContViews; // set in Prep.
        
        // Getters (Public)
        public Basket Basket { get { return basket; } }
        
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            customContViews = GetComponentsInChildren<ContViewBasketCatch>(true);
            myAnimator.enabled = false;
            GameUtils.SetParticleSystemEmissionEnabled(ps_winnerShower, false);
            
            // Update visuals!
            basket.Prep();
            SetContViewsPhysicsEnabled(false);
        }
        public override void Begin() {
            base.Begin();
            basket.Begin();
            SetContViewsPhysicsEnabled(true);
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetContViewsPhysicsEnabled(bool val) {
            foreach (ContViewBasketCatch cv in customContViews) { cv.SetPhysicsEnabled(val); }
        }
        
        override public void Hide() {
            base.Hide();
            GameUtils.SetParticleSystemEmissionEnabled(ps_winnerShower, false);
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnBeamLiftableGetCaught(BeamLiftable beamLiftable) {
            ContViewBasketCatch cv = beamLiftable.GetComponent<ContViewBasketCatch>();
            if (cv != null) {
                SetOutcome(cv.MyContestant);
                basket.posTarget = new Vector2(0, basket.posTarget.y); // put basket in center.
                GameUtils.SetParticleSystemEmissionEnabled(ps_winnerShower, true);
                go_topUI.SetActive(false);
                //cv.gameObject.name = "Winner";
                //myAnimator.enabled = true;
                //myAnimator.Play("AbductWinner");
                Invoke("OnMinigameComplete", 2f);
            }
        }
        





    }
}