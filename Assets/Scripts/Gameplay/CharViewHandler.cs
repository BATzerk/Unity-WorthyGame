using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharViewHandler : MonoBehaviour {
    // Constants
    System.StringComparison ivc = System.StringComparison.InvariantCulture; // to shorten code below
    // References
    [SerializeField] private List<CharView> charsList=null; // set in Inspector.
    //private Dictionary<Chars, CharView> allChars=null; // a dict for ease of access. made from allCharsList in Awake.


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    //private void Awake() {
    //    allChars = new Dictionary<Chars, CharView>();
    //    foreach (CharView cv in allCharsList) { allChars[cv.MyChar] = cv; }
    //}
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void HideAllChars() {
        foreach (CharView cv in charsList) {
            cv.SetVisible(false);
        }
    }
    public void ClearAllCharViewSpeech() {
        foreach (CharView cv in charsList) {
            cv.ClearSpeechText();
        }
    }
    
    public void SetCharTextFromLine(string line) {
        // Find the CharView who's speaking, based on "Abc: " prefix.
        CharView cv=null;
        if (line.StartsWith("N: ", ivc)) { cv = charsList[0]; } // Narrator!
        else if (line.StartsWith("Astro: ", ivc)) { cv = charsList[1]; } // Astronaut!
        else if (line.StartsWith("PetZooHost: ", ivc)) { cv = charsList[2]; } // Petting-zoo host!
        else if (line.StartsWith("User: ", ivc)) { cv = charsList[3]; } // User!
        // Remove the char name prefix.
        if (cv == null) { // Safety check.
            Debug.LogError("Whoa! CharView prefix not recognized in Inky: \"" + line + "\"");
            cv = charsList[0];
        }
        else {
            line = line.Substring(line.IndexOf(":", ivc)+2); // remove the name specified.
        }
        // Show 'em!!
        cv.SetVisible(true);
        cv.SetSpeechText(line);
    }
    
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnSetCurrStep(SeqStep step) {
        foreach (CharView cv in charsList) {
            bool isChar = step.myChar == cv.MyChar;
            cv.SetVisible(isChar);
            if (isChar) {
                string speechText = GameManagers.Instance.DataManager.UserData.FillInBlanks(step.speechText);
                cv.SetSpeechText(speechText);
                if (step.charImgName != null) {
                    cv.SetBodyImage(step.charImgName);
                }
            }
        }
    }
    
    
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //  Debug
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void Debug_RevealAllCharTexts() {
        foreach (CharView cv in charsList) {
            if (cv.IsVisible) {
                cv.Debug_RevealAllText();
            }
        }
    }
}
