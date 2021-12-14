using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriosFinalRank : BaseViewElement
{
    // Components
    //[SerializeField] private GameObject go_scrim=null;
    //[SerializeField] private GameObject go_popup=null; // contains the prioNames.
    [SerializeField] private RectTransform rt_popup=null; // contains the prioNames.
    [SerializeField] private TextMeshProUGUI[] t_prioNames=null;
    [SerializeField] private TextMeshProUGUI t_header=null;
    [SerializeField] private TextMeshProUGUI t_date=null;
    //[SerializeField] private TextMeshProUGUI t_toggleOpenButton=null;
    // Properties
    private bool isOpen;
    private int numRevealed;

    
    //// ----------------------------------------------------------------
    ////  Start
    //// ----------------------------------------------------------------
    //private void Start() {
    //}


    // ----------------------------------------------------------------
    //  Revealing
    // ----------------------------------------------------------------
    public void Open() {
        SetVisible(true);
        UpdateTexts();
        
        numRevealed = 0; // reset this.
        
        //// Open!
        //SetIsOpen(true);
        // Analytics!
        GameManagers.Instance.AnalyticsManager.OnOpenPriosFinalRank();
    }
    private void UpdateTexts() {
        t_header.text = ud.UserName + "'s Priorities";
        t_date.text = System.DateTime.Now.ToShortDateString();
        for (int i=0; i<t_prioNames.Length; i++) {
            if (i >= NumUserPrios) { Debug.LogWarning("Whoa! Too many FinalRank texts, not enough userPrios!"); break; } // Safety check.
            t_prioNames[i].text = userPrios[i].text;
        }
    }
    public void RevealNextPrio() {
        if (numRevealed<0 || numRevealed>=t_prioNames.Length) { Debug.LogError("Whoa! Revealing prios outta bounds."); return; } // Safety check.
        GameObject prioRow = t_prioNames[t_prioNames.Length-numRevealed-1].gameObject;
        numRevealed ++;
        PriosFinalRankRowCover cover = prioRow.GetComponentInChildren<PriosFinalRankRowCover>();
        cover.gameObject.SetActive(false); // TODO: animate this instead.
    }
    public void ShrinkToMiniPos() {
        LeanTween.value(this.gameObject, SetPopupMiniPosLoc, 0,1, 1.2f).setEaseInOutQuad();
    }
    private void SetPopupMiniPosLoc(float loc) {
        rt_popup.localScale = Vector3.one * Mathf.Lerp(1,0.6f, loc);
        rt_popup.anchoredPosition = Vector2.Lerp(new Vector2(0,220), new Vector2(-112,230), loc);
    }
    public void ShowAtMiniPos() {
        SetVisible(true);
        UpdateTexts();
        foreach (PriosFinalRankRowCover cover in GetComponentsInChildren<PriosFinalRankRowCover>()) {
            cover.gameObject.SetActive(false);
        }
        SetPopupMiniPosLoc(1);
    }
    
    
    //// ----------------------------------------------------------------
    ////  Doers
    //// ----------------------------------------------------------------
    //private void SetIsOpen(bool _isOpen) {
    //    this.isOpen = _isOpen;
    //    go_popup.SetActive(isOpen);
    //    go_scrim.SetActive(isOpen);
    //    t_toggleOpenButton.text = isOpen ? "X" : "#";
    //}
    
    
    //// ----------------------------------------------------------------
    ////  Button Events
    //// ----------------------------------------------------------------
    //public void OnClick_ToggleOpen() {
    //    SetIsOpen(!isOpen);
    //}
    
}
