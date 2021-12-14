using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserNameEntry : BaseViewElement {
    // Components
    [SerializeField] private Button b_done=null;
    [SerializeField] private InputField myInputField=null;
    // References
    [SerializeField] private GameController gameController=null;
    
    
    
    // ----------------------------------------------------------------
    //  Show!
    // ----------------------------------------------------------------
    public void Show() {
        SetVisible(true);
        myInputField.ActivateInputField();
        UpdateDoneButtonInteractable();
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void UpdateDoneButtonInteractable() {
        b_done.interactable = myInputField.text.Length > 0;
    }
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnInputFieldEndEdit() {
    }
    public void OnInputFieldValueChanged() {
        UpdateDoneButtonInteractable();
    }
    public void OnClick_Done() {
        ud.UserName = myInputField.text;
        gameController.AdvanceSeqStep();
    }
    
    
    
}
