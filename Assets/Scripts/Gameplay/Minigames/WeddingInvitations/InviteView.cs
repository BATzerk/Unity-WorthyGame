using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class InviteView : BaseViewElement {
        // Components
        [SerializeField] private TextMeshProUGUI t_prioNameA=null;
        [SerializeField] private TextMeshProUGUI t_prioNameB=null;
        [SerializeField] private TextMeshProUGUI t_date=null;
        [SerializeField] private Toggle tog_rsvpYes=null;
        [SerializeField] private Toggle tog_rsvpNo=null;
        [SerializeField] private Toggle[] togs_meal=null;
        // References
        [SerializeField] WeddingInvites myMinigame=null;
        public Contestant ContA { get; private set; }
        public Contestant ContB { get; private set; }
        // Properties
        [SerializeField] private bool DoNamesAllCaps=false;
        public WeddingInvites.RSVP MyRSVP { get; private set; }
        
        // Getters (Private)
        private string TodayMonthAndDay() {
            System.DateTime tomorrow = System.DateTime.Now.AddDays(1);
            string month = GetMonthName(tomorrow.Month);
            int day = tomorrow.Day;
            return month.ToUpper() + " " + day;
        }
        private string GetMonthName(int month) {
            switch (month) {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "Monthtober";
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        public void Prep(Contestant contA,Contestant contB) {
            SetVisible(false); // Start hidden.
            // Set refs
            this.ContA = contA;
            this.ContB = contB;
            // Set texts!
            t_prioNameA.text = contA.myPrio.text;
            t_prioNameB.text = contB.myPrio.text;
            if (DoNamesAllCaps) {
                t_prioNameA.text = t_prioNameA.text.ToUpper();
                t_prioNameB.text = t_prioNameB.text.ToUpper();
            }
            t_date.text = TodayMonthAndDay();
            // Update toggles!
            tog_rsvpNo.isOn = false;
            tog_rsvpYes.isOn = false;
            UpdateMyRSVP();
            foreach (Toggle tog in togs_meal) { tog.isOn = false; }
        }


        private void UpdateMyRSVP() {
            if (!tog_rsvpNo.isOn && !tog_rsvpYes.isOn) {
                MyRSVP = WeddingInvites.RSVP.Undefined;
            }
            else if (tog_rsvpNo.isOn) {
                MyRSVP = WeddingInvites.RSVP.No;
            }
            else {
                MyRSVP = WeddingInvites.RSVP.Yes;
            }
        }



        // ----------------------------------------------------------------
        //  Toggle Events
        // ----------------------------------------------------------------
        public void OnRSVPToggleChanged(Toggle tog) {
            // Untoggle the OTHER one. Only one at a time.
            if (tog.isOn) {
                Toggle otherTog = tog==tog_rsvpYes?tog_rsvpNo:tog_rsvpYes;
                otherTog.isOn = false;
            }
            UpdateMyRSVP();
            // Tell my minigame!
            myMinigame.OnRSVPTogChanged();
        }
        public void OnMealToggleChanged(Toggle tog) {
            // It's on? Turn OFF the others.
            if (tog.isOn) {
                foreach (Toggle t in togs_meal) { if (t!=tog) { t.isOn = false; } }
            }
        }
        
        
        
    }
}