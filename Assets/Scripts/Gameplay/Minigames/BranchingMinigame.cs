using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class BranchingMinigame : Minigame {
        //// Overrides
        //override public int NumContestants() { return 1; }
        // Components
        private Button[] choiceBtns; // set in Prep.
        [SerializeField] private CharView prioCharView=null;
        [SerializeField] private GameObject go_choiceBtns=null;
        // Properties
        private Story myStory;
        // References
        [SerializeField] private TextAsset myStoryText=null;
        
        

        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        override protected void Awake() {
            base.Awake();
            // Add event listeners!
            GameManagers.Instance.EventManager.CharFinishedRevealingSpeechTextEvent += OnCharFinishedRevealingSpeechText;
        }
        private void OnDestroy() {
            // Remove event listeners!
            GameManagers.Instance.EventManager.CharFinishedRevealingSpeechTextEvent -= OnCharFinishedRevealingSpeechText;
        }
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            choiceBtns = GetComponentsInChildren<Button>(true);
            myStory = new Story(myStoryText.text);
            SetChoiceBtnsVisible(false);
            prioCharView.SetVisible(false);
            
            base.Prep(contestants);
        }
        override public void Begin() {
            base.Begin();
            
            prioCharView.SetVisible(true);
            AdvanceStory();
        }
        
        // ----------------------------------------------------------------
        //  Advance Story
        // ----------------------------------------------------------------
        private void AdvanceStory() {
            SetChoiceBtnsVisible(false);
            
            // Set the char text!
            string speechText="";
            while (myStory.canContinue) {
                string line = myStory.Continue();
                //if (line == "[SetCont0Winner]\n") {
                //    SetOutcome(contestants[0]);
                //}
                //else if (line == "[SetCont0Loser]\n") {
                //    SetOutcomeLoser(contestants[0]);
                //}
                //else {
                    speechText += ReplaceTextVariables(line);
                //}
            }
            
            prioCharView.SetSpeechText(speechText);
            
            // Set the buttons.
            int numChoices = myStory.currentChoices.Count;
            for (int i=0; i<choiceBtns.Length; i++) {
                if (i < numChoices) {
                    string btnText = myStory.currentChoices[i].text;
                    choiceBtns[i].gameObject.SetActive(true);
                    choiceBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = btnText;
                }
                else {
                    choiceBtns[i].gameObject.SetActive(false);
                }
            }
            //var outcomeVar = myStory.variablesState["outcome"];
            //print("Outcome var: " + outcomeVar);
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetChoiceBtnsVisible(bool val) {
            go_choiceBtns.SetActive(val);
        }
        private void SetOutcomeFromStoryVariable() {
            var outcomeVar_ = myStory.variablesState["outcome"];
            float outcomeVar = float.Parse(outcomeVar_.ToString()); // AWKWARD hacky workaround. Idk why I can't cast directly to float. :P
            Priority prio = contestants[0].myPrio;
            if (outcomeVar > 0) { prio.NumBattlesWon += outcomeVar; }
            else if (outcomeVar < 0) { prio.NumBattlesLost -= outcomeVar; }
            else { Debug.LogError("Whoa, outcome var is 0. :P"); }
            // 
            //OnSetOutcome();
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnCharFinishedRevealingSpeechText() {
            if (!this.gameObject.activeSelf || myStory == null) { return; } // Safety check.
            if (!myStory.canContinue && myStory.currentChoices!=null && myStory.currentChoices.Count==0) {
                SetOutcomeFromStoryVariable();
                OnMinigameComplete();
            }
            else {
                SetChoiceBtnsVisible(true);
            }
        }
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnClick_Choice(int choiceIndex) {
            myStory.ChooseChoiceIndex(choiceIndex);
            AdvanceStory();
        }
        
        
    }
}