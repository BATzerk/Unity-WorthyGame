using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_BubbleBath : Minigame {
        // Components
        [SerializeField] private Animator myAnimator;
        [SerializeField] private BubbleBath_Hose hose;


        // Getters
        public override string MyWorthyNoun { get { return "Self-Love Worthy"; } }



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();


        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void PauseAnimatior() {
            myAnimator.enabled = false;
        }
        private void UnpauseAnimator() {
            myAnimator.enabled = true;
        }



        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            //HideNextButton();
            gameController.AdvanceSeqStep();
        }



        // ----------------------------------------------------------------
        //  Steps
        // ----------------------------------------------------------------
        override protected void SetCurrStep(int _currStep) {
            base.SetCurrStep(_currStep);
            switch (CurrStep) {
                //// Zoom out to show the room.
                //case 1:
                //    UnpauseAnimator();
                //    break;
                //// Zoom in and allow painting #1!
                //case 2:
                //    mayPaint = true;
                //    go_ui.SetActive(true);
                //    UnpauseAnimator();
                //    break;
                //// Zoom out from Painting #1.
                //case 3:
                //    UnpauseAnimator();
                //    break;
                //// Zoom in and allow painting #2!
                //case 4:
                //    SetCurrMasterpieceIndex(currMasterpieceIndex + 1);
                //    mayPaint = true;
                //    go_ui.SetActive(true);
                //    UnpauseAnimator();
                //    break;
                //// Zoom out from Painting #2.
                //case 5:
                //    UnpauseAnimator();
                //    break;
            }
        }




    }
}





