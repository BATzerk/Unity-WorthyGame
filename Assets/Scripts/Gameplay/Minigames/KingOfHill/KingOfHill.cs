using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using MinigameNamespace.KingOfHillNS;


namespace MinigameNamespace {
    public class KingOfHill : Minigame {
        //private readonly string[] SillyConts = {
        //    "A good pickle",
        //    "Dirty clothes",
        //    "The letter 'A'",
        //    "A backup toothbrush",
        //    "A chocolate fork",
        //    "Takin' out the trash",
        //    "Guy-on-Guy Fieri",
        //    "Gay Pride, and Prejudice",
        //    "Spreading myself too thin",
        //    "Perfectionism",
        //    "Not saying 'no' when I should",
        //    "Something that won't matter next week",
        //    "Hustling for approval",
        //    "Trying to act 'cool'",
        //    "Forgetting what matters",
        //};
        
        // Constants
        private float SecPerBattle = 6; // how long each battle lazts.
        // Enums
        private enum BattleStates { Undefined, PreBattle, Battle, PostBattle }
        // Components
        [SerializeField] private Image i_timerFill=null; // full background fill for the timer.
        // Properties
        private float battleTimeLeft; // reset for each battle.
        private int currBattleIndex; // index in battles list.
        private BattleStates battleState;
        // References
        private Contestant contA;
        private Contestant contB;
        private List<Contestant> upcomingConts; // populated with contestants we're provided, and removed from as battles ensue.
        // Overrides
        override public int NumContestants() { return 6; }
        
        // Getters (Private)
        private bool IsAnotherBattle() { return upcomingConts.Count > 0; }
        private ContViewKingOfHill contButtonA { get { return ContViews[0] as ContViewKingOfHill; } }
        private ContViewKingOfHill contButtonB { get { return ContViews[1] as ContViewKingOfHill; } }
        private Contestant OtherContestant(Contestant _cont) {
            if (_cont == null) { return null; }
            if (_cont == contA) { return contB; }
            if (_cont == contB) { return contA; }
            Debug.LogError("Huh! Asking KingOfHill for OtherContestant, but one provided isn't EITHER of its contestants. Conts: " + contA + ", " + contB);
            return null;
        }


        // ----------------------------------------------------------------
        //  Begin!
        // ----------------------------------------------------------------
        public override void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            SetContViewsVisible(false);
            
            upcomingConts = new List<Contestant>(contestants);
            upcomingConts.Reverse(); // Put the LEAST important ones FIRST
            //upcomingConts.Shuffle(); // Randomize 'em totally!
            contA = null;
            contB = null;
            
            // Look right.
            battleState = BattleStates.PreBattle;
            i_timerFill.rectTransform.sizeDelta = new Vector2(0, myRT.rect.height-i_timerFill.rectTransform.anchoredPosition.y);
        }
        override public void Begin() {
            base.Begin();
            SetContViewsVisible(true);
            
            StartNewBattle();
        }
        
        // ----------------------------------------------------------------
        //  Starting Battles
        // ----------------------------------------------------------------
        private void StartNewBattle() {
            // Last battle? End the minigame!
            if (!IsAnotherBattle()) {
                OnMinigameComplete();
            }
            // More contestants! Start new battle!
            else {
                StartCoroutine(Coroutine_SeqStartBattle());
            }
        }
        private IEnumerator Coroutine_SeqStartBattle() {
            battleState = BattleStates.PreBattle;
            battleTimeLeft = SecPerBattle;
            UpdateTimerFills();
            
            // Replenish my conts!
            if (contA == null) {
                contA = upcomingConts[0];
                upcomingConts.RemoveAt(0);
            }
            if (contB == null) {
                contB = upcomingConts[0];
                upcomingConts.RemoveAt(0);
            }
            
            contButtonA.Prep(contA);
            contButtonB.Prep(contB);
            contButtonA.Begin();
            contButtonB.Begin();
            yield return new WaitForSeconds(0.5f);
            
            contButtonA.SetInteractable(true);
            contButtonB.SetInteractable(true);
            battleState = BattleStates.Battle;
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        /** Note: If time runs out, this is called with prioButton as NULL. */
        override public void OnClick_ContButton(ContestantButton contButton) {
            StartCoroutine(Coroutine_SeqBattleOver(contButton));
        }
        
        private IEnumerator Coroutine_SeqBattleOver(ContestantView winnerView) {
            // Save outcome and update battleState.
            SaveBattleOutcome(winnerView);
            battleState = BattleStates.PostBattle;
            // Null out anyone who DIDn't win (this'll make both clear if they tie).
            Contestant winner = winnerView?.MyContestant;
            if (winner != contA) { contA = null; }
            if (winner != contB) { contB = null; }
            // Animate buttons out.
            contButtonA.AnimateBattleOver(winnerView);
            contButtonB.AnimateBattleOver(winnerView);
            yield return new WaitForSeconds(winnerView==null ? 0.8f : 0.4f); // wait a moment longer if they didn't choose.
            
            // Start next battle or round!
            StartNewBattle();
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SaveBattleOutcome(ContestantView contView) {
            // We DID pick a winner!
            if (contView != null) {
                Contestant winner = contView.MyContestant;
                Contestant loser = OtherContestant(winner);
                // Give HALF points to both. This minigame weighs less than others.
                winner.myPrio.NumBattlesWon += 0.5f;
                loser.myPrio.NumBattlesLost += 0.5f;
            }
            // We did NOT pick a winner.
            else {
                contA.myPrio.NumBattlesTied ++;
                contB.myPrio.NumBattlesTied ++;
            }
        }
        
    
    
        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            CountDownBattleTimer();
        }
        private void CountDownBattleTimer() {
            if (battleState == BattleStates.Battle) {
                // Count down!
                battleTimeLeft -= Time.deltaTime;
                UpdateTimerFills();
                if (battleTimeLeft <= 0) {
                    OnClick_ContButton(null); // choose FOR the player.
                }
            }
        }
        private void UpdateTimerFills() {
            float percentTimeLeft = battleTimeLeft / SecPerBattle;
            // My timer bar!
            float fillHeight = percentTimeLeft * (myRT.rect.height-i_timerFill.rectTransform.anchoredPosition.y); // make it reach empty before actually running out of time.
            i_timerFill.rectTransform.sizeDelta = new Vector2(0, fillHeight);
            // My ContestantViews!
            foreach (ContestantView cv in ContViews) {
                cv.UpdateTimerFill(battleTimeLeft, percentTimeLeft);
            }
        }
        
        
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //  Debug
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        override public void Debug_PickRandOutcome() {
            int safetyCount=0;
            while (IsAnotherBattle()) {
                if (++safetyCount > 99) { Debug.LogError("Caught in infinite loop in KingOfHill Debug_PickRandOutcome!"); break; } // Safety check.
                StartNewBattle();
                OnClick_ContButton(MathUtils.RandomBool() ? contButtonA : contButtonB);
            }
        }
        
    
    
    }
}