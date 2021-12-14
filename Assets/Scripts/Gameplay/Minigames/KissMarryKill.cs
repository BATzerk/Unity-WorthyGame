using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class KissMarryKill : Minigame {
        // Components
        [SerializeField] private TextMeshProUGUI t_header=null;
        // Properties
        //private int pickIndex;
        // References
        private List<Contestant> contsSelected; // 0 is Kiss, 1 is Marry, 2 is Kill
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            contsSelected = new List<Contestant>();
            t_header.text = "Welcome to KISS, MARRY, KILL!\n\nPlease choose which you'd like to KISS.";
        }
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_ContButton(ContestantButton contButton) {
            // Deactivate the button
            contButton.SetInteractable(false);
            // Add it to the list!
            contsSelected.Add(contButton.MyContestant);
            
            string contName = contButton.MyContestant.PrioNameStyled;
            switch (contsSelected.Count) {
                case 1:
                    t_header.text = "You kiss " + contName + ". Its breath tastes like cinnamon.\n\nNow, who do you MARRY?";
                    break;
                case 2:
                    t_header.text = "You marry " + contName + ".\n\nPlease MURDER the remaining Priority.";// It makes a terrific spouse. Excellent communication skills.
                    break;
                case 3:
                    t_header.text = "You conk out " + contName + ".";//attempt to murder " + contName + ", but it narrowly escapes, choking on tears.\n\nYou consider chasing it, but decline the invitation. Your face slides into a devious smile. 'I have better things planned for you,' you whisper.";//A devious smile plays across your lips. You know it won't squeal
                    SetOutcome(contsSelected[0], contsSelected[1]); // the first two are the real winners.
                    OnMinigameComplete();
                    break;
            }
        }
        
    }
}