using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using ElimigameNS.TheBachelorNS;

namespace ElimigameNS {
    public class TheBachelor : Elimigame {
        // Constants
        override protected int NumConts { get { return 5; } }
        // Components
        [SerializeField] private TextMeshProUGUI t_charViewUserName=null;// TODO: Put this elsewhere? Maybe in the charview itself?..
        [SerializeField] private Image[] i_roses=null;
        private ContViewBachelor[] contViews; // Set in Open.
        // References
        private List<Contestant> contsChosen; // those that HAVE been chosen.
        private List<Contestant> contsUnchosen; // those left to be chosen.
        
        // Getters (Public)
        override public string FillInBlanks(string str) {
            if (!str.Contains("[")) { return str; } // No replacement chars? Return string as it is!
            str = str.Replace("[LastPickedCont]", LastPickedContNameStyled);
            str = str.Replace("[ContUnchosen0]", ContUnchosen0);
            str = str.Replace("[ContUnchosen1]", ContUnchosen1);
            str = str.Replace("[ContUnchosen2]", ContUnchosen2);
            return str;
        }
        // Getters (Private)
        private string LastPickedContNameStyled { get {
            if (contsChosen.Count == 0) { return "undefined"; }
            return contsChosen[contsChosen.Count-1].myPrio.NameStyled;
        }}
        private string ContUnchosen0 { get { return contsUnchosen.Count<=0 ? "undefined" : contsUnchosen[0].myPrio.NameStyled; } }
        private string ContUnchosen1 { get { return contsUnchosen.Count<=1 ? "undefined" : contsUnchosen[1].myPrio.NameStyled; } }
        private string ContUnchosen2 { get { return contsUnchosen.Count<=2 ? "undefined" : contsUnchosen[2].myPrio.NameStyled; } }
        
        
        // ----------------------------------------------------------------
        //  Open
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            
            t_charViewUserName.text = ud.UserName;
            contViews = this.GetComponentsInChildren<ContViewBachelor>(true);
            
            // Make contestants!
            contsChosen = new List<Contestant>();
            contsUnchosen = new List<Contestant>(conts);
            
            // Prep visuals!
            for (int i=0; i<NumConts; i++) {
                contViews[i].SetMyCont(this, conts[i]);
            }
            SetContViewsInteractable(false);
        }
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public bool DoFuncFromStory(string line) {
            if (!base.DoFuncFromStory(line)) {
                if (line.Contains("SetContViewsInteractableTrue")) {
                    SetContViewsInteractable(true);
                    return true;
                }
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
            foreach (ContViewBachelor cv in contViews) { cv.SetIsInteractable(val); }
        }
        
        private void OnChoseFinalCont() {
            // Remove losers from userPrios!!
            for (int i=0; i<contsUnchosen.Count; i++) {
                ud.EliminateUserPrio(contsUnchosen[i].myPrio);
            }
        }
        
        
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnClick_ContView(ContViewBachelor cv) {
            i_roses[contsChosen.Count].color = new Color(0,0,0, 0.3f);
            Contestant cont = cv.MyCont;
            contsChosen.Add(cont);
            contsUnchosen.Remove(cont);
            if (contsChosen.Count == 3) { OnChoseFinalCont(); }
            
            SetContViewsInteractable(false);
            cv.SetVisible(false); // TO DO: Animate this fella, maybe! Embellish the pageantry!
            StoryCont.AdvanceStory();
        }



    }
}