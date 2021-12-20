using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorthyMeter : MonoBehaviour {
    // Components
    [SerializeField] private RectTransform rt_fill;
    [SerializeField] private RectTransform myRT;
    [SerializeField] private TextMeshProUGUI t_percentFull;
    [SerializeField] private TextMeshProUGUI t_description;
    // Properties
    private float percentFull;
    private float barWidthFull;
    private Vector2 barPosNeutral;
    private string currWorthyNoun=""; // e.g. "Praise", "Love and Belonging"


    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    private void Start() {
        GameUtils.FlushRectTransform(rt_fill);
        barWidthFull = myRT.rect.width;
        barPosNeutral = this.transform.localPosition;
        SetPercentFull(0);

        Hide();
    }


    // ----------------------------------------------------------------
    //  Show / Hide
    // ----------------------------------------------------------------
    public void Show() {
        this.gameObject.SetActive(true);
    }
    public void Hide() {
        this.gameObject.SetActive(false);
    }
    public void AnimateIn() {
        Show();
        this.gameObject.transform.localPosition = new Vector3(barPosNeutral.x, barPosNeutral.y+80);
        LeanTween.moveLocalY(this.gameObject, barPosNeutral.y, 1f).setEaseOutBack();
    }


    // ----------------------------------------------------------------
    //  Setting Fullness
    // ----------------------------------------------------------------
    public void SetPercentFull(float _val) {
        percentFull = _val;
        UpdateTextAndFill();
    }
    public void SetWorthyNoun(string noun) {
        currWorthyNoun = noun;
        UpdateTextAndFill();
    }
    private void UpdateTextAndFill() {
        string percentageStr = Mathf.Round(percentFull * 100) + "%";
        t_percentFull.text = currWorthyNoun.ToUpper() + " <b>" + percentageStr + "</b>";

        float textWidth = t_percentFull.preferredWidth;
        float fillPosX = barWidthFull * (percentFull - 1);
        float textPosX = Mathf.Clamp(fillPosX-10, -barWidthFull + textWidth+20, -16);
        rt_fill.anchoredPosition = new Vector2(fillPosX, 0);
        t_percentFull.rectTransform.anchoredPosition = new Vector2(textPosX, 0);
        //t_description.text = percentageStr + "<size=20> worthy of </size>" + currWorthyNoun.ToUpper();
    }
    public void AnimateToPercentFull(float _val, float _dur=1f) {
        LeanTween.value(this.gameObject, percentFull,_val, _dur).setOnUpdate(SetPercentFull);
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    public void Update() {
        // DEBUG TESTING
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SetPercentFull(percentFull - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SetPercentFull(percentFull + 0.1f);
        }
    }


}



