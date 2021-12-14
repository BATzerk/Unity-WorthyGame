using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinigameNamespace.DisOrDatNS;
using System.Linq;


namespace MinigameNamespace {
    public class DisOrDat : Minigame {
        private static readonly SillyBattle[] mandatoryFirstSillies = { // these must come first, in this exact order.
            new SillyBattle("Chocolate", "Vanilla"),
            new SillyBattle("Puppies", "Kitties"),
            new SillyBattle("Now", "Later"),
            new SillyBattle("Bride", "Groom"),
        };
        private static readonly SillyBattle[] otherSillies = { // these come once all mandatoryFirstSillies are done.
            // Rando Comparos
            new SillyBattle("Horrific anxiety", "Five dollars"),
            new SillyBattle("Tom Hanks", "Voldemort"),
            new SillyBattle("Having perfect hair, forever", "A brand new car!!!!!!!!!"),
            new SillyBattle("Gospel choir narrating your life", "Knowing everything."),
            new SillyBattle("Warm hug", "Wet pizza (like <i>really</i> wet)"),
            new SillyBattle("Fishnet stockings", "Synergy"),
            new SillyBattle("An impressive resume", "A really long receipt"),
            new SillyBattle("Falling in love for the first time", "Hell"),
            new SillyBattle("Flying", "Getting too attached to the results"),
            new SillyBattle("Comparing two incomparable things", "Religion"),
            // Actual Comparos
            new SillyBattle("Beatles", "Beetles"),
            new SillyBattle("One God", "Two Gods"),
            new SillyBattle("Time", "Space"),
            new SillyBattle("Wham!", "Kablaam!"),
            new SillyBattle("Rabbits", "Holes"),
            new SillyBattle("Baby panda", "Adult panda"),
            new SillyBattle("Chicken Pad Thai", "General Tso's Chicken"),
            new SillyBattle("Their signature Reuben", "The meatloaf sandwich"),
            new SillyBattle("Being 18", "Being 19"),
            new SillyBattle("Music", "Words"),
            new SillyBattle("One anchovy", "Zero anchovies"),
            new SillyBattle("Applebee's", "Literally anywhere else"),
            new SillyBattle("Thick", "Thin"),
            new SillyBattle("Pride", "Prejudice"),
            new SillyBattle("Spider", "Man"),
            new SillyBattle("Duck duck duck duck duck duck", "Goose"),
            new SillyBattle("Bodies", "Antibodies"),
            new SillyBattle("God", "Nothingness"),
            new SillyBattle("Grafting", "Letting trees just do their thing"),
            new SillyBattle("Twosome", "Threesome"),
            new SillyBattle("One lip", "Two lips"),
            // Usable Preferences
            new SillyBattle("Day", "Night"),
            new SillyBattle("Keeping calm", "Carrying on"),
            new SillyBattle("High-fives", "Handshakes"),
            new SillyBattle("Making a checklist", "Keeping options open"),
            new SillyBattle("Friends", "Lovers"),
            new SillyBattle("Hugs", "Drugs"),
            new SillyBattle("Flirting", "Squirting"),
            new SillyBattle("Tokyo", "New York City"),
            new SillyBattle("Comic Sans", "Papyrus"),
            new SillyBattle("Wizards", "Witches"),
            // Linguistic Jokes, or Silly Comparos
            new SillyBattle("Bees?", "BEADS."),
            new SillyBattle("Luigi", "Waluigi"),
            new SillyBattle("Licking", "Wounds"),
            new SillyBattle("Limes", "Disease"),
            new SillyBattle("This one", "That one"),
            new SillyBattle("Tomato", "Tomato"),
            new SillyBattle("Pulp", "Fiction"),
            new SillyBattle("Should I stay", "Should I go"),
            new SillyBattle("To be", "Not to be"),
            new SillyBattle("Kate", "Moss"),
            new SillyBattle("Throwing down", "Throwing up"),
            new SillyBattle("Faking it", "Shaking it"),
            new SillyBattle("Hopscotch", "Hot scotch"),
            new SillyBattle("Caring", "Sharing"),
            new SillyBattle("Puns", "Buns"),
            new SillyBattle("Shaq", "Nasdaq"),
            new SillyBattle("Easy", "Breezy"),
            new SillyBattle("Brains", "Brians"),
            new SillyBattle("Surgery", "Perjery"),
            new SillyBattle("Cool cats", "Cool hats"),
            new SillyBattle("Thanking", "Shanking"),
            new SillyBattle("Shaking hands", "Shaking heads"),
            new SillyBattle("Summer vacation", "Bummer vacation"),
            new SillyBattle("Touching your toes", "Touching your bros"),
            new SillyBattle("Buts", "Butts"),
            new SillyBattle("Windows", "Walls"),
            new SillyBattle("Tripping", "Balls"),
            new SillyBattle("LOL", "BRB"),
            new SillyBattle("The entire first part of this", "Sentence"),
            new SillyBattle("Creative", "Liberties"),
            new SillyBattle("Context", "No context"),
            new SillyBattle("Against", "Vs."),
            new SillyBattle("<nobr>Mr. T</nobr>", "<nobr>Mrs. T</nobr>"),
            
            new SillyBattle("img=JapanSilhouette", "img=ChinaSilhouette"),
            
            new SillyBattle("<color=#24EB89><b>THIS COLOR</b></color>", "<color=#436859><b>THIS COLOR</b></color>"),
            new SillyBattle("<color=#F516EE><b>THIS COLOR</b></color>", "<color=#1A16F5><b>THIS COLOR</b></color>"),
            new SillyBattle("<color=#F1F516><b>THIS COLOR</b></color>", "<color=#F5A416><b>THIS COLOR</b></color>"),
            new SillyBattle("<color=#B39F7D><b>THIS COLOR</b></color>", "<color=#947DB3><b>THIS COLOR</b></color>"),



            //new SillyBattle("Following dreams", "Stalking dreams"),
            //new SillyBattle("Coconut", "Oil"),
            //new SillyBattle("Magic tricks", "Ignoring your son"),
            //new SillyBattle("Perseverence", "A sweaty upper lip"),
            //new SillyBattle("Love", "War"),
            //new SillyBattle("Entitlement", "Enlightenment"),
            //new SillyBattle("Ha", "Hahaha"),
            //new SillyBattle("Every", "One"),
            //new SillyBattle("Paying", "Attention"),
            //new SillyBattle("Ping pong", "Table tennis"),
            //Scott Pilgrim  The World
            //Undercover Boss
            //Flipping Switch
            //Sons raise meat   Suns rays meet
            //Vacuum Cleaners
            //Sally Fields
            //It’s Its
            //A The
            //Time   Time again
            //7 11
            //Users Losers
            //Beggars Choosers
            //Butts Busts
            //new SillyBattle("Shifting", "Fisting"),
            //new SillyBattle("USA", "North America"),
            //new SillyBattle("The Macarena", "The Cupid Shuffle"),
            //new SillyBattle("Mimes", "Clowns"),
            //new SillyBattle("One apple", "Two apples"),
            //new SillyBattle("Angles", "Angels"),
            //new SillyBattle("Locks", "Lochs"),
            //new SillyBattle("Shaking hands", "Shanking hands"),
            //new SillyBattle("Yes", "No"),
            //new SillyBattle("And", "Or"),
            //new SillyBattle("Being hot", "Being bothered"),
            //new SillyBattle("Gravity", "Chivalry"),
            //new SillyBattle("PB and J", "PB & J"),
            //new SillyBattle("602473899", "993073914"),
            //new SillyBattle("Apples", "Oranges"),
            //new SillyBattle("Batman", "Robin"),
            //new SillyBattle("Boxers", "Briefs"),
            //new SillyBattle("Grammar Nazis", "Regular Nazis"),
        };
        // Constants
        private float SecPerBattle = 5; // how long each battle lazts.
        // Enums
        private enum BattleStates { Undefined, PreBattle, Battle, PostBattle }
        // Components
        //[SerializeField] private ContestantButton contButtA=null;
        //[SerializeField] private ContestantButton contButtB=null;
        [SerializeField] private Image i_timerFill=null; // full background fill for the timer.
        // Properties
        private List<Battle> battles;
        private float battleTimeLeft; // reset for each battle.
        private int currBattleIndex; // index in battles list.
        private BattleStates battleState;
        // References
        private Battle currBattle;
        // Overrides
        override public int NumContestants() { return 6; }
        
        // Getters (Public)
        public static List<SillyBattle> GetNewSillyBattleList() {
            List<SillyBattle> list = new List<SillyBattle>(otherSillies);
            list.Shuffle(); // shuffle 'em up!
            list.InsertRange(0, mandatoryFirstSillies); // ok, now put mandatoryFirstSillies at beginning.
            return list;
        }
        // Getters (Private)
        private ContViewDisOrDat contButtonA { get { return ContViews[0] as ContViewDisOrDat; } }
        private ContViewDisOrDat contButtonB { get { return ContViews[1] as ContViewDisOrDat; } }
        private Contestant ContestantA { get { return currBattle.ContestantA; } }
        private Contestant ContestantB { get { return currBattle.ContestantB; } }
        private Contestant OtherContestant(Contestant _cont) {
            if (_cont == null) { return null; }
            if (_cont == ContestantA) { return ContestantB; }
            if (_cont == ContestantB) { return ContestantA; }
            Debug.LogError("Huh! Asking Battle for OtherContestant, but one provided isn't EITHER of its contestants. Battle contestants: " + ContestantA + ", " + ContestantB);
            return null;
        }
        


        // ----------------------------------------------------------------
        //  Begin!
        // ----------------------------------------------------------------
        public override void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            SetContViewsVisible(false);
            
            // Order contestants! NOTE: Don't have to! They come in ordered properly already.
            //contestants.Shuffle(); // shuffle them FIRST before ordering. So the order isn't predictable.
            //contestants = contestants.OrderBy(c => c.NumBattlesWon).ThenByDescending(c => c.NumBattlesLost).ToList();
            // Make list of upcoming Battles!
            MakeBattleList();
            
            //Debug_PrintContestantsList();
            //Debug_PrintBattlesList();
            
            // Look right.
            battleState = BattleStates.PreBattle;
            i_timerFill.rectTransform.sizeDelta = new Vector2(0, myRT.rect.height-i_timerFill.rectTransform.anchoredPosition.y);
        }
        override public void Begin() {
            base.Begin();
            SetContViewsVisible(true);
            
            // Start the first battle!
            SetCurrBattle(0);
        }
        
        
        // ----------------------------------------------------------------
        //  Starting Battles
        // ----------------------------------------------------------------
        private void StartNextBattleOrRound() {
            // Out of battles? End minigame!
            if (currBattleIndex+1 >= battles.Count) {
                OnMinigameComplete();
            }
            // Otherwise, start next battle!
            else {
                SetCurrBattle(currBattleIndex+1);
            }
        }
        private void SetCurrBattle(int _index) {
            currBattleIndex = _index;
            currBattle = battles[currBattleIndex];
            
            StartCoroutine(Coroutine_SeqStartBattle());
        }
        private IEnumerator Coroutine_SeqStartBattle() {
            battleState = BattleStates.PreBattle;
            battleTimeLeft = SecPerBattle;
            UpdateTimerFills();
            
            contButtonA.Prep(ContestantA);
            contButtonB.Prep(ContestantB);
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
        
        private IEnumerator Coroutine_SeqBattleOver(ContestantView contView) {
            // Save outcome and update battleState.
            SaveBattleOutcome(contView);
            battleState = BattleStates.PostBattle;
            // Animate buttons out.
            contButtonA.AnimateBattleOver(contView);
            contButtonB.AnimateBattleOver(contView);
            yield return new WaitForSeconds(contView==null ? 0.8f : 0.4f); // wait a moment longer if they didn't choose.
            
            // Start next battle or round!
            StartNextBattleOrRound();
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void MakeBattleList() {
            battles = new List<Battle>();
            // Add regular priorities battles.
            int numRegularBattles = Mathf.CeilToInt(contestants.Count*0.5f);
            for (int i=0; i<numRegularBattles; i++) {
                int indexA = i*2;
                int indexB = (i*2+1) % contestants.Count; // pick the next one, OR wrap around to start.
                if (MathUtils.RandomBool()) { // 50% chance to swap the two indexes for variety.
                    int temp = indexB;
                    indexB = indexA;
                    indexA = temp;
                }
                battles.Add(new Battle(contestants[indexA],contestants[indexB]));
            }
            // Add some silly options.
            int numSillyBattles = Mathf.RoundToInt(numRegularBattles);
            for (int i=0; i<numSillyBattles; i++) {
                int insertIndex = (i*2) % battles.Count; // make every OTHER a silly battle!
                battles.Insert(insertIndex, GetNextSillyBattleOption());
            }
            // Randomly insert one extra silly-battle somewhere unpredictable.
            int randInd = Random.Range(0, battles.Count);
            battles.Insert(randInd, GetNextSillyBattleOption());
        }
        private SillyBattle GetNextSillyBattleOption() {
            SillyBattle battle = ud.dodSillyBattles[ud.dodSillyBattleIndex];
            ud.dodSillyBattleIndex = (ud.dodSillyBattleIndex+1) % ud.dodSillyBattles.Count; // increment sillyBattleIndex!
            return battle;
        }
        private void SaveBattleOutcome(ContestantView contView) {
            // We DID pick a winner!
            if (contView != null) {
                Contestant winner = contView.MyContestant;
                Contestant loser = OtherContestant(winner);
                winner.myPrio.NumBattlesWon ++;
                loser.myPrio.NumBattlesLost ++;
            }
            // We did NOT pick a winner.
            else {
                ContestantA.myPrio.NumBattlesTied ++;
                ContestantB.myPrio.NumBattlesTied ++;
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
            for (int i=currBattleIndex; i<battles.Count; i++) {
                Priority pA = battles[i].ContestantA.myPrio;
                Priority pB = battles[i].ContestantB.myPrio;
                if (MathUtils.RandomBool()) {
                    pA.NumBattlesWon ++;
                    pB.NumBattlesLost ++;
                }
                else {
                    pB.NumBattlesWon ++;
                    pA.NumBattlesLost ++;
                }
            }
        }
        private void Debug_PrintBattlesList() {
            string str = "Battles:\n";
            for (int i=0; i<battles.Count; i++) {
                str += "   " + battles[i].ContestantA.ToString() + " vs. " + battles[i].ContestantB.ToString();
                if (i < battles.Count-1) { str += "\n"; }
            }
            Debug.Log(str);
        }
        //private void Debug_PrintContestantsList() {
        //    string str = "Contestants:\n";
        //    for (int i=0; i<contestants.Count; i++) {
        //        Contestant c = contestants[i];
        //        str += "   " + c + " - won: " + c.NumBattlesWon + ", lost: " + c.NumBattlesLost + ", tied: " + c.NumBattlesTied;
        //        if (i < contestants.Count-1) { str += "\n"; }
        //    }
        //    Debug.Log(str);
        //}
        private void Debug_PrintSillyBattleDistribution() {
            string str = "Regular-to-silly-battle distribution: ";
            foreach (Battle b in battles) {
                str += b is SillyBattle ? "b" : "A";
            }
            print(str);
        }
        
    
    
    }
}



            
            //// Randomly swap two battles, to keep it less predictable.
            //int indA = Random.Range(0, battles.Count);
            //int indB = Random.Range(0, battles.Count);
            //Battle tempBattle = battles[indA];
            //battles[indA] = battles[indB];
            //battles[indB] = tempBattle;
            //Debug_PrintSillyBattleDistribution();