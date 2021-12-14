using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalRankSample : BaseViewElement {
    // Components
    [SerializeField] private TextMeshProUGUI t_userNameHeader=null;
    [SerializeField] private Image i_mask=null;


    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    //private void Start() {
    //    Hide();
    //}
    
    private void SetMaskPos(Vector2 _pos) {
        i_mask.rectTransform.anchoredPosition = _pos;
    }
    private Vector2 maskPos() { return i_mask.rectTransform.anchoredPosition; }
    

    // ----------------------------------------------------------------
    //  Show / Hide
    // ----------------------------------------------------------------
    public void Show() {
        t_userNameHeader.text = ud.UserName + "'s Priorities";
        SetVisible(true);
        SetMaskPos(new Vector2(0, 0));
        LeanTween.value(this.gameObject, SetMaskPos, maskPos(), new Vector2(220,0), 0.5f).setEaseInOutQuad();
    }
    public void RevealMore() {
        LeanTween.value(this.gameObject, SetMaskPos, maskPos(), new Vector2(600,0), 1f).setEaseInOutQuad();
    }
    public void Hide() {
        SetVisible(false);
    }
    
    
}
