using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

/** Got a story to tell? Give it to me. I'll handle it, and tell you what to do. */
public class BranchingStoryController : BaseViewElement {
    // Constants
    System.StringComparison ivc = System.StringComparison.InvariantCulture; // to shorten code below
    // Components
    [SerializeField] private Animator choiceBtnsAnimator=null;
    [SerializeField] private GameController gameController=null;
    [SerializeField] private ChoiceButton[] choiceBtns=null;
    [SerializeField] private GridLayoutGroup choiceBtnLayoutGroup=null;
    [SerializeField] private GameObject tapToContinue=null;
    // Properties
    private bool autoAdvanceAfterSpeech;
    private bool mayClickToNextStep;
    // References
    [SerializeField] private CharViewHandler charViewHandler=null;
    private IStorySource currStorySource; // the CURRENT storySource! Swapped out and stuff.


    //// TEST
    //private List<string> jsonStoryStateSnapshots=new List<string>();
    //public void Debug_RewindStoryOneStep() {
    //    if (jsonStoryStateSnapshots.Count == 0) { return; } // No more to rewind? Do nothing.
    //    string jsonStr = jsonStoryStateSnapshots[jsonStoryStateSnapshots.Count - 1];
    //    currStory.state.LoadJson(jsonStr);
    //}



    
    // Getters (Private)
    private Story currStory { get { return currStorySource?.MyStory; } }
    private MinigameController minigameCont { get { return gameController.MinigameCont; } }
    private string FillInBlanks(string str) {
        // Replace text unique to this IStorySource.
        str = currStorySource.FillInBlanks(str);
        // Replace with DataManager's known replacements!
        return ud.FillInBlanks(str);
    }
    private float GetFloatInParenthesis(string line) {
        int indexLP = line.IndexOf('(');
        int indexRP = line.IndexOf(')');
        if (indexLP==-1 || indexRP==-1) {
            Debug.LogError("Error with function-call in dialogue, missing parenthesis: \"" + line + "\"");
            return 0;
        }
        string floatStr = line.Substring(indexLP+1, indexRP-indexLP-1);
        return float.Parse(floatStr);
    }

    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    override protected void Awake() {
        base.Awake();
        NullifyCurrStory();
        HideChoiceButtons();
        
        // Add event listeners!
        GameManagers.Instance.EventManager.CharFinishedRevealingSpeechTextEvent += OnCharFinishedRevealingSpeechText;
    }
    private void OnDestroy() {
        // Remove event listeners!
        GameManagers.Instance.EventManager.CharFinishedRevealingSpeechTextEvent -= OnCharFinishedRevealingSpeechText;
    }
    
        
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    private void OnCharFinishedRevealingSpeechText() {
        if (currStory == null) { return; } // No story? Do nothin'.
        
        // More dialogue?
        if (currStory.canContinue) {
            // Auto-advance to next step?
            if (autoAdvanceAfterSpeech) {
                AdvanceStory();
            }
            // Otherwise, wait for TAP from user.
            else {
                SetMayClickToNextStep(true);
            }
        }
        // Choice?
        else if (currStory.currentChoices!=null && currStory.currentChoices.Count>0) {
            ShowChoiceBtns();
        }
        // Story OVER?
        else {
            SetMayClickToNextStep(true);
        }
    }
    private void OnStoryComplete() {
        NullifyCurrStory();
        gameController.AdvanceSeqStep();
    }
    
    // ----------------------------------------------------------------
    //  UI Events
    // ----------------------------------------------------------------
    public void OnClick_Choice(int index) {
        currStory.ChooseChoiceIndex(index);
        AdvanceStory();
    }
    
    
    // ----------------------------------------------------------------
    //  Story Doers
    // ----------------------------------------------------------------
    public void SetCurrStorySource(IStorySource storySource) {
        this.currStorySource = storySource;
        AdvanceStory();
    }
    public void NullifyCurrStory() {
        currStorySource = null;
        HideChoiceButtons();
        SetMayClickToNextStep(false);
    }
    
    public void AdvanceStory() {
        SetMayClickToNextStep(false);
        HideChoiceButtons();
        autoAdvanceAfterSpeech = false;
        
        // NO choices? End it!
        if (!currStory.canContinue && (currStory.currentChoices==null || currStory.currentChoices.Count==0)) {
            OnStoryComplete();
        }
        // Choice?!
        else if (!currStory.canContinue) {
            ShowChoiceBtns();
        }
        // Speech!?
        else {
            bool doAdvanceAgainAfterLine = false; // hacky? Confusing?
            
            while (true) {
                //// Save snapshot before advancing.
                //jsonStoryStateSnapshots.Add(currStory.state.ToJson());
                // Advance!
                string line = currStory.Continue();
                if (string.IsNullOrWhiteSpace(line) && !currStory.canContinue) { OnStoryComplete(); return; }
                else if (line.StartsWith("AutoAdvanceAfterSpeech", ivc)) {
                    autoAdvanceAfterSpeech = true;
                }
                else if (line.StartsWith("ShowTapToContinue(", ivc)) {
                    float delay = GetFloatInParenthesis(line);
                    Invoke("SetMayClickToNextStepTrue", delay);
                    break;
                }
                else if (line.StartsWith("Func", ivc)) {
                    line = line.TrimEnd(); // cut any return characters while looking for function names.
                    // What kinda func?
                    if (line.StartsWith("FuncContinue_", ivc)) { // Will do func AFTER user tap, and KEEP going into next Inky line.
                        line = line.Substring(13); // trim the "FuncContinue_" off.
                        doAdvanceAgainAfterLine = true;
                    }
                    else if (line.StartsWith("FuncHalt_", ivc)) { // Will do func AFTER user tap, and STOP until manually told otherwise (usually through an animation).
                        line = line.Substring(9); // trim the "FuncHalt_" off.
                    }
                    //else if (line.StartsWith("FuncImmediate_", ivc)) { // Will do IMMEDIATELY after previous text is done.
                    //    autoAdvanceAfterSpeech = true;
                    //    line = line.Substring(14); // trim the "FuncImmediate_" off.
                    //}
                    else { // Safety check.
                        Debug.LogError("Func string not valid: \"" + line + "\"");
                    }
                    if (line.StartsWith("HideCharViews", ivc)) { charViewHandler.HideAllChars(); }
                    else if (line.StartsWith("HideCharViewBody", ivc)) { charViewHandler.HideCharBody(line.Substring(17)); }
                    else if (line.StartsWith("ShowCharViewBody", ivc)) { charViewHandler.ShowCharBody(line.Substring(17)); }
                    else if (line.StartsWith("HideWorthyMeter", ivc)) { gameController.HideWorthyMeter(); }
                    else if (line.StartsWith("ShowWorthyMeter", ivc)) { gameController.ShowWorthyMeter(); }
                    else if (line.StartsWith("MinigameStepForward", ivc)) { minigameCont.MinigameStepForward(); }
                    else if (line.StartsWith("OpenMinigame", ivc)) { minigameCont.OpenMinigame(line.Substring(13)); }
                    else if (line.StartsWith("PlayVideo", ivc)) { gameController.VideoClipController.PlayVideo(line.Substring(10)); }
                    else if (line.StartsWith("SetBackgroundImage", ivc)) { gameController.SetBackgroundImage(line.Substring(19)); }
                    else if (line.StartsWith("SetWorthyMeterNoun", ivc)) { gameController.SetWorthyMeterNoun(line.Substring(19)); }
                    else if (line.StartsWith("SetWorthyMeterPercentFull", ivc)) {
                        float percent;
                        float.TryParse(line.Substring(26), out percent);
                        gameController.SetWorthyMeterPercentFull(percent);
                    }
                    else { currStorySource.DoFuncFromStory(line); }
                    break;
                }
                else {
                    // Clear everyone's speech by default.
                    charViewHandler.ClearAllCharViewSpeech();
                    line = FillInBlanks(line); // replace things like "[PrioFirst]"!
                    charViewHandler.SetCharTextFromLine(line);
                    break;
                }
                if (!currStory.canContinue) { break; } // Also break if we hit a choice.
            }
            
            if (doAdvanceAgainAfterLine) {
                AdvanceStory();
            }
        }
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void SetMayClickToNextStep(bool val) {
        mayClickToNextStep = val;
        tapToContinue.SetActive(mayClickToNextStep);
    }
    private void SetMayClickToNextStepTrue() { SetMayClickToNextStep(true); }
    private void HideChoiceButtons() {
        choiceBtnsAnimator.Play("ChoiceBtnsHide");
    }
    private void ShowChoiceBtns() {
        choiceBtnLayoutGroup.gameObject.SetActive(true); // in case it was hidden in editor.
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null); // deselect any currently selected buttons.
        foreach (ChoiceButton button in choiceBtns) {
            button.ResetVisuals();
        }
        choiceBtnsAnimator.Play("ChoiceBtnsShow");
        // Set the buttons.
        int numChoices = currStory.currentChoices.Count;
        for (int i=0; i<choiceBtns.Length; i++) {
            if (i < numChoices) {
                string btnText = currStory.currentChoices[i].text;
                choiceBtns[i].SetVisible(true);
                choiceBtns[i].SetText(FillInBlanks(btnText));
            }
            else {
                choiceBtns[i].SetVisible(false);
            }
        }
        // Update numCols, based on if there's text or images.
        if (numChoices > 0) {
            // TODO: If more than 5 options and NO images, make 2, SHORT cols!
            bool areImages = choiceBtns[0].IsImageVisible;
            if (areImages) {
                choiceBtnLayoutGroup.constraintCount = 2;
                choiceBtnLayoutGroup.cellSize = new Vector2(260, 220);
            }
            else {//if (numChoices % 
                //choiceBtnLayoutGroup.cellSize = new Vector2(480, 80);
                choiceBtnLayoutGroup.constraintCount = 1;
                choiceBtnLayoutGroup.cellSize = new Vector2(480, 80);
            }
        }
    }
    
    
        
    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        if (currStory == null) { return; } // Not active? Do nothin'.
        if (mayClickToNextStep && InputController.IsMouseButtonDown()) {
            AdvanceStory();
        }
    }
    
    
}
