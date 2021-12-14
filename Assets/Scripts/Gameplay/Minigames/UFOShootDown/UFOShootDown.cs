using MinigameNamespace.UFOShootDownNS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

namespace MinigameNamespace {
    
    public class UFOShootDown : Minigame {
        // Constants
        System.StringComparison ivc = System.StringComparison.InvariantCulture; // to shorten code below
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private CharView cv_assistant=null;
        [SerializeField] private CharView cv_primeMin=null;
        [SerializeField] private CharView cv_primeMinMobile=null;
        [SerializeField] private GameObject go_embassy=null;
        [SerializeField] private GameObject go_phoneDisplay=null;
        [SerializeField] private GameObject go_shootDown=null;
        [SerializeField] private GameObject go_incomingCallUI=null;
        [SerializeField] private GameObject go_choiceBtns=null;
        [SerializeField] private GameObject[] choiceBtns=null;
        [SerializeField] private Image i_blackout=null;
        [SerializeField] private TextMeshProUGUI t_tapToContinue=null;
        [SerializeField] private TextMeshProUGUI t_incomingCallTime=null; // this is really over the top. But I like it!!
        // Properties
        private bool isIncomingCallRinging;
        private bool mayClickToNextStep;
        private Story myStory;
        // References
        [SerializeField] private TextAsset myStoryText=null;
        
        

        // ----------------------------------------------------------------
        //  Awake / Destroy
        // ----------------------------------------------------------------
        override protected void Awake() {
            base.Awake();
            // Add event listeners!
            em.CharFinishedRevealingSpeechTextEvent += OnCharFinishedRevealingSpeechText;
        }
        private void OnDestroy() {
            // Remove event listeners!
            em.CharFinishedRevealingSpeechTextEvent -= OnCharFinishedRevealingSpeechText;
        }
        
        // ----------------------------------------------------------------
        //  Prep / Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            myStory = new Story(myStoryText.text);
            isIncomingCallRinging = false;
            SetMayClickToNextStep(false);
            go_choiceBtns.SetActive(false);
            
            ShowEmbassy();
            
            cv_assistant.SetVisible(false);
            cv_primeMin.SetVisible(false);
        }
        public override void Begin() {
            base.Begin();
            
            NextStep(); // Start us off!
        }
        
        
        
        // ----------------------------------------------------------------
        //  Next Step!
        // ----------------------------------------------------------------
        private void NextStep() {
            isIncomingCallRinging = false;
            SetMayClickToNextStep(false);
            go_choiceBtns.SetActive(false);
            
            // Choice?!
            if (!myStory.canContinue) {
                ShowChoiceBtns();
            }
            // Speech!?
            else {
                // Clear everyone's speech by default.
                cv_assistant.SetSpeechText("");
                cv_primeMin.SetSpeechText("");
                cv_primeMinMobile.SetSpeechText("");
                
                while (true) {
                    string line = myStory.Continue();
                    if (line.StartsWith("FUNC_IncomingCallSeq", ivc)) { StartCoroutine(Coroutine_IncomingCallSeq()); }
                    else if (line.StartsWith("FUNC_ShowEmbassy", ivc)) { ShowEmbassy(); }
                    else if (line.StartsWith("FUNC_ShowShootDown", ivc)) { ShowShootDown(); break; }
                    else if (line.StartsWith("FUNC_HideIncomingCallUI", ivc)) { HideIncomingCallUI(); }
                    else if (line.StartsWith("FUNC_CompleteMinigame", ivc)) { OnMinigameComplete(); }
                    else {
                        SetCharTextFromLine(line);
                        break;
                    }
                    if (!myStory.canContinue) { break; } // Also break if we hit a choice.
                }
            }
        }
        
        private void ShowChoiceBtns() {
            if (go_incomingCallUI.gameObject.activeSelf) { return; } // Hacky. Don't show choice buttons if we've got an incoming call.
            go_choiceBtns.SetActive(true);
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
        }
        
        private void SetCharTextFromLine(string line) {
            CharView cv=null;
            if (line.StartsWith("A: ", ivc)) { cv = cv_assistant; }
            if (line.StartsWith("P: ", ivc)) { cv = cv_primeMin; }
            if (line.StartsWith("PM: ", ivc)) { cv = cv_primeMinMobile; }
            if (cv == null) { Debug.LogError("Whoa! CharView prefix not recognized in Inky: \"" + line + "\""); } // Safety check.
            else {
                line = line.Substring(line.IndexOf(":", ivc)+2); // remove the name specified.
                line = ReplaceTextVariables(line);
                cv.SetVisible(true);
                cv.SetSpeechText(line);
            }
        }
        
        
        
        private IEnumerator Coroutine_IncomingCallSeq() {
            i_blackout.enabled = true;
            yield return new WaitForSeconds(0.7f);
            i_blackout.enabled = false;
            
            isIncomingCallRinging = true; // will be set false when we click call-Accept button.
            go_embassy.SetActive(false);
            go_phoneDisplay.SetActive(true);
            go_incomingCallUI.SetActive(true);
            
            float timeUntilVibrate=0;
            while (true) {
                timeUntilVibrate -= Time.deltaTime;
                if (timeUntilVibrate <= 0) {
                    Handheld.Vibrate();
                    timeUntilVibrate = 1f;
                }
                if (!isIncomingCallRinging) { break; }
                yield return null;
            }
            
            HideIncomingCallUI();
            NextStep();
        }
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetMayClickToNextStep(bool val) {
            mayClickToNextStep = val;
            t_tapToContinue.enabled = mayClickToNextStep;
        }
        private void ShowEmbassy() {
            go_embassy.SetActive(true);
            go_phoneDisplay.SetActive(false);
            go_shootDown.SetActive(false);
        }
        private void ShowShootDown() {
            go_embassy.SetActive(false);
            go_phoneDisplay.SetActive(false);
            go_shootDown.SetActive(true);
            Invoke("AllowShooting", 7f); // allow shooting in a moment.
            myAnimator.enabled = false; // disable animator! So ship can do its movement.
        }
        
        private void HideIncomingCallUI() {
            go_incomingCallUI.SetActive(false);
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnCharFinishedRevealingSpeechText() {
            if (!isActiveAndEnabled) { return; } // Safety check.
            
            if (myStory.canContinue) {
                SetMayClickToNextStep(true);
            }
            else {
                ShowChoiceBtns();
            }
        }
        
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnClick_Choice(int index) {
            myStory.ChooseChoiceIndex(index);
            NextStep();
        }
        
        


        
        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                OnTouchDown();
            }
            // Lol kinda overkill, but nbd for this game.
            t_incomingCallTime.text = System.DateTime.Now.ToString("HH:mm");
        }
        
        private void OnTouchDown() {
            if (mayClickToNextStep) {
                NextStep();
            }
            else if (isShootingDown) {
                if (Time.time > timeWhenMayFireMissile) {
                    FireMissile();
                }
            }
        }
        
        
        
        // Shoot-down Stuff
        [SerializeField] private GameObject go_shootInstructions=null;
        [SerializeField] private RectTransform rt_missiles=null;
        [SerializeField] private Ship ship=null;
        //private bool isShootingDown;
        private float timeWhenMayFireMissile=Mathf.Infinity; // never until further notice.
        private int numMissilesFired;
        // Getters
        private bool isShootingDown { get { return go_shootDown.activeSelf && ship.Health>0; } }
        public Ship Ship { get { return ship; } }
        
        // Doers
        private void AllowShooting() {
            go_shootInstructions.SetActive(true);
            timeWhenMayFireMissile = Time.time;
        }
        private void FireMissile() {
            // Add the thing!
            Missile newObj = Instantiate(rh.UFOShootDown_Missile).GetComponent<Missile>();
            Vector2 cannonTipPos = new Vector2(-6, 445);
            newObj.Initialize(this, rt_missiles, cannonTipPos, numMissilesFired);
            // Update values
            numMissilesFired ++;
            timeWhenMayFireMissile = Time.time + 1f; // don't allow firing for a moment.
        }
        // Events
        public void OnMissileHitShip() {
            ship.GetHit();
            if (ship.Health <= 0) {
                OnShipDefeated();
            }
        }
        private void OnShipDefeated() {
            myAnimator.enabled = true;
            myAnimator.Play("ShipDefeated");
        }



    }
}