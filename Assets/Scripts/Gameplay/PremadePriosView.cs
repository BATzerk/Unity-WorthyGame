using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PremadePriosView : BaseViewElement {
    // Components
    [SerializeField] private Button b_done=null;
    [SerializeField] private Transform tf_toggles=null;
    [SerializeField] private TextMeshProUGUI t_doneButton=null;
    [SerializeField] private TextMeshProUGUI t_countLeft=null;
    private List<PremadePrioToggle> toggles; // created in Start.
    private PremadePrioToggle[] customTogs; // found in Start.
    // Properties
    private int NumTogsReq;
    private int showIndex; // fragile system, nbd tho.
    // References
    [SerializeField] private GameController gameController=null;

    // Getters (Public)
    //public Priority[] GetInteractablePriosSelected() {
    //    List<Priority> list = new List<Priority>();
    //    for (int i=0; i<toggles.Count; i++) {
    //        if (toggles[i].IsVisible && toggles[i].IsInteractable && toggles[i].isOn) {
    //            list.Add(toggles[i].MyPrio);
    //        }
    //    }
    //    return list.ToArray();
    //}
    public Priority[] GetPriosSelected() {
        List<Priority> list = new List<Priority>();
        for (int i=0; i<toggles.Count; i++) {
            if (toggles[i].IsVisible && toggles[i].isOn) {
                list.Add(toggles[i].MyPrio);
            }
        }
        return list.ToArray();
    }
    

    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    override protected void Awake() {
        base.Awake();
        
        // Make toggles!
        toggles = new List<PremadePrioToggle>();
        // Sloppy: Add pre-existing ones to the list
        customTogs = GetComponentsInChildren<PremadePrioToggle>(true);
        for (int i=0; i<customTogs.Length; i++) {
            customTogs[i].Initialize(this, tf_toggles, new Priority("", true));
            toggles.Add(customTogs[i]);
        }
        // Now instantiate the rest!
        for (int i=0; i<ud.PremadePrios.Length; i++) {
            PremadePrioToggle newObj = Instantiate(ResourcesHandler.Instance.PremadePrioToggle).GetComponent<PremadePrioToggle>();
            newObj.Initialize(this, tf_toggles, ud.PremadePrios[i]);
            toggles.Add(newObj);
        }
        // Put the customTogs at the end of the visual list.
        foreach (PremadePrioToggle tog in customTogs) {
            tog.transform.SetAsLastSibling();
        }

        // Look right!
        UpdateVisuals();
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void Open(int showIndex) {
        this.showIndex = showIndex;
        SetVisible(true);
        switch (showIndex) {
            case 0:
                NumTogsReq = 10;
                break;
            //case 0:
            //    NumTogsReq = 5;
            //    // Hide customTogs.
            //    foreach (PremadePrioToggle tog in customTogs) { tog.gameObject.SetActive(false); }
            //    break;
            //case 1:
                //NumTogsReq = 5; // note: ADDitive! Doesn't include ones already toggled.
                //// Show customTogs.
                //foreach (PremadePrioToggle tog in customTogs) { tog.gameObject.SetActive(true); }
                //// Lock-in ones we've already chosen.
                //foreach (PremadePrioToggle tog in toggles) { tog.HideIfOn(); }
                //break;
                
            default: Debug.LogError("Whoa, called Open for PremadePriosView with unhandled showIndex: " + showIndex); break;
        }
        UpdateVisuals();
    }
    public void UpdateVisuals() {
        // numTogsOn.
        int numTogsOn = 0;
        for (int i=0; i<toggles.Count; i++) {
            if (toggles[i].IsVisible && toggles[i].isOn) {//toggles[i].IsInteractable && 
                numTogsOn++;
            }
        }
        // b_done.
        b_done.interactable = numTogsOn == NumTogsReq;
        // t_doneButton.
        t_doneButton.enabled = numTogsOn == NumTogsReq;
        // t_countLeft.
        if (numTogsOn==NumTogsReq) { t_countLeft.text = ""; }
        else if (numTogsOn < NumTogsReq) { t_countLeft.text = "Pick " + (NumTogsReq-numTogsOn) + " more."; }
        else { t_countLeft.text = "Too many selected. Deselect some."; }
    }


    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnToggleSetIsOn(bool isOn) {
        UpdateVisuals();
    }
    public void OnClick_Done() {
        Priority[] prios = GetPriosSelected();
        // Tell the priorities from whenst they came!
        foreach (Priority p in prios) { p.MySource_PremadePriosShowIndex = showIndex; }
        // Add userPrios!
        ud.AddUserPrios(prios);
        // Neeext.
        gameController.AdvanceSeqStep();
        // Analytics!
        GameManagers.Instance.AnalyticsManager.OnSetInitialUserPrios();
    }



}
