using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class MinigameTitleCurtain : BaseViewElement {
        // Constants
        [SerializeField] private Color c_barFillGlowy=Color.white;
        [SerializeField] private Color c_barFillNeutral=Color.white;
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Image i_barFill=null;
        [SerializeField] private TextMeshProUGUI t_roundName=null;
        [SerializeField] private TextMeshProUGUI t_minigameTitle=null;
        // References
        [SerializeField] private MinigameController minigameCont=null;
        private Minigame currMinigame; // set in Appear.
        // Properties
        private bool isAnimatingOut;
        private float timeWhenAppeared; // in UNSCALED SECONDS.

        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void Appear(Minigame minigame, List<Contestant> contestants) {
            this.currMinigame = minigame;
            isAnimatingOut = false;
            SetVisible(true);
            timeWhenAppeared = Time.unscaledTime;
            // Prep visuals
            t_minigameTitle.text = currMinigame.Title;
            t_roundName.text = "Round " + (minigameCont.CurrRoundIndex+1);
            float fillWidthFull = i_barFill.rectTransform.parent.GetComponent<RectTransform>().rect.width;
            RoundData currRD = minigameCont.CurrRoundData;
            float fillWidthA = fillWidthFull * (currRD.CurrMinigameIndex / (float)currRD.NumMinigames);
            float fillWidthB = fillWidthFull * ((currRD.CurrMinigameIndex+1) / (float)currRD.NumMinigames);
            SetFillWidth(fillWidthA);
            LeanTween.value(this.gameObject, SetFillWidth, fillWidthA,fillWidthB, 0.55f).setDelay(0.6f).setEaseOutQuint();
            LeanTween.color(i_barFill.rectTransform, c_barFillGlowy, 0.2f).setDelay(0.6f);
            LeanTween.color(i_barFill.rectTransform, c_barFillNeutral, 0.4f).setDelay(0.6f + 0.2f).setEaseOutQuad();
            // Prep minigame.
            minigame.Prep(contestants);
            myAnimator.Play("Appear", -1, 0);
        }
        private IEnumerator Coroutine_Disappear() {
            myAnimator.Play("Disappear", -1, 0);
            isAnimatingOut = true;
            
            yield return new WaitForSeconds(0.9f);
            // Begin minigame!
            currMinigame.Begin();
            
            yield return new WaitForSeconds(0.5f);
            SetVisible(false);
            isAnimatingOut = false;
        }
        
        
        private void SetFillWidth(float w) {
            GameUtils.SizeUIGraphic(i_barFill, w,i_barFill.rectTransform.rect.height);
        }


        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnClick_Scrim() {
            if (!isAnimatingOut && Time.unscaledTime > timeWhenAppeared-0.4f) {
                StartCoroutine(Coroutine_Disappear());
            }
        }


    }
}