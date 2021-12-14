using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ElimigameNS;

public class ElimigameController : BaseViewElement {
    // Components
    private Elimigame[] elimigames; // set in Awake.
    // Properties
    private Dictionary<string, Elimigame> allElimigames; // GameObject name, Elimigame.
    // References
    [SerializeField] private GameController gameController=null;
    
    // Getters (Public)
    public GameController GameController { get { return gameController; } }
    public BranchingStoryController StoryCont { get { return GameController.StoryCont; } }
    
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    private void Initialize() {
        // Make allMinigames list
        Elimigame[] array = GetComponentsInChildren<Elimigame>(true);
        allElimigames = new Dictionary<string, Elimigame>();
        foreach (Elimigame obj in array) {
            obj.Initialize(this);
            allElimigames.Add(obj.name, obj);
        }
    }
    
    
    // ----------------------------------------------------------------
    //  Open / Close
    // ----------------------------------------------------------------
    public void Close() {
        SetVisible(false);
    }
    public void OpenElimigame(string elimigameName) {
        // Didn't yet init? Init!
        if (allElimigames == null) { Initialize(); }
        SetVisible(true);
        
        // Safety check.
        if (!allElimigames.ContainsKey(elimigameName)) { Debug.LogError("Elimigame with name doesn't exist in scene: " + elimigameName); return; }
        
        // Hide all elimigames.
        foreach (BaseViewElement obj in allElimigames.Values) { obj.SetVisible(false); }
        // Show requested one!
        allElimigames[elimigameName].Open();
        // Set currMinigame!
        //currMinigame = allElimigames[elimigameName];
        //// Pull right number of contestants.
        //List<Contestant> contestants = PullContestantsForMinigame(currMinigame);
        //// Show titleCurtain! It waits for a tap to begin the minigame.
        //titleCurtain.Appear(currMinigame, contestants);
    }
    
    
    public void OnElimigameComplete() {
        //gameController.Test_SetAndShowNextButtonText("DONE");
        gameController.AdvanceSeqStep();
    }
    
    
}
