using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class KnifeToMurder : Minigame {
        // Overrides
        public override int NumContestants() { return 2; }
        // Components
        [SerializeField] private GameObject go_inkSplatter=null;
        [SerializeField] private Image i_knife=null;
        [SerializeField] private Image i_blackout=null;
        [SerializeField] private TextMeshProUGUI t_header=null;
        [SerializeField] private KnifeToMurderContView contA=null;
        [SerializeField] private KnifeToMurderContView contB=null;
        // Properties
        private readonly Vector3 knifePosNeutral = new Vector3(0, 60, 0);
        
        // Getters / Setters (Private)
        private Vector2 KnifePos {
            get { return i_knife.rectTransform.anchoredPosition; }
            set { i_knife.rectTransform.anchoredPosition = value; }
        }
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            // Update contestants!
            contA.Prep(contestants[0]);
            contB.Prep(contestants[1]);
            
            ShowNextButton("OK...?");
            i_blackout.enabled = false;
            go_inkSplatter.SetActive(false);
            i_knife.gameObject.SetActive(false);
            t_header.text = "It's Murder Time!";
        }
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        //    SetOutcome(contButt.MyContestant);
        //    t_header.text = "You purchase " + WinnerNameStylized + ". It tastes like freedom.\n\nYou spend the remaining $0.99 on hookers and cocaine.";
            
        //    OnMinigameComplete();
        //}
        
        override public void OnClick_Next() {
            HideNextButton();
            t_header.text = "Please give this KNIFE to one so they can murder the other.";
            // Show knife and start animations!
            i_knife.gameObject.SetActive(true);
            KnifePos = knifePosNeutral;
            contA.PlayAnim_HandOpenClose();
            contB.PlayAnim_HandOpenClose();
        }
        
        
        public void OnStopDraggingKnife() {
            //print("KnifePos.x: " + KnifePos.x);
            // Give to a Contestant!
            if (KnifePos.y>180 && Mathf.Abs(KnifePos.x)>40) {
                SetMurderer(contestants[KnifePos.x<0 ? 0 : 1]);
            }
            // Reset its position.
            else {
                KnifePos = knifePosNeutral;
            }
        }
        
        private void SetMurderer(Contestant murderer) {
            SetOutcome(murderer);
            
            t_header.text = "";
            i_knife.gameObject.SetActive(false);
            
            if (murderer == contestants[0]) { StartCoroutine(Coroutine_MurderSeq(contA, contB)); }
            else { StartCoroutine(Coroutine_MurderSeq(contB, contA)); }
        }
        
        private IEnumerator Coroutine_MurderSeq(KnifeToMurderContView murderer, KnifeToMurderContView victim) {
            bool isMurdererOnLeft = murderer == contA;
            murderer.SetVisualsAsMurderer();
            victim.SetVisualsAsVictim();
            
            yield return new WaitForSeconds(1.2f);
            //myAnimator.Play("Murder");
            
            i_blackout.enabled = true;
            yield return new WaitForSeconds(2f);
            
            i_blackout.enabled = false;
            go_inkSplatter.SetActive(true);
            go_inkSplatter.transform.localScale = new Vector3(isMurdererOnLeft ? 1:-1, 1,1);
            
            murderer.StopAnimations();
            victim.StopAnimations();
            
            murderer.PlayAnim("ThumbsUp");
            victim.SetVisible(false);
            
            t_header.text = ReplaceTextVariables("It has been done.");
            
            yield return new WaitForSeconds(3f);
            OnMinigameComplete();
        }
    }
}