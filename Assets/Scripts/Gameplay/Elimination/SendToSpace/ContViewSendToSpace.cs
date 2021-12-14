using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ElimigameNS.SendToSpaceNS {
    public class ContViewSendToSpace : BaseViewElement {
        // Components
        [SerializeField] private TextMeshProUGUI t_prioName=null;
        // Properties
        private Vector2 posNeutral;
        // References
        public Contestant MyCont { get; private set; }
        private SendToSpace myElimigame;
        
    
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetMyCont(SendToSpace elimigame, Contestant cont) {
            this.myElimigame = elimigame;
            this.MyCont = cont;
            t_prioName.text = MyCont.myPrio.text;
            posNeutral = Pos; // set posNeutral to where we are when the program starts!
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetInteractable(bool val) {
            GetComponent<UIDraggable>().enabled = val;
        }
        public void AnimateToPosNeutral() { AnimateToPos(posNeutral); }
        public void AnimateToPos(Vector3 _pos) {
            LeanTween.cancel(this.gameObject);
            LeanTween.value(this.gameObject, SetPos, Pos,_pos, 0.3f).setEaseOutQuart();
        }
        
    
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnBeginDrag() {
            this.transform.SetAsLastSibling(); // Put it in FRONT of everyone else
            this.Rotation = 0;
            myElimigame.OnBeginDrag_ContView(this);
        }
        public void OnEndDrag() {
            myElimigame.OnEndDrag_ContView(this);
        }
        

    }
}