using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    abstract public class Minigame : BaseViewElement {
        // Enums
        //protected enum States { Undefined, Prepped, Begun, SetOutcome, Complete }
        // Properties
        [SerializeField] public string Title;
        // References
        protected MinigameController minigameCont { get; private set; }



        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        public void Initialize(MinigameController mc) {
            //CurrState = States.Undefined;
            minigameCont = mc;
            SetVisible(false); // hide 'em all by default.
        }



        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        virtual public void Open() {
            SetVisible(true);
        }
        virtual public void Hide() {
            SetVisible(false);
        }
        protected void ShowNextButton(string btnText = "NEXT") {
            minigameCont.b_minigameNext.GetComponentInChildren<TextMeshProUGUI>().text = btnText;
            minigameCont.b_minigameNext.gameObject.SetActive(true);
        }
        protected void HideNextButton() { minigameCont.b_minigameNext.gameObject.SetActive(false); }
        //private void SetTimerBarVisible(bool val) { minigameCont.commonTimerBar.SetVisible(val); }





        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        virtual public void OnClick_Next() { }
        virtual public void OnCountdownComplete() { }
        


        /*
        [SerializeField] private float TimeToChoose = -1; // if -1, we're untimed.
        [SerializeField] public bool DoShowTimerBar=false; // if TRUE, AND I'm timed, we'll show the commonTimerBar.
        //public bool IsComplete { get; private set; } // TRUE when we call OnMinigameComplete.
        protected States CurrState;
        //public bool DidSetOutcome { get; private set; } // TRUE when we set some outcome.
        
        // Getters (Public)
        public bool DidSetOutcome { get { return CurrState >= States.SetOutcome; } }
        public bool IsComplete { get { return CurrState >= States.Complete; } }
        virtual public int NumContestants() { return ContViews.Length; }
        public bool IsTimed { get { return TimeToChoose > 0; } }
        // Getters (Protected)
        private string UserName { get { return ud.UserName; } }
        protected string WinnerNameStylized { get { return Winners==null||Winners.Count==0?"undefined" : Winners[0].PrioNameStyled; } }
        protected string LoserNameStylized { get { return Losers==null||Losers.Count==0?"undefined" :Losers[0].PrioNameStyled; } }
        protected string ContNameStylized(int index) { return index>=contestants.Count ? "undefined" : contestants[index].PrioNameStyled; }
        // Getters (Private)
        private List<Contestant> GetOtherContestants(Contestant[] theseConts) {
            // Make a HashSet for easy look-up.
            HashSet<Contestant> theseContsHash = new HashSet<Contestant>();
            foreach (Contestant c in theseConts) { theseContsHash.Add(c); }
            // For every contestant in OUR list that's not in the GIVEN list, add to NEW list (of others)!
            List<Contestant> list = new List<Contestant>();
            foreach (Contestant c in contestants) {
                if (!theseContsHash.Contains(c)) {
                    list.Add(c);
                }
            }
            return list;
        }
        virtual protected string ReplaceTextVariables(string str) {
            str = str.Replace("\\n", "\n"); // kinda awkward workaround. UnityEditor adds an extra \ to "\n" strings in Inspector.
            if (!str.Contains("[")) { return str; } // No replacement chars? Return string as it is!
            str = str.Replace("[UserName]", UserName);
            str = str.Replace("[Winner]", WinnerNameStylized);
            str = str.Replace("[Loser]", LoserNameStylized);
            str = str.Replace("[Cont0]", ContNameStylized(0));
            str = str.Replace("[Cont1]", ContNameStylized(1));
            str = str.Replace("[Cont2]", ContNameStylized(2));
            str = str.Replace("[Cont3]", ContNameStylized(3));
            if (str.Contains("[")) { Debug.LogError("Error! Unhandled speech-text-fill-in in string: \"" + str + "\""); } // Safety check-- if there are still brackets, print an error.
            return str;
        }


        
        
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        virtual public void Prep(List<Contestant> contestants) {
            this.contestants = contestants;
            CurrState = States.Prepped;
            isTimerActive = false; // No timer for now.
            SetVisible(true);
            HideNextButton();
            SetTimerBarVisible(false);
            
            PrepContViews();
        }
        /// Override this for MULTI-contestant scenarios (where one ContestantView has like 2 Contestants)!
        virtual protected void PrepContViews() {
            if (ContViews.Length == 0) { return; } // Safety check.
            for (int i=0; i<contestants.Count && i<ContViews.Length; i++) {
                ContViews[i].Prep(contestants[i]);
            }
        }
        
        virtual public void Begin() {
            CurrState = States.Begun;
            for (int i=0; i<ContViews.Length; i++) {
                ContViews[i].Begin();
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Timer Stuff
        // ----------------------------------------------------------------
        // Properties
        private bool isTimerActive; // true WHEN we're counting down.
        private float timeToChooseLeft; // this counts down if we're timed.
        // Start / Stop
        protected void StartTimer() {
            isTimerActive = true;
            if (DoShowTimerBar) { SetTimerBarVisible(true); }
            SetTimeToChooseLeft(TimeToChoose);
        }
        protected void StopTimer() {
            isTimerActive = false;
            SetTimerBarVisible(false);
        }
        // Events
        virtual protected void OnOutOfTime() {
            SetOutcomeAsTie();
            OnMinigameComplete();
        }
        // Doers
        private void SetTimeToChooseLeft(float val) {
            timeToChooseLeft = val;
            UpdateTimerBarFills();
            // Outta time? Gotta blast!
            if (timeToChooseLeft <= 0) {
                OnOutOfTime();
            }
        }
        private void UpdateTimerBarFills() {
            float percentTimeLeft = timeToChooseLeft/TimeToChoose;
            // Update common timerBar.
            minigameCont.commonTimerBar.UpdateFill(timeToChooseLeft, percentTimeLeft);
            // Update my ContestantViews.
            foreach (ContestantView cv in ContViews) {
                cv.UpdateTimerFill(timeToChooseLeft, percentTimeLeft);
            }
        }
        // Update
        private void Update() {
            if (isTimerActive) {
                SetTimeToChooseLeft(timeToChooseLeft - Time.deltaTime);
            }
        }
        
        
        
        
        // ----------------------------------------------------------------
        //  Set Outcome
        // ----------------------------------------------------------------
        protected void SetOutcomeAsTie() {
            // Set ref lists.
            this.Winners = new List<Contestant>();
            this.Losers = new List<Contestant>();
            // Tell everyone they've just been tiiied.
            //float tiedFraction = 1 / contestants.Length; // evenly split the tie-points among all contestants.
            foreach (Contestant c in contestants) {
                c.myPrio.NumBattlesTied += 1;//tiedFraction;
            }
            OnSetOutcome();
        }
        protected void SetOutcomeLoser(params Contestant[] losers) { SetOutcome(GetOtherContestants(losers).ToArray()); }
        protected void SetOutcome(params Contestant[] winners) {
            // Set ref lists.
            this.Winners = new List<Contestant>(winners);
            this.Losers = GetOtherContestants(winners);
            
            // Reward winners!
            if (Winners.Count > 0) {
                float winFraction = 1 / (float)Winners.Count; // if 1 wins, this is 1. If 2 win, this is 0.5. If 3 win, this is 0.3333. Etc.
                foreach (Contestant c in Winners) {
                    c.myPrio.NumBattlesWon += winFraction;
                }
            }
            // Demote losers!
            if (Losers.Count > 0) {
                float loseFraction = 1 / (float)Losers.Count;
                foreach (Contestant c in Losers) {
                    c.myPrio.NumBattlesLost += loseFraction;
                }
            }
            OnSetOutcome();
        }
        
        
        private void OnSetOutcome() {
            CurrState = States.SetOutcome;
            StopTimer(); // Make sure to stop the timer.
            SetContViewsInteractable(false); // Make all contViews not interactable.
        }
        
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        virtual public void OnClick_ContButton(ContestantButton contButton) { }
        virtual public void OnBeginDrag_ContDraggable(ContestantDraggable cont) { }
        virtual public void OnEndDrag_ContDraggable(ContestantDraggable cont) { }
        
        public void OnClick_Done() {
            minigameCont.EndCurrMinigame();
        }
        
        protected void OnMinigameComplete() {
            CurrState = States.Complete;
            minigameCont.ShowEndMinigameButton();
        }
        
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //  Debug
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        virtual public void Debug_PickRandOutcome() {
            if (contestants.Count > 0) { // Safety check.
                // By default, pick one RANDOM winner.
                SetOutcome(contestants[Random.Range(0, contestants.Count)]);
            }
        }
        */



    }
}
