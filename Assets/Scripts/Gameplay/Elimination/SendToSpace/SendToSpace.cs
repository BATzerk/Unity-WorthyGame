using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using ElimigameNS.SendToSpaceNS;

namespace ElimigameNS {
    public class SendToSpace : Elimigame {
        // Constants
        override protected int NumConts { get { return 5; } }
        // Components
        [SerializeField] private Image i_slotA=null; // a helmet!
        [SerializeField] private Image i_slotB=null; // a helmet!
        private ContViewSendToSpace[] contViews; // Set in Open.
        // References
        private ContViewSendToSpace astroA;
        private ContViewSendToSpace astroB;
        
        
        // ----------------------------------------------------------------
        //  Open
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            
            contViews = this.GetComponentsInChildren<ContViewSendToSpace>(true);
            
            // Prep visuals!
            for (int i=0; i<NumConts; i++) {
                contViews[i].SetMyCont(this, conts[i]);
            }
            SetContViewsVisible(false);
            
            NullifyAstroA();
            NullifyAstroB();
            OnChangedAstros();
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public bool DoFuncFromStory(string line) {
            if (!base.DoFuncFromStory(line)) {
                if (false) {}
                //else if (line.Contains("HideContViews")) { HideContViews(); return true; }
                else if (line.Contains("RevealContViews")) { RevealContViews(); return true; }
                else {
                    Debug.LogError("Func not recognized: " + line);
                    return false;
                }
            }
            return false;
        }
        
    
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetContViewsInteractable(bool val) {
            foreach (ContViewSendToSpace cv in contViews) { cv.SetInteractable(val); }
        }
        private void SetContViewsVisible(bool val) {
            foreach (ContViewSendToSpace cv in contViews) { cv.SetVisible(val); }
        }
        private void RevealContViews() {
            //SetContViewsInteractable(true);
            SetContViewsVisible(true);
            myAnimator.Play("RevealContViews");
        }
        
        
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnChangedAstros() {
            // Update if we've got ourselves a little date-aroonie!
            if (astroA != null && astroB != null) {
                BeginLaunchSeq();
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetAstroA(ContViewSendToSpace cont) {
            if (astroA != null && astroA!=cont) { // We're usurping another Draggable??
                astroA.AnimateToPosNeutral();
                astroA.Rotation = 0;
            }
            astroA = cont;
            astroA.AnimateToPos(i_slotA.transform.localPosition);
            //astroA.Rotation = 80;
            i_slotA.gameObject.SetActive(false);
            OnChangedAstros();
        }
        private void SetAstroB(ContViewSendToSpace cont) {
            if (astroB != null && astroB!=cont) { // We're usurping another Draggable??
                astroB.AnimateToPosNeutral();
                astroB.Rotation = 0;
            }
            astroB = cont;
            astroB.AnimateToPos(i_slotB.transform.localPosition);
            //astroB.Rotation = -80;
            i_slotB.gameObject.SetActive(false);
            OnChangedAstros();
        }
        
        
        private void NullifyAstroA() {
            astroA = null;
            i_slotA.gameObject.SetActive(true);
        }
        private void NullifyAstroB() {
            astroB = null;
            i_slotB.gameObject.SetActive(true);
        }
        
        
        private void BeginLaunchSeq() {
            // Eliminate the two new astronauts!
            ud.EliminateUserPrio(astroA.MyCont.myPrio);
            ud.EliminateUserPrio(astroB.MyCont.myPrio);
            // Hide ones that WEREN'T picked.
            foreach (ContViewSendToSpace cv in contViews) {
                if (cv!=astroA && cv!=astroB) { cv.SetVisible(false); }
            }
            // Parent the contViews inside of the helmets TO DO: Finish this part.
            
            // Animate!
            myAnimator.Play("ChoseAstros");
        }
        
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnBeginDrag_ContView(ContViewSendToSpace cv) {
            // This is astroA or astroB? Null the ref out.
            if (cv == astroA) { NullifyAstroA(); }
            if (cv == astroB) { NullifyAstroB(); }
        }
        public void OnEndDrag_ContView(ContViewSendToSpace cv) {
            // Dragged into SlotA?
            if (Vector3.Distance(cv.transform.localPosition,i_slotA.transform.localPosition) < 150f) {
                SetAstroA(cv);
            }
            else if (Vector3.Distance(cv.transform.localPosition,i_slotB.transform.localPosition) < 150f) {
                SetAstroB(cv);
            }
            else {
                cv.AnimateToPosNeutral();
            }
        }



    }
}