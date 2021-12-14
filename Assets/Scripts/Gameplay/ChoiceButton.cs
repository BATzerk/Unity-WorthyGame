using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceButton : BaseViewElement {
    // Components
    [SerializeField] private Image myImage=null; // we show either text or an image.
    [SerializeField] private TextMeshProUGUI myText=null; // we show either text or an image.
    [SerializeField] private DepthButtonSer depthButtonSer;
    // Properties
    [SerializeField] private int choiceIndex=0;
    // References
    [SerializeField] private BranchingStoryController storyCont=null;
    
    // Getters (Public)
    public bool IsImageVisible { get { return myImage.enabled; } }
    
    
    // ----------------------------------------------------------------
    //  UI Events
    // ----------------------------------------------------------------
    public void OnClick() {
        storyCont.OnClick_Choice(choiceIndex);
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void ResetVisuals() {
        //depthButtonSer.TODO: This.
        this.GetComponent<Button>().OnPointerExit(null);
    }
    public void SetText(string str) {
        // Image!
        if (str.StartsWith("Img_", System.StringComparison.InvariantCulture)) {
            myImage.enabled = true;
            string imgName = str.Substring(4);
            string imgPath = "Images/ChoiceBtns/" + imgName;
            Sprite sprite = Resources.Load<Sprite>(imgPath);
            if (sprite == null) { Debug.LogError("ChoiceBtn image not found: \"" + imgPath + "\""); } // Safety check.
            myImage.sprite = sprite;
            myText.text = "";
        }
        // Text!
        else {
            myImage.enabled = false;
            myText.text = str;
        }
    }
    
}
