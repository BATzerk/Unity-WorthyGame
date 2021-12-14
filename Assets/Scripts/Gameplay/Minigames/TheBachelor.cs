using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
namespace MinigameNamespace {

    public class TheBachelor : Minigame {
        // Overrides
        //public override MinigameType MyType => MinigameType.TheBachelor;
        // Components
        [SerializeField] private TextMeshProUGUI t_header=null;
        // Properties
        private int NumRosesLeft;
        // References
        private List<Contestant> contestantsWithRoses;
        

        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            contestantsWithRoses = new List<Contestant>();
            NumRosesLeft = contestants.Count-1;
            t_header.text = "Welcome to The Bachelor!\n\nPlease choose " + NumRosesLeft + " Priorities to receive a Rose.";
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_ContButton(ContestantButton contButton) {
            // Make it not interactable
            contButton.SetInteractable(false);
            contestantsWithRoses.Add(contButton.MyContestant);
            
            // Spend the rose!
            NumRosesLeft --;
            if (NumRosesLeft <= 0) {
                OnSpentFinalRose();
            }
        }
        private void OnSpentFinalRose() {
            SetOutcome(contestantsWithRoses.ToArray());
            
            t_header.text = "\"" + LoserNameStylized + "... I love you.\n...\n...But I'm not <i>in</i> love with you.\"";
            //"It was a close competition! Ultimately, we don't love " + LoserNameStylized + ".";
            OnMinigameComplete();
        }
        
        
    }
}*/