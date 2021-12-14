//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using DisOrDatNamespace;
//using System.Linq;


///** OLD! Do not use! DisOrDat is a MINIGAME now. */
//public class DisOrDatController : BaseViewElement {
//    // TEMP TESTING
//    private readonly Priority[] DefaultPrioList = {
//        //new Priority("A"),
//        //new Priority("B"),
//        //new Priority("C"),
//        //new Priority("D"),
//        //new Priority("E"),
//        //new Priority("F"),
//        //new Priority("G"),
//        //new Priority("H"),
//        //new Priority("I"),
//        //new Priority("J"),
//        //new Priority("K"),
//        //new Priority("L"),
//        //new Priority("M"),
//        //new Priority("N"),
//        //new Priority("O"),
//        //new Priority("P"),
//        //new Priority("Q"),
//        //new Priority("R"),
//        //new Priority("S"),
//        //new Priority("T"),
//        //new Priority("U"),
//        //new Priority("V"),
//        new Priority("Family"),
//        new Priority("My kids"),
//        new Priority("My partner"),
//        new Priority("Dating"),
//        new Priority("Socializing"),
//        new Priority("Volunteering"),
//        new Priority("Earning money"),
//        new Priority("Having enough money"),
//        new Priority("My career"),
//        new Priority("School"),
//        new Priority("Boss"),
//        new Priority("Coworkers"),
//        new Priority("Employees"),
//        new Priority("Being a leader"),
//        new Priority("Mental health"),
//        new Priority("Physical health"),
//        new Priority("My looks"),
//        new Priority("Creating"),
//        new Priority("Joy and Playfulness"),
//        new Priority("Alone time"),
//        new Priority("Reading"),
//        new Priority("Curbing addiction"),
//    };
//    private readonly SillyBattle[] sillyBattleOptions = {
//        new SillyBattle("Puppies", "Kitties"),
//        new SillyBattle("Chocolate", "Vanilla"),
//        new SillyBattle("Day", "Night"),
//        new SillyBattle("Tomato", "Tomato"),
//        //new SillyBattle("Batman", "Robin"),
//        //new SillyBattle("Boxers", "Briefs"),
//        new SillyBattle("Love", "War"),
//        new SillyBattle("Horrific anxiety", "Five dollars"),
//        new SillyBattle("One apple", "Two apples"),
//        new SillyBattle("Tom Hanks", "Voldemort"),
//        new SillyBattle("Baby panda", "Adult panda"),
//        new SillyBattle("Mimes", "Clowns"),
//        new SillyBattle("Magic tricks", "Ignoring your son"),
//        //new SillyBattle("Grammar Nazis", "Regular Nazis"),
//        new SillyBattle("Perseverence", "Sweating on a date"),
//        new SillyBattle("Caring", "Sharing"),
//        new SillyBattle("Time", "Space"),
//        new SillyBattle("USA", "North America"),
//        new SillyBattle("Touching your toes", "Touching your bros"),
//        new SillyBattle("Science", "The Cupid Shuffle"),
//        new SillyBattle("Faking it", "Shaking it"),
//        new SillyBattle("Ping pong", "Table tennis"),
//        new SillyBattle("Hopscotch", "Hot scotch"),
//        new SillyBattle("Warm hug", "Wet pizza (like <i>really</i> wet)"),
//        new SillyBattle("Synergy", "Fishnet stockings"),
//        new SillyBattle("Rabbits", "Holes"),
//        new SillyBattle("An impressive resume", "Really long receipt"),
//    };
//    // Constants
//    private float SecPerBattle = 5; // how long each battle lazts.
//    // Enums
//    private enum BattleStates { Undefined, PreBattle, Battle, PostBattle }
//    // Components
//    [SerializeField] private PrioButton prioButtonA=null;
//    [SerializeField] private PrioButton prioButtonB=null;
//    [SerializeField] private Image i_timerFill=null; // full background fill for the timer.
//    // Properties
//    private List<Battle> battles;
//    private Contestant[] contestants; // one for each Priority.
//    private float battleTimeLeft; // reset for each battle.
//    private float percentFullyOrdered=-1; // 0% is every Contestant is equal. 100% is we know the most imp, second-most imp, etc. Based on NumBattlesWon.
//    private int currBattleIndex; // index in battles list.
//    private int currRoundIndex; // which round it is. Each round includes a battle with every priority.
//    private int sillyBattleIndex; // testing silly-battles!
//    private BattleStates battleState;
//    // References
//    //[SerializeField] private GameController gameController=null;
//    private Battle currBattle;
    
//    // Getters (Private)
//    private Contestant ContestantA { get { return currBattle.ContestantA; } }
//    private Contestant ContestantB { get { return currBattle.ContestantB; } }
//    private Contestant OtherContestant(Contestant _cont) {
//        if (_cont == null) { return null; }
//        if (_cont == ContestantA) { return ContestantB; }
//        if (_cont == ContestantB) { return ContestantA; }
//        Debug.LogError("Huh! Asking Battle for OtherContestant, but one provided isn't EITHER of its contestants. Battle contestants: " + ContestantA + ", " + ContestantB);
//        return null;
//    }
    
    
//    // ----------------------------------------------------------------
//    //  Begin!
//    // ----------------------------------------------------------------
//    public void Open() {
//        SetVisible(true);
        
//        // TEST shuffle silly battles
//        sillyBattleOptions.Shuffle();
        
//        // Make contestants!
//        contestants = new Contestant[DefaultPrioList.Length];
//        for (int i=0; i<contestants.Length; i++) {
//            contestants[i] = new Contestant(DefaultPrioList[i]);
//        }
        
//        // Start first round!
//        SetCurrRound(0);
//    }
    
    
//    // ----------------------------------------------------------------
//    //  Starting Battles/Rounds
//    // ----------------------------------------------------------------
//    private void StartNextBattleOrRound() {
//        // Round over? Start next round!
//        if (currBattleIndex+1 >= battles.Count) {
//            SetCurrRound(currRoundIndex+1);
//        }
//        // Otherwise, start next battle!
//        else {
//            SetCurrBattle(currBattleIndex+1);
//        }
//    }
//    private void SetCurrRound(int _index) {
//        currRoundIndex = _index;
        
//        // Order contestants and update percentFullyOrdered.
//        contestants.Shuffle(); // shuffle them FIRST before ordering. So the order isn't predictable.
//        contestants = contestants.OrderBy(c => c.NumBattlesWon).ToArray<Contestant>();
//        UpdatePercentFullyOrdered();
//        // Make list of upcoming Battles!
//        MakeBattleList();
        
//        Debug_PrintContestantsList();
//        Debug_PrintBattlesList();
        
//        // Start the first battle!
//        SetCurrBattle(0);
//    }
//    private void SetCurrBattle(int _index) {
//        currBattleIndex = _index;
//        currBattle = battles[currBattleIndex];
        
//        StartCoroutine(Coroutine_SeqStartBattle());
//    }
//    private IEnumerator Coroutine_SeqStartBattle() {
//        battleState = BattleStates.PreBattle;
//        battleTimeLeft = SecPerBattle;
//        UpdateTimerFillHeight();
        
//        prioButtonA.ResetAndAnimateIn(ContestantA);
//        prioButtonB.ResetAndAnimateIn(ContestantB);
//        yield return new WaitForSeconds(0.5f);
        
//        prioButtonA.SetInteractable(true);
//        prioButtonB.SetInteractable(true);
//        battleState = BattleStates.Battle;
//    }
    
    
//    // ----------------------------------------------------------------
//    //  Events
//    // ----------------------------------------------------------------
//    /** Note: If time runs out, this is called with prioButton as NULL. */
//    public void OnChoosePrioButton(PrioButton prioButton) {
//        StartCoroutine(Coroutine_SeqBattleOver(prioButton));
//    }
    
//    private IEnumerator Coroutine_SeqBattleOver(PrioButton prioButton) {
//        // Save outcome and update battleState.
//        SaveBattleOutcome(prioButton);
//        battleState = BattleStates.PostBattle;
//        // Animate buttons out.
//        prioButtonA.AnimateBattleOver(prioButton);
//        prioButtonB.AnimateBattleOver(prioButton);
//        yield return new WaitForSeconds(prioButton==null ? 0.64f : 0.4f); // wait a moment longer if they didn't choose.
        
//        // Start next battle or round!
//        StartNextBattleOrRound();
//    }
    
//    // ----------------------------------------------------------------
//    //  Doers
//    // ----------------------------------------------------------------
//    private void MakeBattleList() {
//        battles = new List<Battle>();
//        // Add regular priorities battles.
//        int numRegularBattles = Mathf.CeilToInt(contestants.Length*0.5f);
//        for (int i=0; i<numRegularBattles; i++) {
//            int indexA = i*2;
//            int indexB = (i*2+1) % contestants.Length; // pick the next one, OR wrap around to start.
//            if (MathUtils.RandomBool()) { // 50% chance to swap the two indexes for variety.
//                int temp = indexB;
//                indexB = indexA;
//                indexA = temp;
//            }
//            battles.Add(new Battle(contestants[indexA],contestants[indexB]));
//        }
//        // Add some silly options.
//        int numSillyBattles = 10;
//        for (int i=0; i<numSillyBattles; i++) {
//            battles.Add(sillyBattleOptions[sillyBattleIndex]);
//            sillyBattleIndex = (sillyBattleIndex+1) % sillyBattleOptions.Length; // increment sillyBattleIndex!
//        }
//        battles.Shuffle();
//    }
//    private void UpdatePercentFullyOrdered() {
//        // Make a list of JUST the rank values, then set percentFullyOrdered to the size of that against the number of contestants!
//        HashSet<int> ranks = new HashSet<int>();
//        for (int i=0; i<contestants.Length; i++) {
//            ranks.Add(contestants[i].NumBattlesWon);
//        }
//        percentFullyOrdered = ranks.Count / (float)contestants.Length;
//    }
//    private void SaveBattleOutcome(PrioButton prioButton) {
//        // We DID pick a winner!
//        if (prioButton != null) {
//            Contestant winner = prioButton.MyContestant;
//            Contestant loser = OtherContestant(winner);
//            winner.NumBattlesWon ++;
//            loser.NumBattlesLost ++;
//        }
//        // We did NOT pick a winner.
//        else {
//            ContestantA.NumBattlesTied ++;
//            ContestantB.NumBattlesTied ++;
//        }
//    }
    


//    // ----------------------------------------------------------------
//    //  Update
//    // ----------------------------------------------------------------
//    private void Update() {
//        CountDownBattleTimer();
//    }
//    private void CountDownBattleTimer() {
//        if (battleState == BattleStates.Battle) {
//            // Count down!
//            battleTimeLeft -= Time.deltaTime;
//            UpdateTimerFillHeight();
//            if (battleTimeLeft <= 0) {
//                OnChoosePrioButton(null); // choose FOR the player.
//            }
//        }
//    }
//    private void UpdateTimerFillHeight() {
//        float fillHeight = battleTimeLeft / SecPerBattle * myRT.rect.height;
//        i_timerFill.rectTransform.sizeDelta = new Vector2(0, fillHeight);
//    }
    
    
    
//    // ----------------------------------------------------------------
//    //  Close
//    // ----------------------------------------------------------------
//    public void Close() {
//        SetVisible(false);
//    }
    
    
    
//    // Debug
//    private void Debug_PrintBattlesList() {
//        string str = "Battles:\n";
//        for (int i=0; i<battles.Count; i++) {
//            str += "   " + battles[i].ToString();
//            if (i < battles.Count-1) { str += "\n"; }
//        }
//        Debug.Log(str);
//    }
//    private void Debug_PrintContestantsList() {
//        string str = "Contestants:\n";
//        for (int i=0; i<contestants.Length; i++) {
//            Contestant c = contestants[i];
//            str += "   " + c + " - won: " + c.NumBattlesWon + ", lost: " + c.NumBattlesLost + ", tied: " + c.NumBattlesTied;
//            if (i < contestants.Length-1) { str += "\n"; }
//        }
//        Debug.Log(str);
//    }
    
//    private void OnGUI() {
//        // percentFullyOrdered.
//        string str = Mathf.Round(percentFullyOrdered*100f) + "%";
//        GUIStyle style = new GUIStyle {
//            fontSize = 18,
//        };
//        style.normal.textColor = Color.white;
//        GUI.Label(new Rect(10,10, 100,100), str, style);
//    }


//}
