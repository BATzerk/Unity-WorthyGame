using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class LazySusan : Minigame {
        // Overrides
        //public override int NumContestants() { return 5; }
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private GameObject go_shark=null;
        [SerializeField] private RectTransform rt_disc=null;
        [SerializeField] private TextMeshProUGUI t_header=null;
        [SerializeField] private TextMeshProUGUI t_contLoserName=null;
        private ContViewLazySusan customContViewA; // set in Prep.
        private ContViewLazySusan customContViewB; // set in Prep.
        // Properties
        private bool didSharkBeginDescent;
        private float pmouseAngle;
        private float discRotationVel;
        // References
        private ContestantView cv_sharkTarget;
        
        // Getters (Private)
        private float DiscRotation {
            get { return rt_disc.localEulerAngles.z; }
            set { rt_disc.localEulerAngles = new Vector3(0,0,value); }
        }
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            customContViewA = ContViews[0] as ContViewLazySusan;
            customContViewB = ContViews[1] as ContViewLazySusan;
            
            base.Prep(contestants);
            
            // Update visuals!
            didSharkBeginDescent = false;
            t_header.text = "PROTECT!";//Please FLICK OFF the LESS IMPORTANT one.";
            go_shark.SetActive(false);
        }
        public override void Begin() {
            base.Begin();
            go_shark.SetActive(true);
            myAnimator.Play("SharkDescent");
        }
        
        
        //private IEnumerator Coroutine_SharkAttack() {
        //    rt_shark.gameObject.SetActive(true);
        //    rt_shark.anchoredPosition = new Vector2(0, 1000);
            
        //    for (
        //}


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnSharkBeginDescent() {
            didSharkBeginDescent = true;
        }
        public void OnSharkReachTarget() {
            t_header.text = "";
            SetOutcomeLoser(cv_sharkTarget.MyContestant);
            cv_sharkTarget.SetVisible(false);
            t_contLoserName.text = cv_sharkTarget.MyContestant.myPrio.text;
            myAnimator.Play("SharkEatLoser");
            Invoke("OnMinigameComplete", 5.5f);
        }


        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (CurrState == States.Begun) {
                UpdateDiscRotation();
                if (didSharkBeginDescent) {
                    UpdateSharkTarget();
                }
            }
        }
        
        
        private void UpdateDiscRotation() {
            // Disc!
            Vector2 mousePos = InputController.Instance.MousePosCanvas;
            float mouseAngle = Mathf.Atan2(mousePos.y-rt_disc.anchoredPosition.y, mousePos.x-rt_disc.anchoredPosition.x);
            if (Mathf.Abs(mouseAngle-pmouseAngle) >= Mathf.PI) {
                mouseAngle -= Mathf.PI*2;
            }
            if (InputController.IsMouseButtonDown()) {
                pmouseAngle = mouseAngle; // reset pmouseAngle when we tap down.
            }
            if (InputController.IsMouseButtonHeld()) {
                discRotationVel += (mouseAngle-pmouseAngle) * 12f;
            }
            DiscRotation += discRotationVel;
            discRotationVel *= 0.86f;
            pmouseAngle = mouseAngle;
            // ContViews! Offset so they're always horizontal.
            customContViewA.Rotation = -DiscRotation;
            customContViewB.Rotation = -DiscRotation;
        }
        private void UpdateSharkTarget() {
            // Who's for dinner??
            ContestantView _target = DiscRotation>180 ? customContViewA : customContViewB;
            if (cv_sharkTarget != _target) {
                cv_sharkTarget = _target;
                customContViewA.OnSetSharkTarget(cv_sharkTarget);
                customContViewB.OnSetSharkTarget(cv_sharkTarget);
            }
            // Update shark x pos!
            Vector3 sharkPosTarget = new Vector3(cv_sharkTarget.transform.position.x, go_shark.transform.position.y);
            go_shark.transform.position += (sharkPosTarget-go_shark.transform.position) * 0.2f;
        }
    }
}