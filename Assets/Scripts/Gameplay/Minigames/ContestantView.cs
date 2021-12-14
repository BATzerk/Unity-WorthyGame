using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class ContestantView : BaseViewElement {
        // Constants
        private readonly Color c_timerFillNormal = new Color255(251,209,32).ToColor();
        // Components
        [SerializeField] private GameObject go_timer=null;
        [SerializeField] private Image i_timerFill=null;
        [SerializeField] protected TextMeshProUGUI t_prioName=null;
        // References
        //public Contestant MyContestant { get; protected set; }
        public Contestant[] MyContestants { get; private set; }
        protected Minigame myMinigame { get; private set; }
        public TextMeshProUGUI t_PrioName { get { return t_prioName; } }
        
        // Getters (Public)
        public Contestant MyContestant {
            get {
                if (MyContestants.Length != 1) { Debug.LogError("Oops, we can only call MyContestant for ContestantViews with only ONE contestant. MyContestants.Length: " + MyContestants.Length); }
                return MyContestants[0];
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        virtual public void SetMyMinigame(Minigame minigame) {
            this.myMinigame = minigame;
        }
        /// contestants: USUALLY just one. Can be more for multi-contestant views.
        virtual public void Prep(params Contestant[] contestants) {
            this.MyContestants = contestants;
            t_prioName.text = ""; // hide prio text until we start. Keep it a secret.
            SetInteractable(true);
            SetVisible(true);
            if (go_timer != null) { go_timer.SetActive(myMinigame.IsTimed); }
        }
        virtual public void Begin() {
            // Set text.
            //t_prioName.text = MyContestant.myPrio.text;
            string str = "";
            for (int i=0; i<MyContestants.Length; i++) {
                str += MyContestants[i].myPrio.text;
                if (i<MyContestants.Length-1) { str += "\n+\n"; }
            }
            t_prioName.text = str;
        }
        
        


        //public void SetMyContestants(params Contestant[] conts) {
        //    MyContestants = new List<Contestant>(conts);
        //    //MyContestant = null; // I no longer have one contestant.
        //}
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        virtual public void SetInteractable(bool val) {}
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void UpdateTimerFill(float timeLeft, float percentTimeLeft) {
            if (i_timerFill == null) { return; } // Safety check.
            
            // Fill
            i_timerFill.fillAmount = percentTimeLeft;
            // Color
            if (timeLeft < 2f) {
                bool b = MathUtils.Sin01Bool(timeLeft*30);
                i_timerFill.color = Color.Lerp(c_timerFillNormal,Color.red, b ? 0.4f : 1);
            }
            else {
                i_timerFill.color = c_timerFillNormal;
            }
        }
        
        
    }
}