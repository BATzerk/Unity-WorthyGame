using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace.UFOHungryNS {
    public class AbductUI : BaseViewElement {
        // Components
        [SerializeField] private Image i_barFill=null;
        
        
        // Events
        public void OnChangeNumFoodAbducted(int numAbducted, int numTotal) {
            float fillWidthFull = i_barFill.rectTransform.parent.GetComponent<RectTransform>().rect.width;
            float fillWidthA = fillWidthFull * ((numAbducted-1) / (float)numTotal);
            float fillWidthB = fillWidthFull * (numAbducted / (float)numTotal);
            SetFillWidth(fillWidthA);
            LeanTween.value(this.gameObject, SetFillWidth, fillWidthA,fillWidthB, 0.5f).setEaseOutQuint();
            //LeanTween.color(i_barFill.rectTransform, c_barFillGlowy, 0.2f).setDelay(0.6f);
            //LeanTween.color(i_barFill.rectTransform, c_barFillNeutral, 0.4f).setDelay(0.6f + 0.2f).setEaseOutQuad();
        }
        
        // Doers
        private void SetFillWidth(float w) {
            GameUtils.SizeUIGraphic(i_barFill, w,i_barFill.rectTransform.rect.height);
        }
        
        
    }
}