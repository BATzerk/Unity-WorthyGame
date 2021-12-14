using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TourneyNamespace {
    public class BracketPrioView : BaseViewElement {
        // Components
        [SerializeField] private Animation anim_onDeck=null;
        [SerializeField] private CanvasGroup myCanvasGroup=null;
        [SerializeField] private Image i_fill=null;
        //[SerializeField] private Image i_stroke=null;
        [SerializeField] private Image i_crossout=null;
        [SerializeField] private TextMeshProUGUI t_prioName=null;
        // References
        private Contestant myContestant;
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void InitVisuals(Contestant myContestant) {
            this.myContestant = myContestant;
            t_prioName.text = myContestant.myPrio.text;
            i_crossout.enabled = false;
        }
        public void Appear(float delay) {
            SetVisible(true);
            myCanvasGroup.alpha = 0;
            LeanTween.alphaCanvas(myCanvasGroup, 1, 0.4f).setDelay(delay);
            this.gameObject.transform.localPosition += new Vector3(-80,0,0);
            LeanTween.moveLocalX(this.gameObject, this.gameObject.transform.localPosition.x+80, 0.4f).setEaseOutQuad().setDelay(delay);
        }
        
        
        public void UpdateOnDeckVisuals(TourneyController tc) {
            bool isContestant = myContestant==tc.ContestantA || myContestant==tc.ContestantB;
            
            // Play or stop animation.
            if (isContestant) {
                anim_onDeck.Play();
                myCanvasGroup.alpha = 1;
            }
            else {
                anim_onDeck.Stop();
                myCanvasGroup.alpha = 0.5f;
            }
            
            UpdateEliminatedVisuals();
        }
        private void UpdateEliminatedVisuals() {
            i_crossout.enabled = myContestant.myStatus==Contestant.Status.Eliminated;
            i_fill.enabled = myContestant.myStatus!=Contestant.Status.Eliminated;
        }
        
        public void UpdateVisualsFromFinalResults(TourneyController tc) {
            //UpdateEliminatedVisuals();
            //anim_onDeck.Play();
            //myCanvasGroup.alpha = 1;
            // TEMP: Just do UpdateOnDeckVisuals for now. TO DO: Give the winner a crown!
            UpdateOnDeckVisuals(tc);
        }
        
        
    }
}