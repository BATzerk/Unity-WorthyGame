using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace {
public class MinigameTimerBar : BaseViewElement {
        // Components
        [SerializeField] private Image i_fill=null;
        // Properties
        private readonly Color c_fillNormal = new Color255(251,209,32).ToColor();
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void UpdateFill(float timeLeft, float percentTimeLeft) {
            // Update MY fill.
            if (i_fill != null) {
                float barWidth = MainCanvas.Width * percentTimeLeft;
                i_fill.rectTransform.sizeDelta = new Vector2(barWidth, 0);
                // Color
                if (timeLeft < 2f) {
                    bool b = MathUtils.Sin01Bool(timeLeft*32+1f);
                    i_fill.color = Color.Lerp(c_fillNormal,Color.red, b ? 0.35f : 0.92f);
                }
                else {
                    i_fill.color = c_fillNormal;
                }
            }
        }
        
    }
}