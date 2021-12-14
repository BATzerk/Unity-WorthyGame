using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.BasketCatchNS {
    public class Basket : BaseViewElement {
        // Components
        [SerializeField] private Image i_beam=null;
        //[SerializeField] private Image i_body=null;
        [SerializeField] private Image i_basketRim=null;
        [SerializeField] private Image i_calloutArrow=null;
        //[SerializeField] private Image i_body=null;
        // Properties
        private float timeWhenCanControl;
        private Vector2 vel;
        public Vector2 posTarget;
        // References
        [SerializeField] private BasketCatch myBasketMinigame=null;
        
        // Getters (Public)
        public bool IsBeamOn { get { return i_beam.gameObject.activeSelf; } }


        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        public void Prep() {
            SetVisible(false);
            SetBeamIsOn(false);
        }
        public void Begin() {
            // Reset visuals
            SetVisible(true);
            i_beam.transform.localEulerAngles = Vector3.zero;
            //i_body.transform.localEulerAngles = Vector3.zero;
            AnchoredPos = new Vector2(0, 1200);
            posTarget = new Vector2(0, 500);
            timeWhenCanControl = Time.time + 1.5f;
        }
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetBeamIsOn(bool val) {
            i_beam.gameObject.SetActive(val);
        }
        

        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Limit posTarget.
            posTarget = new Vector2(Mathf.Clamp(posTarget.x, -180,180), posTarget.y);
            posTarget = new Vector2(posTarget.x, 660); // force y-pos.
            
            // Change/apply vel.
            vel *= 0.92f;
            vel += (posTarget - AnchoredPos) * 0.01f;
            AnchoredPos += vel;
            
            // Update rotation.
            this.transform.localEulerAngles = new Vector3(0, 0, vel.x*0.7f);
            
            // Accept input!
            if (!myBasketMinigame.DidSetOutcome && Time.time > timeWhenCanControl) {
                if (InputController.IsMouseButtonDown()) { OnTouchDown(); }
                if (InputController.IsMouseButtonUp()) { OnTouchUp(); }
                if (InputController.IsMouseButtonHeld()) { OnTouchHeld(); }
            }
            
            // Update poses!
            i_beam.rectTransform.anchoredPosition = AnchoredPos;
            i_calloutArrow.rectTransform.anchoredPosition = AnchoredPos + new Vector2(0, 50);
            // Put basket rim where basket is!
            i_basketRim.rectTransform.anchoredPosition = AnchoredPos;
            i_basketRim.rectTransform.localEulerAngles = this.transform.localEulerAngles;
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