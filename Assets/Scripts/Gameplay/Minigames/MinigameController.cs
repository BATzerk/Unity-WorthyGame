using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinigameNamespace;
using System.Linq;

public class MinigameController : BaseViewElement {
    
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
        // Show titleCurtain! It waits for a tap to begin the minigame.
        titleCurtain.Appear(currMinigame, contestants);
    }
    public void EndCurrMinigame() {
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
    }



    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //  Debug
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
    
    


}
