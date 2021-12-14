using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour {
    // Components
    [SerializeField] private Animator randoAnims=null;
    [SerializeField] private CharViewHandler charHandler=null;
    [SerializeField] public  FinalRankSample finalRankSample=null;
    [SerializeField] private GameObject go_nextButton=null;
    [SerializeField] private GameTimeController gameTimeCont=null;
    [SerializeField] private MainStory mainStory=null;
    [SerializeField] private BranchingStoryController storyCont=null;
    [SerializeField] private TextMeshProUGUI t_nextButton=null;
    // Main Views
    [SerializeField] private ElimigameController elimigameCont=null;
    [SerializeField] private MinigameController minigameCont=null;
    [SerializeField] private UserNameEntry userNameEntry=null;
    // OLD views
    [SerializeField] private PremadePriosView premadePrios=null;
    [SerializeField] private PriosEntryView priosEntry=null;
    [SerializeField] private PriosFinalRank priosFinalRank=null;
    [SerializeField] private PriosManualRankView manualRankView=null;
    [SerializeField] private JugglingGame jugglingGame=null;
    [SerializeField] private TourneyController tourneyController=null;
    // References
    private SeqChunk currChunk;
    private SeqStep currStep;

    // Getters (Public)
    public GameTimeController GameTimeCont { get { return gameTimeCont; } }
    public MinigameController MinigameCont { get { return minigameCont; } }
    public BranchingStoryController StoryCont { get { return storyCont; } }
    // Getters (Private)
    private ContentManager cm { get { return GameManagers.Instance.ContentManager; } }
    private DataManager dm { get { return GameManagers.Instance.DataManager; } }
    private EventManager em { get { return GameManagers.Instance.EventManager; } }
    private UserData ud { get { return dm.UserData; } }
    private SeqChapter[] seqChapters { get { return cm.seqChapters; } }
    private List<Priority> userPrios { get { return ud.userPrios; } }


    // ----------------------------------------------------------------
    //  Awake / Destroy
    // ----------------------------------------------------------------
    private void Awake() {
        // Add event listeners!
        em.CharFinishedRevealingSpeechTextEvent += OnCharFinishedRevealingSpeechText;
    }
    private void OnDestroy() {
        // Remove event listeners!
        em.CharFinishedRevealingSpeechTextEvent -= OnCharFinishedRevealingSpeechText;
    }
    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    private void Start () {
        // Start sequence!
        SetCurrAddr(ud.CurrSeqAddr);
        //SetCurrAddr(new SeqAddress(1,0,1));//TEMP not starting at beginning
    }



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
	private void Update () {
		RegisterButtonInput ();
	}
	private void RegisterButtonInput () {
		// ~~~~ DEBUG ~~~~
		// S = Save screenshot
		if (Input.GetKeyDown(KeyCode.S)) {
			ScreenCapture.CaptureScreenshot("screen.png");
		}
        // SHIFT + DELETE = Erase save data
        if (InputController.IsKey_shift && Input.GetKeyDown(KeyCode.Delete)) {
            dm.ClearAllSaveData();
            return;
        }
        // CONTROL + R = Reload scene
        if (InputController.IsKey_control && Input.GetKeyDown(KeyCode.R)) {
            SceneHelper.ReloadScene();
            return;
        }
        
        // S = Slow-mo timeScale
        if (Input.GetKeyDown(KeyCode.S)) { gameTimeCont.SetIsSlowMo(true); }
        if (Input.GetKeyUp(KeyCode.S)) { gameTimeCont.SetIsSlowMo(false); }
        // F = Fast-forward timeScale
        if (Input.GetKeyDown(KeyCode.F)) { gameTimeCont.SetIsFastMo(true); }
        if (Input.GetKeyUp(KeyCode.F)) { gameTimeCont.SetIsFastMo(false); }
        // BRACKET keys to change seq step.
        if (false) {}
        else if (Input.GetKeyDown(KeyCode.LeftBracket) && InputController.IsKey_shift) { Debug_SetSeqPrevChunk(); return; }
        else if (Input.GetKeyDown(KeyCode.RightBracket) && InputController.IsKey_shift) { Debug_SetSeqNextChunk(); return; }
        else if (Input.GetKeyDown(KeyCode.LeftBracket)) { Debug_PrevGeneralStep(); return; }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) { Debug_NextGeneralStep(); return; }
	}



    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnClick_Next() {
        AdvanceSeqStep();
    }
    private void OnCharFinishedRevealingSpeechText() {
        if (!string.IsNullOrEmpty(t_nextButton.text)) { // If the nextButton has text, show it!
            go_nextButton.SetActive(true);
        }
    }
    
    

    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void AdvanceSeqStep() {
        isDebugStepping=false;
        SetCurrAddr(cm.NextStep(ud.CurrSeqAddr));
    }
    private void SetNextButtonText(SeqStep step) {
        t_nextButton.text = step.nextBtnText;
        if (string.IsNullOrEmpty(step.nextBtnText) // If there's no Next-button text, OR...
            || step.speechText.Length > 0) { // ...or there IS speechText (in which case we'll show the button when speech is done being typed)...
            go_nextButton.SetActive(false);
        }
        else {
            go_nextButton.SetActive(true);
        }
    }
    public void ManuallyShowNextButton(string text, float delay=0) {
        StartCoroutine(Coroutine_ShowNextButtonWithDelay(text, delay));
    }
    private IEnumerator Coroutine_ShowNextButtonWithDelay(string text, float delay) {
        go_nextButton.SetActive(false);
        t_nextButton.text = text;
        yield return new WaitForSeconds(delay);
        go_nextButton.SetActive(true);
    }
    
    private void SetCurrAddr(SeqAddress addr) {
        if (addr.chapter >= seqChapters.Length) { // Safety check.
            Debug.Log("Can't advance: No more content!");
            return;
        }
        
        ud.CurrSeqAddr = addr;
        currStep = cm.GetStep(addr);
        
        // Hide/show some things by default.
        elimigameCont.SetVisible(false);
        manualRankView.SetVisible(false);
        //finalRankSample.SetVisible(false);
        //priosFinalRank.SetVisible(false);
        minigameCont.SetVisible(false);
        premadePrios.SetVisible(false);
        priosEntry.SetVisible(false);
        userNameEntry.SetVisible(false);
        //tourneyController.SetVisible(false);TO DO: Something about this visibility stuff. Maybe have current MainView thing, and only that ONE is visible, all others aren't.
        
        // Set values in components!
        SetNextButtonText(currStep);
        charHandler.OnSetCurrStep(currStep);
        mainStory.OnSetCurrAddr(currStep.mainStoryKnot);
        
        // Maybe start minigames!
        if (currStep.mgRoundData != null) {
            OpenMinigames(currStep.mgRoundData);
        }
        
        // Maybe call function!
        bool mayAutoAdvance = 
            CallSeqFuncByName(currStep.funcToCallName);
        if (currStep.mgRoundData != null) { mayAutoAdvance = false; } // getting hacky here.
        if (isDebugStepping) { mayAutoAdvance = false; } // Debug stepping suspends auto-advancing feature.
        
        // We're ok to auto-advance?
        if (mayAutoAdvance) {
            // Convenience: If there's no speech nor button text, AUTO-ADVANCE to next step.
            if (string.IsNullOrEmpty(currStep.speechText)
             && string.IsNullOrEmpty(currStep.nextBtnText)
             && string.IsNullOrEmpty(currStep.mainStoryKnot)) {
                AdvanceSeqStep();
            }
        }
        
        // Dispatch event!
        em.OnSetCurrSeqAddr(ud.CurrSeqAddr);
        
        // Save!!
        dm.SaveUserData();
    }



    // ----------------------------------------------------------------
    //  Special Seq Func Doers
    // ----------------------------------------------------------------
    /** Return TRUE if we should auto-advance to the next step right after calling the function. */
    // #cando: Actually pass func reference instead of a string.
    private bool CallSeqFuncByName(string funcName) {
        if (funcName == null) { return true; } // Ignore null names.
        switch (funcName) {
            case "ShowUserNameEntry": ShowUserNameEntry(); return false;
            case "ShowRateGamePopup": GameUtils.ShowRateGamePopup(); return true;
            //case "ShowFinalRankSample": ShowFinalRankSample(); return false;
            //case "RevealMoreFinalRankSample": RevealMoreFinalRankSample(); return false;
            //case "HideFinalRankSample": HideFinalRankSample(); return true;
            //case "OpenMinigames": OpenMinigames(); return false;
            //case "CloseMinigames": CloseMinigames(); return true;
            case "OpenPremadePriosChoices0": OpenPremadePriosChoices(0); return false;
            case "OpenPremadePriosChoices1": OpenPremadePriosChoices(1); return false;
            case "OpenPriosEntry": OpenPriosEntry(); return false;
            case "ClosePriosEntry": ClosePriosEntry(); return true;
            case "OpenPriosFinalRank": OpenPriosFinalRank(); return false;
            case "RevealPriosFinalRankPrio": RevealPriosFinalRankPrio(); return false;
            case "ShrinkPriosFinalRank": ShrinkPriosFinalRank(); return true;
            case "ShowPriosFinalRankAtMiniPos": priosFinalRank.ShowAtMiniPos(); return true;
            //case "ClosePriosFinalRank": ClosePriosFinalRank(); return true;
            case "OpenPriosManualRankView": OpenPriosManualRankView(); return false;
            case "ClosePriosManualRankView": ClosePriosManualRankView(); return true;
            case "RevealPriosManualRankView": RevealPriosManualRankView(); return false;
            case "OpenElimigame_Murder": OpenElimigame("Murder"); return false;
            case "OpenElimigame_PettingZoo": OpenElimigame("PettingZoo"); return false;
            case "OpenElimigame_SendToSpace": OpenElimigame("SendToSpace"); return false;
            case "OpenElimigame_TestBasic": OpenElimigame("TestBasic"); return false;
            case "OpenElimigame_TheBachelor": OpenElimigame("TheBachelor"); return false;
            case "CloseElimigame": CloseElimigame(); return true;
            case "SetDidCompleteGameTrue": SetDidCompleteGameTrue(); return true;
            case "Debug_SetTestUserPrios": ud.Debug_AddMinimumTestUserPrios(); return true;
            case "Debug_SetTestPriosWins": ud.Debug_SetTestPriosWins(); return true;
            // OLD
            case "StartJugglingGame": StartJugglingGame(); return false;
            case "StopJugglingGame": StopJugglingGame(); return true;
            case "ShowTourneyTimeWatchAnim": ShowTourneyTimeWatchAnim(); return false;
            case "BeginTourneySequence": BeginTourneySequence(); return true;
            case "Trny_RevealPrioCandidates": tourneyController.RevealPrioCandidates(); return true;
            case "Trny_ShowUpcomingBattleBrackets": tourneyController.ShowUpcomingBattleBrackets(); return true;
            case "Trny_BeginNextBattle": tourneyController.BeginNextBattle(); return false;
            case "Trny_RevealFinalResults": tourneyController.FinalResults(); return false;
            case "EndTourneySequence": EndTourneySequence(); return true;
            default: Debug.LogWarning("Oops! No switch case to handle func name: " + funcName); return true;
        }
    }
    
    
    
    private void ShowUserNameEntry() { userNameEntry.Show(); }
    private void SetDidCompleteGameTrue() { ud.DidCompleteGame = true; }
    
    private void OpenMinigames(RoundData roundData) { minigameCont.Open(roundData); }
    //private void CloseMinigames() { minigameCont.Close(); }
    
    private void OpenPriosEntry() {
        charHandler.HideAllChars();
        priosEntry.Open();
    }
    private void ClosePriosEntry() {
        ud.AddNewPriosFromStrings(priosEntry.GetInputFieldStrings());
    }
    
    private void OpenPremadePriosChoices(int showIndex) {
        charHandler.HideAllChars();
        premadePrios.Open(showIndex);
    }
    
    private void OpenPriosFinalRank() { priosFinalRank.Open(); }
    private void RevealPriosFinalRankPrio() { priosFinalRank.RevealNextPrio(); }
    private void ShrinkPriosFinalRank() { priosFinalRank.ShrinkToMiniPos(); }
    
    private void OpenPriosManualRankView() { manualRankView.Open(); }
    private void ClosePriosManualRankView() { manualRankView.Close(); }
    private void RevealPriosManualRankView() {
        manualRankView.SetVisible(true); // hack! Temp.
        manualRankView.RevealAnswers();
    }
    
    private void CloseElimigame() {
        elimigameCont.Close();
    }
    private void OpenElimigame(string elimigameName) {
        elimigameCont.OpenElimigame(elimigameName);
    }
    
    
    
    private void StartJugglingGame() {
        charHandler.HideAllChars();
        jugglingGame.Restart();
    }
    private void StopJugglingGame() {
        jugglingGame.Close();
    }
    
    private void BeginTourneySequence() {
        tourneyController.Begin();
    }
    private void EndTourneySequence() {
        tourneyController.End();
    }
    
    
    private void ShowTourneyTimeWatchAnim() { randoAnims.Play("TourneyTimeWatch"); }
    
    public void OnClick_Quit() {
        SceneHelper.OpenScene(SceneNames.MainMenu);
    }
    
    
    
    
    
    
    
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //  Debug
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private bool isDebugStepping;
    public void Debug_PrevGeneralStep() { // Either affects MinigameController or my seq stuff.
        if (minigameCont.IsVisible) {
            if (minigameCont.CurrRoundData.CurrMinigameIndex<=0) { Debug_SetSeqPrevStep(); } // FIRST minigame? Go to prev seq-step, then.
            else { minigameCont.Debug_StartPrevMinigame(); } // Otherwise, prev minigame!
        }
        else { Debug_SetSeqPrevStep(); }
    }
    public void Debug_NextGeneralStep() { // Either affects MinigameController or my seq stuff.
        if (minigameCont.IsVisible) { minigameCont.Debug_SkipCurrMinigame(); }
        else { Debug_SetSeqNextStep(); }
    }
    
    
    private void Debug_SetSeqPrevStep() { isDebugStepping=true; Debug_SetCurrAddrAndSkipText(cm.PrevStep(ud.CurrSeqAddr)); }
    private void Debug_SetSeqNextStep() { isDebugStepping=true; Debug_SetCurrAddrAndSkipText(cm.NextStep(ud.CurrSeqAddr)); }
    public void Debug_SetSeqPrevChunk() { isDebugStepping=true; Debug_SetCurrAddrAndSkipText(cm.PrevChunk(ud.CurrSeqAddr)); }
    //private void Debug_SetSeqNextChunk() { Debug_SetCurrAddrAndSkipText(cm.NextChunk(currSeqAddr)); }
    public void Debug_SetSeqNextChunk() {
        isDebugStepping=true;
        SeqAddress targetAddr = cm.NextChunk(ud.CurrSeqAddr);
        int safetyCount=0;
        while (ud.CurrSeqAddr < targetAddr) {
            Debug_NextGeneralStep();
            //print(Time.frameCount + "  Debug advancing: " + currSeqAddr);
            if (safetyCount++>99999) { Debug.LogError("Oops! Caught in infinite loop in Debug_SetSeqNextChunk."); break; }
        }
    }
    
    
    private void Debug_SetCurrAddrAndSkipText(SeqAddress addr) {
        storyCont.NullifyCurrStory();
        SetCurrAddr(addr);
        charHandler.Debug_RevealAllCharTexts();
    }



}






