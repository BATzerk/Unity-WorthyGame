using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using ElimigameNS.TestBasicNS;

namespace ElimigameNS {
    public class TestBasic : Elimigame {
        // Constants
        override protected int NumConts { get { return 5; } }
        // Components
        [SerializeField] private TextMeshProUGUI t_instructions=null;
        private ContViewTestBasic[] contViews; // Set in Open.
        // References
        private List<Contestant> contsChosen; // those that HAVE been chosen.
        private List<Contestant> contsUnchosen; // those left to be chosen.
        
        // Getters (Public)
        override public string FillInBlanks(string str) {
            if (!str.Contains("[")) { return str; } // No replacement chars? Return string as it is!
            str = str.Replace("[ContUnchosen0]", ContUnchosen0);
            str = str.Replace("[ContUnchosen1]", ContUnchosen1);
            return str;
        }
        // Getters (Private)
        private string ContUnchosen0 { get { return contsUnchosen.Count<=0 ? "undefined" : contsUnchosen[0].myPrio.NameStyled; } }
        private string ContUnchosen1 { get { return contsUnchosen.Count<=1 ? "undefined" : contsUnchosen[1].myPrio.NameStyled; } }
        
        
        
        // ----------------------------------------------------------------
        //  Open
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            
            contViews = this.GetComponentsInChildren<ContViewTestBasic>(true);
            
            // Make contestants!
            contsChosen = new List<Contestant>();
            contsUnchosen = new List<Contestant>(conts);
            
            // Prep visuals!
            for (int i=0; i<NumConts; i++) {
                contViews[i].SetMyCont(this, conts[i]);
                contViews[i].SetVisible(true);
            }
            t_instructions.text = "choose THREE priorities to KEEP.";
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnChoseFinalCont() {
            // Remove losers from userPrios!!
            for (int i=0; i<contsUnchosen.Count; i++) {
                ud.EliminateUserPrio(contsUnchosen[i].myPrio);
            }
            t_instructions.text = ContUnchosen0 + " and " + ContUnchosen1 + " are removed from the game.";
            // Hacky testing flow.
            myEGCont.GameController.ManuallyShowNextButton("done");
            //OnComplete();
        }
        
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnClick_ContView(ContViewTestBasic cv) {
            cv.SetVisible(false); // just hide the fella.
            Contestant cont = cv.MyCont;
            contsChosen.Add(cont);
            contsUnchosen.Remove(cont);
            if (contsChosen.Count == 3) { OnChoseFinalCont(); }
        }



    }
}