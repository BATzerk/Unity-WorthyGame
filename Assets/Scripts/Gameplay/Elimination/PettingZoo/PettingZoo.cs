using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using ElimigameNS.PettingZooNS;

namespace ElimigameNS {
    public class PettingZoo : Elimigame {
        // Constants
        override protected int NumConts { get { return 5; } }
        // Components
        [SerializeField] private TextMeshProUGUI t_contToEat=null; // the fake one we animate being eaten.
        private ContViewPettingZoo[] contViews; // Set in Open.
        private List<ContViewPettingZoo> contViewsLeft;
        // References
        private List<Contestant> contsEliminated; // those that HAVE been chosen.
        private List<Contestant> contsLeft; // those left to be chosen.
        
        
        // ----------------------------------------------------------------
        //  Open
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            
            contViews = this.GetComponentsInChildren<ContViewPettingZoo>(true);
            contViewsLeft = new List<ContViewPettingZoo>(contViews);
            
            // Make contestants!
            contsEliminated = new List<Contestant>();
            contsLeft = new List<Contestant>(conts);
            
            // Prep visuals!
            for (int i=0; i<NumConts; i++) {
                contViews[i].SetMyCont(this, conts[i]);
            }
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public bool DoFuncFromStory(string line) {
            if (!base.DoFuncFromStory(line)) {
                if (line.Contains("HideContViews")) { HideContViews(); return true; }
                else if (line.Contains("ShowContViews")) { ShowContViews(); return true; }
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
            foreach (ContViewPettingZoo cv in contViewsLeft) { cv.SetInteractable(val); }
        }
        private void ShowContViews() {
            SetContViewsInteractable(true);
            foreach (ContViewPettingZoo cv in contViewsLeft) { cv.SetVisible(true); }
            myAnimator.Play("HungryGoatMouth"); // AAAAAH
        }
        private void HideContViews() {
            foreach (ContViewPettingZoo cv in contViewsLeft) { cv.SetVisible(false); }
        }
        
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnEndDrag_ContView(ContViewPettingZoo cv) {
            if (cv.AnchoredPos.y > 350) {
                FeedContToGoat(cv);
            }
        }
        private void FeedContToGoat(ContViewPettingZoo cv) {
            contViewsLeft.Remove(cv);
            Contestant cont = cv.MyCont;
            contsEliminated.Add(cont);
            contsLeft.Remove(cont);
            //if (contsEliminated.Count >= 2) { OnChoseFinalCont(); }
            ud.EliminateUserPrio(cont.myPrio);
            
            SetContViewsInteractable(false);
            cv.SetVisible(false);
            t_contToEat.text = cont.myPrio.text;
            
            myAnimator.Play("GoatEatCont");
        }



    }
}