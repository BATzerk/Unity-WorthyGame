using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ElimigameNS.TestBasicNS {
    public class ContViewTestBasic : BaseViewElement {
        // Components
        //[SerializeField] private Button myButton=null;
        [SerializeField] private Image i_highlight=null; // For when I'm interactable! Oscillates.
        [SerializeField] private TextMeshProUGUI t_prioName=null;
        // References
        public Contestant MyCont { get; private set; }
        private TestBasic myElimigame;
        
    
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetMyCont(TestBasic myElimigame, Contestant cont) {
            this.myElimigame = myElimigame;
            this.MyCont = cont;
            t_prioName.text = MyCont.myPrio.text;
        }
        //public void SetIsInteractable(bool val) {
        //    myButton.interactable = val;
        //    i_highlight.enabled = val;
        //}
        
    
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnClicked() {
            myElimigame.OnClick_ContView(this);
        }


        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (i_highlight.enabled) {
                float alpha = MathUtils.SinRange(0f, 0.6f, Time.time*6f+transform.localPosition.x*0.1f);
                GameUtils.SetUIGraphicAlpha(i_highlight, alpha);
            }
        }

    }
}