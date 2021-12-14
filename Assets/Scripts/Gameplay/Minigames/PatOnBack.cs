using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class PatOnBack : Minigame {
        // Overrides
        public override int NumContestants() { return 2; }
        private const int NumPatsReq = 4;
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Button b_contA=null;
        [SerializeField] private Button b_contB=null;
        [SerializeField] private TextMeshProUGUI t_header=null;
        [SerializeField] private TextMeshProUGUI t_numPatsA=null;
        [SerializeField] private TextMeshProUGUI t_numPatsB=null;
        [SerializeField] private TextMeshProUGUI t_contNameA=null;
        [SerializeField] private TextMeshProUGUI t_contNameB=null;
        // Properties
        private int numPatsA;
        private int numPatsB;
        // References
        [SerializeField] private Sprite s_contSadA=null;
        [SerializeField] private Sprite s_contSadB=null;
        [SerializeField] private Sprite s_contHappy=null;
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            // Update visuals!
            //t_header.text = "Both these priorities just gave excellent presentations, but nobody acknowledged either of them.\n\nPlease give one of them FOUR encouraging pats on the back.";
            //t_header.text = "These priorities just gave excellent presentations, but nobody acknowledged either of them.\n\nPlease give one FOUR encouraging pats on the back.";
            t_header.enabled = true;
            ShowNextButton();
            numPatsA = 0;
            numPatsB = 0;
            OnChangedNumPats();
            b_contA.interactable = true;
            b_contB.interactable = true;
            b_contA.gameObject.SetActive(false);
            b_contB.gameObject.SetActive(false);
            t_contNameA.text = contestants[0].myPrio.text;
            t_contNameB.text = contestants[1].myPrio.text;
            b_contA.image.sprite = s_contSadA;
            b_contB.image.sprite = s_contSadB;
        }
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnClick_ContPat(int contIndex) {
            if (contIndex==0) {
                numPatsA ++;
                myAnimator.Play("PatA", -1, 0);
            }
            else {
                numPatsB ++;
                myAnimator.Play("PatB", -1, 0);
            }
            OnChangedNumPats();
        }
        
        private void OnChangedNumPats() {
            t_numPatsA.text = numPatsA + "/" + NumPatsReq + "   PATS";
            t_numPatsB.text = numPatsB + "/" + NumPatsReq + "   PATS";
            if (numPatsA>=4) { OnPatContEnoughTimes(0); }
            else if (numPatsB>=4) { OnPatContEnoughTimes(1); }
        }
        private void OnPatContEnoughTimes(int contIndex) {
            SetOutcome(contestants[contIndex]);
            //t_header.text = "";//ReplaceTextVariables("[Winner] genuinely appreciates your acknowledgment.");
            t_header.enabled = false;
            Image i_winner = contIndex==0 ? b_contA.image : b_contB.image;
            i_winner.sprite = s_contHappy;
            i_winner.rectTransform.sizeDelta = new Vector2(350, 400);
            t_numPatsA.text = "";
            t_numPatsB.text = "";
            b_contA.interactable = false;
            b_contB.interactable = false;
            OnMinigameComplete();
        }
        
        override public void OnClick_Next() {
            HideNextButton();
            //StartTimer();
            
            b_contA.gameObject.SetActive(true);
            b_contB.gameObject.SetActive(true);
            //// Show knife and start animations!
            //i_knife.gameObject.SetActive(true);
            //KnifePos = knifePosNeutral;
            //contA.PlayAnim_HandOpenClose();
            //contB.PlayAnim_HandOpenClose();
        }
    }
}