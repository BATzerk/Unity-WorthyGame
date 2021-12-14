using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    [RequireComponent(typeof(Button))]
    public class ContestantButton : ContestantView {
        // Components
        [SerializeField] private Button myButton=null;


        //// ----------------------------------------------------------------
        ////  Initialize
        //// ----------------------------------------------------------------
        //public override void Initialize(Minigame minigame) {
        //    base.Initialize(minigame);
        //    i_timerFill.gameObject.SetActive(myMinigame.IsTimed); // hide i_timerFill if my minigame's not timed.
        //}


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public override void SetInteractable(bool val) {
            myButton.interactable = val;
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnClick() {
            myMinigame.OnClick_ContButton(this);
        }
        
        
    }
}