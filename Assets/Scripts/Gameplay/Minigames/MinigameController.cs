using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinigameNamespace;
using System.Linq;

public class MinigameController : BaseViewElement {
    /*
    static private readonly RoundData[] roundDatas = {
        //// TESTING
        //new RoundData(new string[] {
        //    "UFOShootDown",
            
        //    "DoD",
        //    "KingOfHill",
            
        //    "ComfyChair",
        //    "MakeADate",
        //    "WindowChuck",
        //}),
        
        // ROUND 1: All 20
        new RoundData(PrioRank.Any,new string[] {
            "BurningBuilding",
            "BurningBuilding",
            "HaveToPee",
            "DoD",
            "PatOnBack",
            "DoD",
        }),
        
        // ROUND 2
        new RoundData(PrioRank.Any,new string[] {
            "ComfyChair",
            "KingOfHill",
            "FreeTonight",
            "KnifeToMurder",
            "KingOfHill",
            "BasketCatch",
        }),
        // ROUND 4
        new RoundData(PrioRank.Any,new string[] {
            "DoD",
            "UFOAbduction",
            "DoD",
            "UFOHungry",
            "KingOfHill",
            "UFOShootDown",
        }),
        // ROUND 5
        new RoundData(PrioRank.Any,new string[] {
            "LazySusan",
            "CaptchaFill",
            "DoD",
            "WindowChuck",
            "KingOfHill",
        }),
        
        
        
        // ROUND 3: Matchmakin'!
        new RoundData(PrioRank.Any,new string[] {
            "DoD",
            "MakeADate",
            "PickDateLocation0",
            "DoD",
            "MakeADate",
            "PickDateLocation1",
            "KingOfHill",
            "WeddingInvites",
        }),
        // ROUND 6: ____
        new RoundData(PrioRank.Any,new string[] {
            "VendingMachine",
            "KissMarryKill",
            "ExamComfort",
            "SinkingShip",
            "SaveDrowning",
        }),
        // ROUND 7: ____
        new RoundData(PrioRank.Any,new string[] {
            "DoD",
        }),
    };
    */
    
    // Constants
    public static int NumTotalRounds = 6;
    // Components
    [SerializeField] private Button b_endMinigame=null;
    [SerializeField] public  Button b_minigameNext=null; // the NEXT button that's shared across Minigames!
    [SerializeField] private MinigameTitleCurtain titleCurtain=null;
    [SerializeField] private NewRoundCurtain newRoundCurtain=null;
    [SerializeField] public  MinigameTimerBar commonTimerBar=null; // yellow, generic top bar.
    // Properties
    private Dictionary<string, Minigame> allMinigames; // GameObject name, Minigame.
    public Contestant[] AllContestants { get; private set; } // one for each Priority.
    //public int CurrRoundIndex { get; private set; }
    private int numTimesMadeUpcomingContListThisRound; // for developer.
    // References
    [SerializeField] private GameController gameController=null;
    public RoundData CurrRoundData { get; private set; }
    private Minigame currMinigame;
    private List<Contestant> upcomingContestants; // every round, this is set to allContestants, and taken from as we play through minigames. Upon depletion, we start a new round.
    public List<ContCouple> couples { get; private set; } // who we make fall in love with each other.//NOTE: This isn't saved anywhere!!!
    
    
    // Getters (Public)
    public int CurrRoundIndex { get { return CurrRoundData==null ? -1 : CurrRoundData.RoundIndex; } }
    // Getters (Private)
    private Contestant GetContestant(Priority prio) {
        foreach (Contestant c in AllContestants) { if (c.myPrio == prio) { return c; } }
        return null; // Hmm.
    }
    private bool MayUseContestantForMinigame(Contestant c, Minigame m) {
        if (m is MakeADate && c.HasPartner) { // This is a double-date, BUT this contestant's already in a relationship?? Return false.
            return false;
        }
        return true; // Sure, looks ok.
    }
    
    private List<Contestant> PullContestantsForMinigame(Minigame mg) {
        // Note: Use HashSet to guarantee no duplicates.
        HashSet<Contestant> list = new HashSet<Contestant>();
        // First, add any SPECIFIC custom ones for this minigame.
        AddSpecificContsForSpecificMinigame(mg, list);
        // Then, just fill in the rest of the list with (suitable) RANDOM contestants.
        AddRandContsForMinigame(mg, list);
        // Return!
        return list.ToList();
    }
    private void AddSpecificContsForSpecificMinigame(Minigame mg, HashSet<Contestant> list) {
        // WindowChuck? Add most important vs. least two important.
        if (mg is WindowChuck) {
            list.Add(GetContestant(userPrios[0]));
            list.Add(GetContestant(userPrios[NumUserPrios-2]));
            list.Add(GetContestant(userPrios[NumUserPrios-1]));
        }
        // WeddingInvites? Add already-known couples!
        if (mg is WeddingInvites) {
            for (int i=0; i<couples.Count; i++) {
                list.Add(couples[i].ContA);
                list.Add(couples[i].ContB);
            }
        }
    }
    private void AddRandContsForMinigame(Minigame mg, HashSet<Contestant> list) {
        int numContsForMG = mg.NumContestants();
        
        // Remove from upcomingContestants and add to list.
        int safetyCount=0;
        int indexCheck = 0; // if an upcomingContestant doesn't work, we'll increment this to try out the next one.
        while (list.Count < numContsForMG) {
            // Out of contestants? Remake 'em!
            if (upcomingContestants.Count==0 || indexCheck>=upcomingContestants.Count) {
                upcomingContestants = GetUpcomingContestants();
                indexCheck = 0;
            }
            // We MAY use this contestant for this minigame!
            Contestant c = upcomingContestants[indexCheck];
            if (MayUseContestantForMinigame(c, mg)) {
                list.Add(c);
                if (upcomingContestants.Contains(c)) { // Safety check. (In specific, uncommon minigames, we didn't get this val from upcomingContestants.)
                    upcomingContestants.Remove(c);
                }
            }
            // We may NOT use this contestant. Try the next one.
            else {
                indexCheck ++;
            }
            
            // Safety check.
            if (safetyCount++>99) {
                Debug.LogError("Caught in infinite loop finding next upcoming Contestant!");
                break;
            }
        }
    }
    private List<Contestant> GetUpcomingContestants() {
        numTimesMadeUpcomingContListThisRound ++;
        if (CurrRoundData.prioRank!=PrioRank.Any && numTimesMadeUpcomingContListThisRound>1) {
            Debug.LogError("Whoa! We have TOO MANY minigames in a round that's supposed to use prios in a certain order. Check the minigames in this round.");
        }
        
        List<Contestant> list = new List<Contestant>(AllContestants);
        switch (CurrRoundData.prioRank) {
            case PrioRank.Top:
                //list.Shuffle();
                list = list.OrderByDescending(c => c.myPrio.NumBattlesWon).ThenBy(c => c.myPrio.NumBattlesLost).ToList();
                break;
            case PrioRank.Bottom:
                //list.Shuffle();
                list = list.OrderBy(c => c.myPrio.NumBattlesWon).ThenByDescending(c => c.myPrio.NumBattlesLost).ToList();
                break;
            default:
                list.Shuffle(); // Note: Shuffling before ordering yields better randomness.
                list = list.OrderByDescending(c => c.myPrio.NumBattlesWon).ThenBy(c => c.myPrio.NumBattlesLost).ToList();
                break;
        }
        return list;
    }
    
    
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    private void InitializeMinigames() {
        // Make allMinigames list
        Minigame[] allMinigamesArray = GetComponentsInChildren<Minigame>(true);
        allMinigames = new Dictionary<string, Minigame>();
        foreach (Minigame m in allMinigamesArray) {
            m.Initialize(this);
            allMinigames.Add(m.name, m);
        }
        
        // Reset other lists!
        couples = new List<ContCouple>();
    }
    
    public void Open(RoundData roundData) {
        ud.Debug_AddMinimumTestUserPrios();
        // Didn't yet init? Init!
        if (allMinigames == null) {
            InitializeMinigames();
        }
        // Show me!
        SetVisible(true);
        StartRound(roundData);
    }
    //public void Close() {
    //    SetVisible(false);
    //}
    
    
    
    // ----------------------------------------------------------------
    //  Start Round
    // ----------------------------------------------------------------
    private void StartRound(RoundData roundData) {
        //if (roundIndex >= roundDatas.Length) { Debug.LogError("RoundData outta bounds! roundIndex: " + roundIndex); return; } // Safety check.
        // Set round values.
        //CurrRoundIndex = roundIndex;
        numTimesMadeUpcomingContListThisRound = 0;
        CurrRoundData = roundData;//roundDatas[CurrRoundIndex];
        // Remake AllContestants each round! userPrios may have changed.
        AllContestants = new Contestant[NumUserPrios];
        for (int i=0; i<AllContestants.Length; i++) {
            AllContestants[i] = new Contestant(userPrios[i]);
        }
        // Prep values.
        upcomingContestants = new List<Contestant>(); // clear this out between rounds.
        //Debug_PrintUpcomingMinigames();
        // Show newRound full-screen display!
        newRoundCurtain.Show();
        // Analytics!
        GameManagers.Instance.AnalyticsManager.OnBeginMinigameRound(CurrRoundIndex);
    }
    
    // ----------------------------------------------------------------
    //  Start / End Minigames
    // ----------------------------------------------------------------
    private void StartCurrMinigame() {
        string _name = CurrRoundData.CurrMinigameName();
        // Safety check.
        if (!allMinigames.ContainsKey(_name)) { Debug.LogError("Minigame with name doesn't exist in scene: " + _name); return; }
        
        // Hide all minigames
        HideAllMinigames();
        // Set currMinigame!
        currMinigame = allMinigames[_name];
        // Pull right number of contestants.
        List<Contestant> contestants = PullContestantsForMinigame(currMinigame);
        // Show titleCurtain! It waits for a tap to begin the minigame.
        titleCurtain.Appear(currMinigame, contestants);
    }
    public void EndCurrMinigame() {
        // Order userPrios after every minigame!
        ud.OrderUserPriosByBattle();
        // Out of minigames! End the round.
        if (CurrRoundData.IsLastMinigame()) {
            EndCurrRound();
        }
        // More minigames? Start the next one!
        else {
            StartNextMinigame();
        }
    }
    
    private void HideAllMinigames() {
        b_endMinigame.gameObject.SetActive(false); // hide DONE for now.
        foreach (Minigame m in allMinigames.Values) { m.Hide(); }
    }
    
    private void StartNextMinigame() {
        CurrRoundData.CurrMinigameIndex ++;
        StartCurrMinigame();
    }
    
    
    private void EndCurrRound() {
        HideAllMinigames();
        SetVisible(false);
        gameController.AdvanceSeqStep();
        // Analytics!
        GameManagers.Instance.AnalyticsManager.OnCompleteMinigameRound(CurrRoundIndex);
    }
    [SerializeField] private UnityEngine.Analytics.AnalyticsEventTracker analyticsTracker=null;


    
    // ----------------------------------------------------------------
    //  Button Events / Doers
    // ----------------------------------------------------------------
    public void OnClick_ReadyForNewRound() {
        newRoundCurtain.Hide();
        StartCurrMinigame();
    }
    public void OnClick_MinigameNext() {
        currMinigame.OnClick_Next();
    }
    public void ShowEndMinigameButton() {
        b_endMinigame.gameObject.SetActive(true);
    }
    
    
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void AddCouple(Contestant cA, Contestant cB) {
        couples.Add(new ContCouple(cA,cB));
    }
    


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        RegisterButtonInput();
    }
    private void RegisterButtonInput() {
        // ~~~~ DEBUG ~~~~
        // CTRL + R = Reload scene
        if (InputController.IsKey_control && Input.GetKeyDown(KeyCode.R)) {
            SceneHelper.ReloadScene();
            return;
        }
        // C = Print Contestants list
        if (Input.GetKeyDown(KeyCode.C)) {
            Debug_PrintContestantsList();
        }
    }



    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //  Debug
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //private void CheckForUpcomingContestantsRequestError() {
    //    // If we've specified the rank for this minigame...
    //    if (CurrRoundData.prioRank!=PrioRank.Any || CurrRoundData.prioRank!=PrioRank.Undefined) {
    //        int numGamesInRound = 0;
    //    }
    //}
    public void Debug_StartPrevMinigame() {
        CurrRoundData.CurrMinigameIndex --;
        StartCurrMinigame();
    }
    public void Debug_SkipCurrMinigame() {
        if (newRoundCurtain.IsVisible) { OnClick_ReadyForNewRound(); }
        else {
            currMinigame.Debug_PickRandOutcome();
            EndCurrMinigame();
        }
    }
    
    
    
    //private void Debug_PrintUpcomingMinigames() {
    //    string str = "Upcoming Minigames ("+upcomingMinigames.Count+"):\n";
    //    for (int i=0; i<upcomingMinigames.Count; i++) {
    //        str += "   " + upcomingMinigames[i];
    //        if (i < upcomingMinigames.Count-1) { str += "\n"; }
    //    }
    //    Debug.Log(str);
    //}
    private void Debug_PrintContestantsList() {
        string str = "Contestants:\n";
        for (int i=0; i<AllContestants.Length; i++) {
            Contestant c = AllContestants[i];
            str += "   " + c + " - won: " + c.myPrio.NumBattlesWon + ", lost: " + c.myPrio.NumBattlesLost + ", tied: " + c.myPrio.NumBattlesTied;
            if (i < AllContestants.Length-1) { str += "\n"; }
        }
        Debug.Log(str);
    }
    
    
    
    
    /*
    private List<string> GetUpcomingMinigames() {
        List<Minigame> options = new List<Minigame>();
        int numContestants = upcomingContestants.Count;
        
        List<string> list = new List<string>();
        while (numContestants > 0) {
            // Out of options? Make more.
            if (options.Count == 0) {
                options = new List<Minigame>(allMinigames.Values);
                options.Shuffle();
                // Avoid repeats: If first option is same as last list item, just REVERSE the whole list.
                if (list.Count>0 && list[list.Count-1]==options[0].name) {//.GetType()) {
                    options.Reverse();
                }
            }
            // Take the first option!
            Minigame option = options[0];
            list.Add(option.name);
            options.RemoveAt(0);
            // Decrement numContestants based on how many this minigame uses.
            numContestants -= option.NumContestants();
        }
        
        // Return!
        return list;
    }
    */
    
    /*
    //private List<Minigame> currMinigames;
    //private int currMinigameIndex; // index in currMinigames list.
    //private int currRoundIndex; // which round it is. Each round includes a battle with every priority.
    
    // ----------------------------------------------------------------
    //  Starting Battles/Rounds
    // ----------------------------------------------------------------
    //private void StartNextMinigameOrRound() {
    //    // Round over? Start next round!
    //    if (currMinigameIndex+1 >= currMinigames.Count) {
    //        SetCurrRound(currRoundIndex+1);
    //    }
    //    // Otherwise, start next battle!
    //    else {
    //        SetCurrBattle(currMinigameIndex+1);
    //    }
    //}
    private void SetCurrRound(int _index) {
        currRoundIndex = _index;
        
        // Order contestants and update percentFullyOrdered.
        allContestants.Shuffle(); // shuffle them FIRST before ordering, so the order isn't predictable.
        allContestants = allContestants.OrderBy(c => c.NumBattlesWon).ToArray<Contestant>();
        UpdatePercentFullyOrdered();
        // Make list of upcoming Battles!
        MakeBattleList();
        
        Debug_PrintContestantsList();
        Debug_PrintBattlesList();
        
        // Start the first battle!
        SetCurrBattle(0);
    }
    private void SetCurrBattle(int _index) {
        currMinigameIndex = _index;
        currBattle = battles[currMinigameIndex];
        
        StartCoroutine(Coroutine_SeqStartBattle());
    }
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnMinigameOver() {
        StartCoroutine(Coroutine_SeqBattleOver(prioButton));
    }
    
    private IEnumerator Coroutine_SeqBattleOver(PrioButton prioButton) {
        // Save outcome and update battleState.
        SaveBattleOutcome(prioButton);
        battleState = BattleStates.PostBattle;
        // Animate buttons out.
        prioButtonA.AnimateBattleOver(prioButton);
        prioButtonB.AnimateBattleOver(prioButton);
        yield return new WaitForSeconds(prioButton==null ? 0.64f : 0.4f); // wait a moment longer if they didn't choose.
        
        // Start next battle or round!
        StartNextMinigameOrRound();
    }
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void MakeBattleList() {
        battles = new List<Battle>();
        // Add regular priorities battles.
        int numRegularBattles = Mathf.CeilToInt(allContestants.Length*0.5f);
        for (int i=0; i<numRegularBattles; i++) {
            int indexA = i*2;
            int indexB = (i*2+1) % allContestants.Length; // pick the next one, OR wrap around to start.
            if (MathUtils.RandomBool()) { // 50% chance to swap the two indexes for variety.
                int temp = indexB;
                indexB = indexA;
                indexA = temp;
            }
            battles.Add(new Battle(allContestants[indexA],allContestants[indexB]));
        }
        // Add some silly options.
        int numSillyBattles = 10;
        for (int i=0; i<numSillyBattles; i++) {
            battles.Add(sillyBattleOptions[sillyBattleIndex]);
            sillyBattleIndex = (sillyBattleIndex+1) % sillyBattleOptions.Length; // increment sillyBattleIndex!
        }
        battles.Shuffle();
    }
    private void SaveBattleOutcome(PrioButton prioButton) {
        // We DID pick a winner!
        if (prioButton != null) {
            Contestant winner = prioButton.MyContestant;
            Contestant loser = OtherContestant(winner);
            winner.NumBattlesWon ++;
            loser.NumBattlesLost ++;
        }
        // We did NOT pick a winner.
        else {
            ContestantA.NumBattlesTied ++;
            ContestantB.NumBattlesTied ++;
        }
    }
    
    
    
    // ----------------------------------------------------------------
    //  Close
    // ----------------------------------------------------------------
    public void Close() {
        SetVisible(false);
    }
    
    
    
    // Debug
    private void Debug_PrintBattlesList() {
        string str = "Battles:\n";
        for (int i=0; i<battles.Count; i++) {
            str += "   " + battles[i].ToString();
            if (i < battles.Count-1) { str += "\n"; }
        }
        Debug.Log(str);
    }
    
    private void OnGUI() {
        // percentFullyOrdered.
        string str = Mathf.Round(percentFullyOrdered*100f) + "%";
        GUIStyle style = new GUIStyle {
            fontSize = 18,
        };
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10,10, 100,100), str, style);
    }
    */


}
