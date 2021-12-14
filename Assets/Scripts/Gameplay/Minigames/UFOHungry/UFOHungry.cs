using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MinigameNamespace.UFOHungryNS;

namespace MinigameNamespace {
    
    public class UFOHungry : Minigame {
        // Constants
        private const int NumCakesTotal = 3; // (insert pun about tiers here)
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private AbductUI abductUI=null;
        [SerializeField] private CharView cv_alien0=null;
        [SerializeField] private CharView cv_alien1=null;
        [SerializeField] private TextMeshProUGUI t_tapToContinue=null;
        [SerializeField] private Ship ship=null;
        // Prio Abducted
        [SerializeField] private Image i_cakeInShip=null;
        [SerializeField] private Image i_prioArmL=null;
        [SerializeField] private Image i_prioArmR=null;
        [SerializeField] private RectTransform rt_prioAbducted=null;
        [SerializeField] private Sprite s_happyHandIn=null;
        [SerializeField] private Sprite s_happyHandOut=null;
        [SerializeField] private TextMeshProUGUI t_abductedName=null; // the "fake" ContView that we animate for the win.
        // Properties
        private bool mayClickToNextStep;
        private int numCakesAbducted;
        private int currStep;
        
        // Getters (Public)
        public Ship Ship { get { return ship; } }
        
        
        // ----------------------------------------------------------------
        //  Awake / Destroy
        // ----------------------------------------------------------------
        override protected void Awake() {
            base.Awake();
            // Add event listeners!
            em.CharFinishedRevealingSpeechTextEvent += OnCharFinishedRevealingSpeechText;
        }
        private void OnDestroy() {
            // Remove event listeners!
            em.CharFinishedRevealingSpeechTextEvent -= OnCharFinishedRevealingSpeechText;
        }
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            SetMayClickToNextStep(false);
            
            // Update visuals!
            i_prioArmL.enabled = false;
            i_prioArmR.enabled = false;
            myAnimator.Play("EstablishingShot", -1, 0);
            //SetLiftablePhysicsEnabled(false);
            myAnimator.speed = 0;
            t_abductedName.text = ud.AbductedPrio.text;
        }
        public override void Begin() {
            base.Begin();
            myAnimator.enabled = true;
            currStep = 0;
            numCakesAbducted = 0;
            abductUI.OnChangeNumFoodAbducted(numCakesAbducted, NumCakesTotal);
            
            NextStep();
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void NextStep() {
            SetMayClickToNextStep(false);
            cv_alien0.SetSpeechText("");
            cv_alien1.SetSpeechText("");
            
            currStep ++;
            
            int index=0;
            if (false) {}
            if (currStep == ++index) {
                myAnimator.speed = 1;
                myAnimator.Play("EstablishingShot");
            }
            else if (currStep == ++index) {
                myAnimator.enabled = false;
                cv_alien1.SetSpeechText("Tuurk, " + ud.AbductedPrio.NameStyled + " is getting hungry.");
            }
            else if (currStep == ++index) {
                cv_alien1.SetSpeechText("I see cake on the ground below.");
            }
            else if (currStep == ++index) {
                cv_alien1.SetSpeechText("Should we abduct the cake, for " + ud.AbductedPrio.NameStyled + "?");
            }
            else if (currStep == ++index) {
                cv_alien0.SetSpeechText("Daaaa, sure, Tsyché.\n\nSure.");
            }
            else if (currStep == ++index) {
                myAnimator.enabled = true;
                myAnimator.speed = 1;
                myAnimator.Play("ShowWedding");
            }
            else if (currStep == ++index) {
                SetLiftableOutlinesEnabled(true);
                myAnimator.enabled = false;
                abductUI.SetVisible(true);
                ship.Begin();
            }
            else if (currStep == ++index) {
                i_cakeInShip.enabled = true;
                myAnimator.enabled = true;
                myAnimator.Play("AbductedAllCake");
                StartCoroutine(Anim_PrioHappyHands());
                //TO DO: Add all the stuff we abducted inside spaceship?
            }
            //else if (currStep == ++index) {
            //    SetMayClickToNextStep(true); // wait for a tap before first speech.
            //}
            else if (currStep == ++index) {
                cv_alien1.SetSpeechText("Tuurk, " + ud.AbductedPrio.NameStyled + " is happy.");
            }
            else if (currStep == ++index) {
                cv_alien0.SetSpeechText("Yes, Tsyché.\n\nYes it is.");
            }
            else if (currStep == ++index) {
                myAnimator.enabled = true;
                myAnimator.Play("Ending");
            }
            else if (currStep == ++index) {
                OnMinigameComplete();
            }
        }
        
        public void OnBeamLiftableGetAbducted(BeamLiftable beamLiftable) {
            if (beamLiftable.IsCake) {
                numCakesAbducted ++;
                abductUI.OnChangeNumFoodAbducted(numCakesAbducted, NumCakesTotal);
                if (numCakesAbducted >= NumCakesTotal) {
                    OnAbductedAllCake();
                }
            }
            //ContViewUFOAbduction cv = beamLiftable.GetComponent<ContViewUFOAbduction>();
            //if (cv != null) {
            //    t_abductedName.text = cv.MyContestant.myPrio.text;
            //    myAnimator.enabled = true;
            //    Invoke("OnMinigameComplete", 7f);
            //}
        }
        

        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetMayClickToNextStep(bool val) {
            mayClickToNextStep = val;
            t_tapToContinue.enabled = mayClickToNextStep;
        }
        private void SetLiftableOutlinesEnabled(bool val) {
            foreach (BeamLiftable bl in GetComponentsInChildren<BeamLiftable>()) {
                bl.SetOutlineEnabled(val);
            }
        }
        //private void SetLiftablePhysicsEnabled(bool val) {
        //    foreach (BeamLiftable bl in GetComponentsInChildren<BeamLiftable>()) {
        //        Rigidbody2D rb = bl.GetComponent<Rigidbody2D>();
        //        rb.bodyType = val ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        //        rb.Sleep(); // sleep, yo!
        //    }
        //}
        private IEnumerator Anim_PrioHappyHands() {
            rt_prioAbducted.localEulerAngles = new Vector3(0, 0, 6);
            i_prioArmL.enabled = true;
            i_prioArmR.enabled = true;
            while (true) {
                i_prioArmL.sprite = s_happyHandIn;
                i_prioArmR.sprite = s_happyHandOut;
                yield return new WaitForSeconds(0.35f);
                i_prioArmL.sprite = s_happyHandOut;
                i_prioArmR.sprite = s_happyHandIn;
                yield return new WaitForSeconds(0.35f);
                rt_prioAbducted.localEulerAngles = new Vector3(0, 0, rt_prioAbducted.localEulerAngles.z*-1);
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnCharFinishedRevealingSpeechText() {
            if (!isActiveAndEnabled) { return; } // Safety check.
            SetMayClickToNextStep(true);
        }
        private void OnAbductedAllCake() {
            ship.OnAbductedAllCake();
            NextStep();
        }
        

        
        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (mayClickToNextStep && Input.GetMouseButtonDown(0)) {
                NextStep();
            }
        }



    }
}