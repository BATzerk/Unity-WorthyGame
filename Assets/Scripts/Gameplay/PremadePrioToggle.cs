using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

//[RequireComponent(typeof(Toggle))
public class PremadePrioToggle : BaseViewElement {
    // Components
    [SerializeField] private Toggle myToggle=null;
    //[SerializeField] private Text myTextUI=null; // HACKY with both of these! Ideally there should only be ONE, a TMPro text. But InputField uses UI Text by default.
    [SerializeField] private TextMeshProUGUI myTextTMPro=null;
    [SerializeField] private InputField myInputField=null; // NOTE: ONLY for custom you-can-type-your-own ones.
    //[SerializeField] private Image i_fill=null;
    //[SerializeField] private Image i_checkmark=null;
    // References
    public Priority MyPrio { get; private set; }
    
    // Getters (Public)
    public bool isOn { get { return myToggle.isOn; } }
    public bool IsInteractable { get { return myToggle.interactable; } }

    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public void Initialize(PremadePriosView premadePriosView, Transform parentTF, Priority priority) {
        GameUtils.ParentAndReset(this.gameObject, parentTF);
        MyPrio = priority;
        if (myInputField!=null) { myInputField.text = MyPrio.text; }
        if (myTextTMPro!=null) { myTextTMPro.text = MyPrio.text; }
        myToggle.onValueChanged.AddListener(premadePriosView.OnToggleSetIsOn);
        UpdateToggleInteractable();
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void HideIfOn() {
        SetVisible(!isOn);
        //if (isOn) {
        //    SetVisible(false);
        //    //myToggle.interactable = false;
        //    //myTextTMPro.color = Color.gray;
        //}
    }
    private void UpdateToggleInteractable() {
        //// HACK to determine if it's a custom one.
        //if (myInputField != null) {
        //    myToggle.interactable = myInputField.text.Length > 0;
        //}
    }
    
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    /// NOTE: This is ONLY called for the custom ones (which have InputFields).
    public void OnInputFieldChanged() {
        // Make sure we don't type in anything that'll break the game.
        string inputStr = RemoveIllegalCharacters(myInputField.text);
        myInputField.text = inputStr;
        // Update MyPrio!
        MyPrio.text = inputStr;
        // By default, toggle me on.
        //if (!myToggle.isOn) {
        //    myToggle.isOn = true;
        //}
        // I'm toggled on if there's text in me.
        myToggle.isOn = !string.IsNullOrWhiteSpace(inputStr);
        // Update toggle interactable.
        UpdateToggleInteractable();
    }
    public void OnToggleChanged() {
        //// HACK to determine if it's a custom one.
        //if (myInputField != null) {
        //    // Oh, our InputField is EMPTY? Ok, deselect toggle and auto-say we're editing the text.
        //    if (myInputField.text.Length == 0) {
        //        myToggle.isOn = false;
        //        myInputField.ActivateInputField();
        //    }
        //}
        // Update visuals.
        //myTextTMPro.color = 
        //i_checkmark.enabled = myToggle.isOn;
        //if (i_fill != null) { // WTF? Why is this null for custom input fields?
        //i_fill.enabled = myToggle.isOn;
        //}
    }
    
    
    private string RemoveIllegalCharacters(string str) {
        int index;
        
        index = str.IndexOf('<');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        index = str.IndexOf('>');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        index = str.IndexOf('[');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        index = str.IndexOf(']');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        index = str.IndexOf('{');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        index = str.IndexOf('}');
        if (index != -1) { return RemoveIllegalCharacters(str.Remove(index, 1)); }
        
        // String looks good, kid!
        return str;
    }
    
    
}
