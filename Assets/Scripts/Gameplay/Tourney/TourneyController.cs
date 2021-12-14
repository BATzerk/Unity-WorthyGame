using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TourneyNamespace;


public class TourneyController : BaseViewElement {
    // Components
    [SerializeField] private BracketController bracketCont=null;
    [SerializeField] private BattleController battleCont=null;
    // Properties
    public Contestant[] Contestants { get; private set; } // one for each Priority
    public int CurrBattleIndex { get; private set; }
    // References
    //[SerializeField] private GameController gameController=null;
    public Contestant ContestantA { get; private set; } // which one's up to fight next!
    public Contestant ContestantB { get; private set; } // which one's up to fight next!
    
    // Getters (Private)
    private Contestant WhoeverWon(params int[] indexes) {
        foreach (int i in indexes) {
            if (Contestants[i].myStatus == Contestant.Status.Queued) {
                return Contestants[i];
            }
        }
        return Contestant.undefined; // Uh-oh.
    }
    private Contestant GetFinalWinner() {
        for (int i=0; i<Contestants.Length; i++) {
            if (Contestants[i].myStatus == Contestant.Status.Queued) {
                return Contestants[i];
            }
        }
        return null; // Hmm.
    }
    
    
    // ----------------------------------------------------------------
    //  Begin!
    // ----------------------------------------------------------------
    public void Begin() {
        SetVisible(true);
        
        // Make contestants!
        CurrBattleIndex = 0;
        Contestants = new Contestant[NumUserPrios];
        for (int i=0; i<Contestants.Length; i++) {
            Contestants[i] = new Contestant(userPrios[i]);
        }
        // Init things!
        bracketCont.InitViews();
        // Start bracket/battle hidden.
        //bracketCont.SetVisible(false);
        battleCont.SetVisible(false);
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void RevealPrioCandidates() {
        bracketCont.RevealPrioCandidates();
    }
    
    public void ShowUpcomingBattleBrackets() {
        // Update visibilities.
        bracketCont.SetVisible(true);
        battleCont.SetVisible(false);
        // Choose the next contestants!
        SetNextContestants();
        // Update bracket views!
        bracketCont.ShowUpcomingBattleBrackets();
    }
    public void BeginNextBattle() {
        bracketCont.SetVisible(false);
        battleCont.BeginBattle(ContestantA, ContestantB);
    }
    
    private void SetNextContestants() {
        // Hardcoded, nbd.
        switch (CurrBattleIndex) {
            case 0:
                ContestantA = Contestants[1];
                ContestantB = Contestants[2];
                break;
            case 1:
                ContestantA = Contestants[3];
                ContestantB = Contestants[4];
                break;
            case 2:
                ContestantA = Contestants[0];
                ContestantB = WhoeverWon(1, 2);
                break;
            case 3:
                ContestantA = WhoeverWon(3, 4);
                ContestantB = Contestants[5];
                break;
            case 4:
                ContestantA = WhoeverWon(0, 1, 2);
                ContestantB = WhoeverWon(3, 4, 5);
                break;
            default: Debug.LogError("Whoa, currBattleIndex is too high! " + CurrBattleIndex); break;
        }
    }
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnBattleOutcome() {
        CurrBattleIndex ++; // it's the next battle now!
        //StartCoroutine(Coroutine_OnBattleOutcome());
    }
    //private IEnumerator Coroutine_OnBattleOutcome() {
    //    // Wait a little for animations to finish.
        
    //    //// Advance to next step now.
    //    //gameController.AdvanceCurrSeqStep();
        
    //}
    
    
    // ----------------------------------------------------------------
    //  End
    // ----------------------------------------------------------------
    public void FinalResults() {
        // Put winning priority first!
        //dm.MakePrioFirstPriority(GetFinalWinner().myPrio);
        
        // TO DO: order other priorities, too?
        
        
        // Update visuals
        battleCont.SetVisible(false);
        bracketCont.ShowFinalResults();
    }
    public void End() {
        SetVisible(false);
    }
    
    
}
