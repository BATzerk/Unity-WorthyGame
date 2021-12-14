using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ElimigameNS.PettingZooNS {
    [RequireComponent(typeof(UIDraggable))]
    public class ContViewPettingZoo : BaseViewElement {
        // Components
        [SerializeField] private TextMeshProUGUI t_prioName=null;
        // References
        public Contestant MyCont { get; private set; }
        private PettingZoo myElimigame;
        
    
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetMyCont(PettingZoo elimigame, Contestant cont) {
            this.myElimigame = elimigame;
            this.MyCont = cont;
            t_prioName.text = MyCont.myPrio.text;
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetInteractable(bool val) {
            GetComponent<UIDraggable>().enabled = val;
        }
    
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnBeginDrag() {
            this.transform.SetAsLastSibling(); // Put it in FRONT of everyone else
            this.Rotation = 0;
            //myMinigame.OnBeginDrag_ContDraggable(this);
        }
        public void OnEndDrag() {
            myElimigame.OnEndDrag_ContView(this);
        }

    }
}