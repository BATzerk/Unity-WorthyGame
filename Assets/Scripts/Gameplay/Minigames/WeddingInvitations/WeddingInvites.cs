using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class WeddingInvites : Minigame {
        // Enums
        public enum RSVP { Undefined, Yes, No }
        // Overrides
        public override int NumContestants() { return 4; }
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Button b_openMail=null;
        [SerializeField] private Button b_sendRSVPs=null;
        [SerializeField] private TextMeshProUGUI t_intro=null;
        [SerializeField] private TextMeshProUGUI t_instructions=null;
        [SerializeField] private Toggle tog_rsvpAYes=null;
        [SerializeField] private Toggle tog_rsvpANo=null;
        [SerializeField] private Toggle tog_rsvpBYes=null;
        [SerializeField] private Toggle tog_rsvpBNo=null;
        // References
        [SerializeField] private InviteView inviteA=null;
        [SerializeField] private InviteView inviteB=null;
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            inviteA.Prep(contestants[0],contestants[1]);
            inviteB.Prep(contestants[2],contestants[3]);
            
            t_intro.enabled = true;
            b_openMail.gameObject.SetActive(true);
            b_sendRSVPs.gameObject.SetActive(false);
            
            UpdateRSVPValidity();
        }
        
        private void UpdateRSVPValidity() {
            bool areRSVPsValid = (inviteA.MyRSVP==RSVP.Yes && inviteB.MyRSVP==RSVP.No) || (inviteA.MyRSVP==RSVP.No && inviteB.MyRSVP==RSVP.Yes);
            t_instructions.enabled = !areRSVPsValid;
            b_sendRSVPs.interactable = areRSVPsValid;
            if (areRSVPsValid) {
                b_sendRSVPs.gameObject.SetActive(true); // show ONLY once both RSVPs are valid.
            }
        }
        
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnRSVPTogChanged() {
            UpdateRSVPValidity();
        }
        public void OnClick_OpenMail() {
            t_intro.enabled = false;
            b_openMail.gameObject.SetActive(false);
            ////b_sendRSVPs.gameObject.SetActive(true);
            //inviteA.SetVisible(true);
            //inviteB.SetVisible(true);
            myAnimator.Play("ShowInvites");
        }
        
        
        public void OnClick_InviteA() {
            tog_rsvpAYes.isOn = true;
            tog_rsvpANo.isOn = false;
            tog_rsvpBYes.isOn = false;
            tog_rsvpBNo.isOn = true;
            OnRSVPTogChanged();
        }
        public void OnClick_InviteB() {
            tog_rsvpAYes.isOn = false;
            tog_rsvpANo.isOn = true;
            tog_rsvpBYes.isOn = true;
            tog_rsvpBNo.isOn = false;
            OnRSVPTogChanged();
        }
        
        
        public void OnClick_SendRSVPs() {
            StartCoroutine(Coroutine_SendRSVPsSeq());
        }
        private IEnumerator Coroutine_SendRSVPsSeq() {
            InviteView yesInvite = inviteA.MyRSVP==RSVP.Yes ? inviteA : inviteB;
            SetOutcome(yesInvite.ContA, yesInvite.ContB);
            myAnimator.Play("SendRSVPs");
            b_sendRSVPs.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            
            // It's over, Bill. Give it up.
            OnMinigameComplete();
        }
        
        
    }
}