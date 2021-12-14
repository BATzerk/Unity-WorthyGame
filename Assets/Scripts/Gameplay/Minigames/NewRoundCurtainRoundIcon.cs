using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class NewRoundCurtainRoundIcon : BaseViewElement {
        // Components
        [SerializeField] private Image i_fill=null;
        [SerializeField] private Image i_checkmark=null;
        [SerializeField] private TextMeshProUGUI t_roundName=null;
        // References
        private Coroutine c_animateColor;
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void UpdateVisuals(int myIndex, int currRoundIndex) {
            if (c_animateColor!=null) { StopCoroutine(c_animateColor); }
            if (myIndex < MinigameController.NumTotalRounds) { // legit round? Set text!
                t_roundName.text = "Round " + (myIndex+1);
            }
            else {
                t_roundName.text = "END";
            }
            
            // I'm a PREVious round?
            if (myIndex < currRoundIndex) {
                SetDiameter(40);
                i_checkmark.enabled = true;
                i_fill.color = new Color(0,0,0, 0.5f);
                t_roundName.color = new Color(0,0,0, 0.5f);//new Color255(195,0,200, 130).ToColor();
            }
            // I'm the CURRent round?
            else if (myIndex == currRoundIndex) {
                SetDiameter(55);
                i_checkmark.enabled = false;
                t_roundName.color = new Color255(231,153,12).ToColor();
                c_animateColor = StartCoroutine(Coroutine_AnimateColorCurrRound());
            }
            // I'm a FUTure round?
            else {
                SetDiameter(40);
                i_checkmark.enabled = false;
                i_fill.color = new Color(1,1,1, 0.3f);
                t_roundName.color = new Color255(195,0,200, 130).ToColor();
            }
        }
        
        private void SetDiameter(float diameter) {
            myRT.sizeDelta = new Vector2(diameter,diameter);
        }
        
        private IEnumerator Coroutine_AnimateColorCurrRound() {
            i_fill.color = new Color(1, 0.7f, 0f);
            //Color cA = new Color(1,0.7f,0f);
            //Color cB = new Color(1,0.7f,0f, 0.4f);
            while (true) {
                LeanTween.alpha(i_fill.rectTransform, 0.4f, 0.4f);
                yield return new WaitForSeconds(0.5f);
                LeanTween.alpha(i_fill.rectTransform, 1f, 0.4f);
                yield return new WaitForSeconds(0.4f);
            }
        }
        
        
    }
}