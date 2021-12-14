using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour {
    // Components
    [SerializeField] private Animator randoAnims=null;
    [SerializeField] private CharViewHandler charHandler=null;
    [SerializeField] private GameObject go_nextButton=null;
    [SerializeField] private GameTimeController gameTimeCont=null;
    [SerializeField] private MainStory mainStory=null;
    [SerializeField] private BranchingStoryController storyCont=null;
    [SerializeField] private TextMeshProUGUI t_nextButton=null;
    // Main Views
    [SerializeField] private MinigameController minigameCont=null;
    [SerializeField] private UserNameEntry userNameEntry=null;
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
        minigameCont.SetVisible(false);
        userNameEntry.SetVisible(false);
        
        // Set values in components!
        SetNextButtonText(currStep);
        charHandler.OnSetCurrStep(currStep);
        mainStory.OnSetCurrAddr(currStep.mainStoryKnot);
        
        //// Maybe start minigames!
        //if (currStep.mgRoundData != null) {
        //    OpenMinigames(currStep.mgRoundData);
        //}
        
        // Maybe call function!
        bool mayAutoAdvance = 
            CallSeqFuncByName(currStep.funcToCallName);
        //if (currStep.mgRoundData != null) { mayAutoAdvance = false; } // getting hacky here.
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
            case "SetDidCompleteGameTrue": SetDidCompleteGameTrue(); return true;
            default: Debug.LogWarning("Oops! No switch case to handle func name: " + funcName); return true;
        }
    }
    
    
    
    private void ShowUserNameEntry() { userNameEntry.Show(); }
    private void SetDidCompleteGameTrue() { ud.DidCompleteGame = true; }
    
    //private void OpenMinigames(RoundData roundData) { minigameCont.Open(roundData); }
    //private void CloseMinigames() { minigameCont.Close(); }
    
    
    public void OnClick_Quit() {
        SceneHelper.OpenScene(SceneNames.MainMenu);
    }
    
    
    
    
    
    
    
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //  Debug
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private bool isDebugStepping;
    public void Debug_PrevGeneralStep() {
        Debug_SetSeqPrevStep();
    }
    public void Debug_NextGeneralStep() {
        //if (minigameCont.IsVisible) { minigameCont.Debug_SkipCurrMinigame(); }
        //else
        Debug_SetSeqNextStep();
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






