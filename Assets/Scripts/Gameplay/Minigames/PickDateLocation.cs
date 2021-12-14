using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class PickDateLocation : Minigame {
        // Overrides
        public override int NumContestants() { return 0; } // Technically there are NO contestants for this minigame. No winners/losers. We're just picking their date location.
        // Components
        [SerializeField] private GameObject go_options=null; // images and buttons.
        [SerializeField] private Image i_outcome=null; // in the center of the screen
        [SerializeField] private Image i_option0=null;
        [SerializeField] private Image i_option1=null;
        [SerializeField] private TextMeshProUGUI t_header=null;
        // Properties
        [SerializeField] private int DateLocationsType = 0; // 0 for flower-arranging/bouldering, and 1 for hot-air-balloon/skydiving.
        // References
        private ContCouple myCouple; // set in Begin.

        // Getters (Protected)
        protected override string ReplaceTextVariables(string str) {
            str = str.Replace("[ContA]", myCouple.ContA.PrioNameStyled);
            str = str.Replace("[ContB]", myCouple.ContB.PrioNameStyled);
            return base.ReplaceTextVariables(str);
        }


        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            if (minigameCont.couples.Count == 0) { return; } // Safety check for development.
            
            // Set myCouple to the most recent one!
            myCouple = minigameCont.couples[minigameCont.couples.Count-1];
            
            if (DateLocationsType == 0) {
                t_header.text = ReplaceTextVariables("Where should [ContA] and [ContB] go on their second date?");//Happy news!\n
            }
            else {
                t_header.text = ReplaceTextVariables("[ContA] and [ContB] really clicked!\n\nWhat should they do for their second date?");//I have more good news! It turns out 
            }
            // Reset visuals.
            //ShowNextButton();
            //go_options.SetActive(false);
            go_options.SetActive(true);
            i_outcome.enabled = false;
            //i_option0.gameObject.SetActive(true);
            //i_option1.gameObject.SetActive(true);
            //b_option0.gameObject.SetActive(true);
            //b_option1.gameObject.SetActive(true);
        }
        
        
        
        
        // ----------------------------------------------------------------
        //  Input Events
        // ----------------------------------------------------------------
        //override public void OnClick_Next() {
        //    HideNextButton();
        //    go_options.SetActive(true);
        //}
        public void OnClick_DateLocation(int type) {
            go_options.SetActive(false);
            i_outcome.enabled = true;
            //b_option0.gameObject.SetActive(false);
            //b_option1.gameObject.SetActive(false);
            switch (type) {
                // DateLocation 0
                case 0:
                    //i_option1.gameObject.SetActive(false);
                    i_outcome.sprite = i_option0.sprite;
                    t_header.text = ReplaceTextVariables("They have a wonderful evening!");//The two share a lovely evening learning about the craft of flower arrangement.\n\nBest of all, everyone gets to bring their sample flowers home!");
                    break;
                case 1:
                    //i_option0.gameObject.SetActive(false);
                    i_outcome.sprite = i_option1.sprite;
                    t_header.text = ReplaceTextVariables("Looks like they're really hitting it off!");//The two go bouldering at Funtopia USA.\n\n[ContA] slips on a tricky jump, but [ContB] catches them.\n\n
                    break;
                // DateLocation 1
                case 2:
                    //i_option1.gameObject.SetActive(false);
                    i_outcome.sprite = i_option0.sprite;
                    t_header.text = ReplaceTextVariables("<3");//[ContA] was pretty scared at first, but once [ContB] held their hand, everything felt all right.\n\n<3");//The two kick off their second date with skydiving!\n\n
                    break;
                case 3:
                    //i_option0.gameObject.SetActive(false);
                    i_outcome.sprite = i_option1.sprite;
                    t_header.text = ReplaceTextVariables("<3");//[ContA] was a little nervous at first, but once [ContB] held their hand, everything felt all right.\n\n<3");//The two enjoy a romantic sunrise over the Appalachian Mountains. 
                    break;
                default:
                    t_header.text = "Undefined index for OnClick_DateLocation: " + type;
                    break;
            }
            OnMinigameComplete();
        }
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        //private IEnumerator Coroutine_BeginDateSeq() {
        //    // Save values, mon!
        //    SetOutcome(dateA.MyContestant, dateB.MyContestant);
        //    minigameController.AddCouple(dateA.MyContestant, dateB.MyContestant);
        //    // Change visuals.
        //    t_header.text = "";
        //    foreach (ContestantView cv in ContViews) {
        //        if (cv!=dateA && cv!=dateB) { cv.SetVisible(false); }
        //    }
        //    // Animate!
        //    myAnimator.Play("BeginDate");
        //    yield return new WaitForSeconds(8f);
            
        //    // All right, consider this donezo.
        //    OnMinigameComplete();
        //}
        
        
    }
}