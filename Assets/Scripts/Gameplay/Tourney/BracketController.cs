using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TourneyNamespace {
    public class BracketController : BaseViewElement {
        // Components
        [SerializeField] private BracketPrioView[] bracketPrios=null;
        [SerializeField] private TextMeshProUGUI t_roundHeader=null;
        // References
        [SerializeField] private TourneyController tourneyCont=null;
        
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        public void InitViews() {
            // Assign bracket texts!
            for (int i=0; i<tourneyCont.Contestants.Length; i++) {
                bracketPrios[i].InitVisuals(tourneyCont.Contestants[i]);
                bracketPrios[i].SetVisible(false);
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void RevealPrioCandidates() {
            for (int i=0; i<bracketPrios.Length; i++) {
                bracketPrios[i].Appear(1.8f + i*0.15f);
            }
        }
        
        public void ShowUpcomingBattleBrackets() {
            // Update header
            t_roundHeader.text = "ROUND " + (tourneyCont.CurrBattleIndex+1);
            t_roundHeader.enabled = true;
            // Update bracketPrios
            for (int i=0; i<bracketPrios.Length; i++) {
                bracketPrios[i].UpdateOnDeckVisuals(tourneyCont);
            }
        }
        
        
        public void ShowFinalResults() {
            SetVisible(true);
            // Update header.
            t_roundHeader.text = "";
            
            // Update bracketPrios
            for (int i=0; i<bracketPrios.Length; i++) {
                bracketPrios[i].UpdateVisualsFromFinalResults(tourneyCont);
            }
        }
        
        
    }
}

