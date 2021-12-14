using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriosEntryView : BaseViewElement {
    // Constants
    const int NumPriosReq = 5;
    // Components
    [SerializeField] private Button b_done=null;
    [SerializeField] private TextMeshProUGUI t_countLeft=null;
    private InputField[] inputFields; // set in Awake.

    // Getters (Public)
    public string[] GetInputFieldStrings() {
        string[] list = new string[inputFields.Length];
        //string[] list = new string[inputFields.Length];
        for (int i=0; i<inputFields.Length; i++) {
            string fieldStr = inputFields[i].text;
            list[i] = inputFields[i].text;
            //list[i] = dm.GetPriorityFromStr(fieldStr);
            //// It's already a predefined one! Use that.
            //if (dm.GetPriorityFromStr(fieldStr) != null) {
            //    list[i] = dm.GetPriorityFromStr(fieldStr);
            //}
            //else {
            //    list[i] = new Priority(fieldStr);
            //}
        }
        return list;
    }



    // ----------------------------------------------------------------
    //  Awake / Start
    // ----------------------------------------------------------------
    //protected override void Awake() {
    //    base.Awake();
    //}
    //private void Start() {
        //// Make inputFields!
        //inputFields = new InputField[NumPriosReq];
        //float fieldHeight = 36;
        //float gapY = 10;
        //for (int i=0; i<inputFields.Length; i++) {
        //    float posY = 10 + i*(fieldHeight+gapY);
        //    Rect rect = new Rect(10, posY, myRT.rect.width - 20, fieldHeight);
        //    //InputField 
        //    inputFields[i] = new InputField(this, rect);
        //}
    //}


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void Open() {
        // Set inputFields to what userPrios already are!
        inputFields = GetComponentsInChildren<InputField>();
        for (int i=0; i<inputFields.Length&&i<NumUserPrios; i++) {
            inputFields[i].text = userPrios[i].text;
        }
        // Show mee!
        SetVisible(true);
        // Look right.
        UpdateComponentVisuals();
    }
    private void UpdateComponentVisuals() {
        int numFieldsFilled = 0;
        for (int i=0; i<inputFields.Length; i++) {
            if (inputFields[i].text.Length > 0) { numFieldsFilled++; }
        }
        bool filledEnoughFields = numFieldsFilled >= NumPriosReq;
        // b_done
        b_done.interactable = filledEnoughFields;
        // t_countLeft
        t_countLeft.text = filledEnoughFields ? "" : "Add " + (NumPriosReq-numFieldsFilled) + " more.";
    }
    

    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnInputFieldValueChanged() {
        UpdateComponentVisuals();
    }
        
        
        // TextInputField business.
        //if (inputFieldEditing != null) {
        //    if (false) {}
        //    else if (Input.GetKeyDown(KeyCode.UpArrow)) { ChangeInputFieldEditing(-1); }
        //    else if (Input.GetKeyDown(KeyCode.DownArrow)) { ChangeInputFieldEditing(1); }
        //    else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) { SetInputFieldEditing(null); }
        //}


    //void SetInputFieldEditing(InputField tif) {
    //    inputFieldEditing = tif;
    //}
}
