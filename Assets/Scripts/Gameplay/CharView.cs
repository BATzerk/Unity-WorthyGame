using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// NOTE: Chars types isn't really used (that much) anymore!
public enum Chars { Undefined, Narrator, User, Trny0,Trny1, BlandPopup }


public class CharView : BaseViewElement {
    // Constants
    [SerializeField] public Chars MyChar; // assigned in Inspector.
    // Properties
    private int numCharsRevealed;
    private float timeWhenRevealNextChar; // in SECONDS.
    private string speechTextFull = "";
    // Components
    [SerializeField] private GameObject go_speechBubble=null;
    [SerializeField] private Image i_body=null;
    [SerializeField] private TextMeshProUGUI t_speech=null;
    
    // Getters (Public)
    public bool IsSpeechText { get { return !string.IsNullOrWhiteSpace(speechTextFull); } }
    // Getters (Private)
    private float GetRevealDelayForSpeechChar(string str, int charIndex) {
        // NOT whitespace next? Return MINIMAL delay.
        bool isNextCharNotWhitespace = charIndex < str.Length-1 && !char.IsWhiteSpace(str[charIndex+1]);
        // YES whitespace next! Return maybe minimal, or longer delay.
        switch (str[charIndex]) {
            case ',':
            case '.':
                return 0.5f;
            case '!':
            case '?':
            case ':':
            case ';':
                return isNextCharNotWhitespace ? 0.03f : 0.5f;
            default:
                return 0.03f;
        }
    }


    // ----------------------------------------------------------------
    //  Setters
    // ----------------------------------------------------------------
    public void ClearSpeechText() {
        SetSpeechText("");
    }
    public void SetSpeechText(string str) {
        str = str.Replace("\\n", "\n");
        speechTextFull = str;
        numCharsRevealed = 0;
        timeWhenRevealNextChar = Time.time + 0.1f;
        UpdateSpeechTextDisplay();
        
        // If I have speech, HIDE next-button until I'm done spelling out my speech!
        bool isSpeechText = speechTextFull.Length > 0;
        // If I have NO speech, hide my bubble entirely!
        go_speechBubble.SetActive(isSpeechText);
    }
    public void SetBodyImage(string imgName) {
        i_body.sprite = Resources.Load<Sprite>("Images/CharViews/" + imgName);
    }
    public void SetBodyVisible(bool val) {
        i_body.enabled = val;
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    //private void SetSpeechTextSizeFromStr(string str) {
    //    string oldText = t_speech.text; // rememmer thiz.
    //    t_speech.enableAutoSizing = true;
    //    t_speech.text = str;
    //    t_speech.ForceMeshUpdate();
    //    float fontSize = t_speech.fontSize;
    //    t_speech.enableAutoSizing = false;
    //    t_speech.fontSize = fontSize;
    //    t_speech.text = oldText; // put back what I was sayin' before, just for cleanness.
    //}
    private void UpdateSpeechTextDisplay() {
        string str = speechTextFull.Substring(0, numCharsRevealed);
        str += "<alpha=#00>"; // make all text after this char invisible.
        string latterStr = speechTextFull.Substring(numCharsRevealed);
        latterStr = System.Text.RegularExpressions.Regex.Replace(latterStr, "<(.*?)>", ""); // remove all tags! So we don't interfere with alpha color.
        str += latterStr;
        t_speech.text = str;//speechTextFull.Substring(0, numCharsRevealed);
        
        if (IsSpeechText && numCharsRevealed >= speechTextFull.Length) {
            // Dispatch event!
            em.OnCharFinishedRevealingSpeechText();
        }
    }
    private void RevealNextSpeechChar() {
        char charToReveal = speechTextFull[numCharsRevealed];
        float speedScale = Input.GetMouseButton(0) ? 10 : 1;
        timeWhenRevealNextChar += GetRevealDelayForSpeechChar(speechTextFull, numCharsRevealed) / speedScale;
        numCharsRevealed++;
        //// Is this a "\\n"? Reveal it all at once! So we get the line break in TMPro.
        //if (charToReveal == '\\') {
        //    if (numCharsRevealed < speechTextFull.Length-3) {
        //        char c1 = speechTextFull[numCharsRevealed+1];
        //        char c2 = speechTextFull[numCharsRevealed+2];
        //        if (c1=='\\' && c2=='n') {
        //            numCharsRevealed += 2;
        //        }
        //    }
        //}
        // Is this a '<'? Then reveal ALL THE WAY until the closing >!
        if (charToReveal == '<') {
            while (++numCharsRevealed < speechTextFull.Length) {
                if (speechTextFull[numCharsRevealed] == '>') { // We found the closing bracket!
                    numCharsRevealed ++;
                    break;
                }
            }
        }
        UpdateSpeechTextDisplay();
    }
    public void Debug_RevealAllText() {
        timeWhenRevealNextChar = -1;
        numCharsRevealed = speechTextFull.Length;
        UpdateSpeechTextDisplay();
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    public void Update() {
        UpdateSpeechTextTyping();
    }
    private void UpdateSpeechTextTyping() {
        while (numCharsRevealed < speechTextFull.Length && Time.time >= timeWhenRevealNextChar) {
            RevealNextSpeechChar();
        }
    }

}
