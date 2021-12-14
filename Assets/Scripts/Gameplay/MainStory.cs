using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

/** The MAIN, meta-story (i.e. NOT any elimigames).
    Note: Each branching dialogue sequence is in ONE knot. Specifiy the knot name.
*/
public class MainStory : MonoBehaviour, IStorySource {
    // Properties
    private Story myStory;
    // References
    [SerializeField] private BranchingStoryController storyCont=null;
    [SerializeField] private TextAsset myStoryText=null;

    // Interface
    Story IStorySource.MyStory => myStory;
    public void ResetMyStory() {
        myStory = new Story(myStoryText.text);
    }
    bool IStorySource.DoFuncFromStory(string line) {
        Debug.LogWarning("Hmm, no funcs for MainStory provided. Line: \"" + line + "\"");
        return false;
    }
    public string FillInBlanks(string str) { return str; } // MainStory doesn't have any special blanks taht aren't already covered by BranchingStoryController.
    

    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake() {
        ResetMyStory();
    }
    

    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnSetCurrAddr(string knotName) {
        // No knot specified? No story.
        if (string.IsNullOrEmpty(knotName)) {
            //NullifyCurrStory();TODO: Test this. We do need something here, right?
        }
        // YES story! Start it!
        else {
            // Put myStory at requested knot.
            myStory.ChoosePathString(knotName);
            // Pump myStory into storyCont!
            storyCont.SetCurrStorySource(this);
        }
    }
    
    
    
}
