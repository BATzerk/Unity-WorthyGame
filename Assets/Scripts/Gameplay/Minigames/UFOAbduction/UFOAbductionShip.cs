using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.UFOAbductionNS {
    public class UFOAbductionShip : BaseViewElement {
        // Components
        [SerializeField] private Image i_beam=null;
        [SerializeField] private Image i_ship=null;
        // Properties
        private bool didAbductCont;
        private float timeWhenCanControl;
        private Vector2 vel;
        private Vector2 posTarget;
        // References
        [SerializeField] private UFOAbduction myUFOMinigame=null;
        
        // Getters (Public)
        public bool IsBeamOn { get { return i_beam.gameObject.activeSelf; } }


        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        public void Prep() {
            didAbductCont = false;
            SetVisible(false);
            SetBeamIsOn(false);
        }
        public void Begin() {
            // Reset visuals
            SetVisible(true);
            i_beam.transform.localEulerAngles = Vector3.zero;
            i_ship.transform.localEulerAngles = Vector3.zero;
            AnchoredPos = new Vector2(0, 2000);
            posTarget = new Vector2(0, 800);
            timeWhenCanControl = Time.time + 1.5f;
        }
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetBeamIsOn(bool val) {
            i_beam.gameObject.SetActive(val);
        }
        
        public void OnAbductedCont() {
            didAbductCont = true;
            vel = Vector2.zero;
            i_beam.GetComponent<Collider2D>().enabled = false; // disable beam collider! Don't pick anyone else up, yo.
            i_ship.transform.localEulerAngles = Vector3.zero;
        }
        

        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (!didAbductCont) {
                // Change/apply vel.
                vel *= 0.86f;
                vel += (posTarget - AnchoredPos) * 0.01f;
                AnchoredPos += vel;
                
                // Update rotation.
                i_ship.transform.localEulerAngles = new Vector3(0, 0, vel.x*0.8f);
                
                // Accept input!
                if (!myUFOMinigame.DidSetOutcome && Time.time > timeWhenCanControl) {
                    if (InputController.IsMouseButtonDown()) { OnTouchDown(); }
                    if (InputController.IsMouseButtonUp()) { OnTouchUp(); }
                    if (InputController.IsMouseButtonHeld()) { OnTouchHeld(); }
                }
            }
        }
        
        // ----------------------------------------------------------------
        //  Input Events
        // ----------------------------------------------------------------
        private void OnTouchDown() {
            //isControllingPos = true;
            SetBeamIsOn(true);
        }
        private void OnTouchUp() {
            //isControllingPos = false;
            SetBeamIsOn(false);
        }
        private void OnTouchHeld() {
            posTarget = InputController.Instance.MousePosCanvas + new Vector2(-MainCanvas.Width*0.5f, 500);
            //posTarget = new Vector2(posTarget.x, Mathf.Min(posTarget.y, 
            
            const float maxY = 770;
            if (IsBeamOn && posTarget.y>maxY) { SetBeamIsOn(false); }
            else if (!IsBeamOn && posTarget.y<maxY) { SetBeamIsOn(true); }
        }
        
        
        
        
    }
}