using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinigameNamespace;
using System.Linq;

public class MinigameController : BaseViewElement {
    // Components
    //[SerializeField] private Button b_endMinigame=null;
    [SerializeField] public Button b_minigameNext=null; // the NEXT button that's shared across Minigames!
    [SerializeField] public ConfettiBursts confettiBursts;
    [SerializeField] public WorthyMeter worthyMeter;
    [SerializeField] private Animator anim_321go;
    // Properties
    private Dictionary<string, Minigame> allMinigames; // GameObject name, Minigame.
    public int CurrMinigameIndex { get; private set; }
    // References
    [SerializeField] private GameController gameController=null;
    [SerializeField] private UnityEngine.Analytics.AnalyticsEventTracker analyticsTracker = null;
    private Minigame currMinigame;



    
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    private void Start() {
        // Make allMinigames list.
        Minigame[] allMinigamesArray = GetComponentsInChildren<Minigame>(true);
        allMinigames = new Dictionary<string, Minigame>();
        foreach (Minigame m in allMinigamesArray) {
            m.Initialize(gameController, this);
            allMinigames.Add(m.name, m);
        }
    }
    
    //public void Open(int _mgIndex) {
    //    // Didn't yet init? Init!
    //    if (allMinigames == null) {
    //        InitializeMinigames();
    //    }
    //    // Show me!
    //    SetVisible(true);
    //    StartMinigame(_name);
    //}
    //public void Close() {
    //    SetVisible(false);
    //}



    // ----------------------------------------------------------------
    //  Start / End Minigames
    // ----------------------------------------------------------------
    public void OpenMinigame(string _name) {
        SetVisible(true);
        //// Set round values.
        //CurrMinigameIndex = _mgIndex;//roundDatas[CurrRoundIndex];
        // Safety check.
        if (!allMinigames.ContainsKey(_name)) { Debug.LogError("Minigame with name doesn't exist in scene: " + _name); return; }
        
        // Hide all minigames
        HideAllMinigames();
        // Set currMinigame!
        currMinigame = allMinigames[_name];
        currMinigame.Open();
        // Update WorthyMeter.
        worthyMeter.SetWorthyNoun(currMinigame.MyWorthyNoun);
    }
    public void EndCurrMinigame() {
        HideAllMinigames();
        SetVisible(false);
        gameController.AdvanceSeqStep();
        // Analytics!
        //GameManagers.Instance.AnalyticsManager.OnCompleteMinigameRound(CurrRoundIndex);
    }
    
    private void HideAllMinigames() {
        //b_endMinigame.gameObject.SetActive(false); // hide DONE for now.
        foreach (Minigame m in allMinigames.Values) { m.Hide(); }
    }

    public void MinigameStepForward() {
        currMinigame?.StepForward();
    }



    // ----------------------------------------------------------------
    //  Button Events / Doers
    // ----------------------------------------------------------------
    public void OnClick_MinigameNext() {
        currMinigame.OnClick_Next();
    }
    public void ShowEndMinigameButton() {
        //b_endMinigame.gameObject.SetActive(true);
    }
    public void PlayAnim_321Go() {
        anim_321go.Play("321Go");
    }
    public void OnCountdownComplete() {
        // Tell Minigame.
        currMinigame.OnCountdownComplete();
        // Animate WorthyMeter to full.
        worthyMeter.AnimateToPercentFull(1f, 0.5f);
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
    //public void Debug_StartPrevMinigame() {
    //    CurrRoundData.CurrMinigameIndex --;
    //    StartCurrMinigame();
    //}
    //public void Debug_SkipCurrMinigame() {
    //    if (newRoundCurtain.IsVisible) { OnClick_ReadyForNewRound(); }
    //    else {
    //        currMinigame.Debug_PickRandOutcome();
    //        EndCurrMinigame();
    //    }
    //}
    
    


}
