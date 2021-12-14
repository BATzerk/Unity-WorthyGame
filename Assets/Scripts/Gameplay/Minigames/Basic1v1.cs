using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class Basic1v1 : Minigame {
        // Components
        [SerializeField] private TextMeshProUGUI t_mainText=null;
        // Properties
        [SerializeField] private bool IsSelectionWinner = true; // if FALSE, then the one we pick will be the LOSER instead.
        [SerializeField] private string PreambleText="";
        [SerializeField] private string MainText="";
        [SerializeField] private string EndText="";
        [SerializeField] private string EndTextOutOfTime="";
        
        // Getters (Private)
        private Contestant OtherContestant(ContestantButton thisButton) {
            if (contestants.Count != 2) { Debug.LogError("Oops: calling OtherContestant for a Minigame that DOESN'T have exactly 2 Contestants."); return null; }
            if (thisButton.MyContestant == contestants[0]) { return contestants[1]; }
            return contestants[0];
        }
        
            
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            t_mainText.text = ReplaceTextVariables(PreambleText);
            // Hide contestants for now.
            SetContViewsVisible(false);
            
            ShowNextButton();
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override protected void OnOutOfTime() {
            t_mainText.text = ReplaceTextVariables(EndTextOutOfTime);
            base.OnOutOfTime();
        }
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            HideNextButton();
            // Show ContViews!
            SetContViewsVisible(true);
            t_mainText.text = ReplaceTextVariables(MainText);
            if (IsTimed) {
                StartTimer();
            }
        }
        override public void OnClick_ContButton(ContestantButton contButton) {
            Contestant winner;
            if (IsSelectionWinner) { winner = contButton.MyContestant; }
            else { winner = OtherContestant(contButton); }
            
            SetOutcome(winner);
            t_mainText.text = ReplaceTextVariables(EndText);
            OnMinigameComplete();
        }

    }
}