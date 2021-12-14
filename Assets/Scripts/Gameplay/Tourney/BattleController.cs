using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TourneyNamespace {
    public class BattleController : BaseViewElement {
        // Components
        [SerializeField] private Animation anim_battleOver=null;
        [SerializeField] private Animation anim_readyGoText=null;
        [SerializeField] private Animation anim_aVsB=null; // "A vs. B"
        [SerializeField] private Button b_advanceSeqStep=null; // note: my OWN button.
        [SerializeField] private FighterView fighterA=null;
        [SerializeField] private FighterView fighterB=null;
        [SerializeField] private GameObject go_attackButtons=null;
        [SerializeField] private TextMeshProUGUI t_roundHeader=null;
        [SerializeField] private TextMeshProUGUI t_vsA=null;
        [SerializeField] private TextMeshProUGUI t_vsB=null;
        [SerializeField] private TextMeshProUGUI t_winnerName=null;
        // References
        [SerializeField] private TourneyController tourneyCont=null;
        
        // Getters (Private)
        private FighterView OtherFighter(FighterView fv) {
            return fv==fighterA ? fighterB : fighterA;
        }
        
        // Generic Fighter Hellos
        private int currGenericFighterHelloIndex = 0;
        public string GetNextGenericFighterHello() {
            string str = GenericFighterHelloStrs[currGenericFighterHelloIndex];
            // Loop to the next index for next time.
            currGenericFighterHelloIndex ++;
            if (currGenericFighterHelloIndex >= GenericFighterHelloStrs.Length) { currGenericFighterHelloIndex = 0; }
            return str;
        }
        private readonly string[] GenericFighterHelloStrs = {
            "Prepare to be re-prioritized!",
            "I'm indefatigable!",
            "You're so two-thousand-and-late!",
            "Hit me with your best shot!",
            "You're goin' downnn!",
            "Let's dance!",
            //"1990 called... it wants its... uhh... wait how does it go...",
            "Time for a knuckle sandwich!",
            "I'm gonna bean you!",
            "Why can't we both be winners?",
            //"AAAAA",
            //"AAAAA",
        };
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void BeginBattle(Contestant cA, Contestant cB) {
            SetVisible(true);
            StartCoroutine(Coroutine_BeginBattle(cA,cB));
        }
        private IEnumerator Coroutine_BeginBattle(Contestant cA, Contestant cB) {
            // Hide stuff by default
            go_attackButtons.SetActive(false);
            t_winnerName.gameObject.SetActive(false);
            b_advanceSeqStep.gameObject.SetActive(false);
            // Reset fighters
            fighterA.Reset(cA);
            fighterB.Reset(cB);
            // Update texts
            t_roundHeader.text = "ROUND " + (tourneyCont.CurrBattleIndex+1);
            t_roundHeader.enabled = true;
            t_vsA.text = cA.myPrio.text.ToUpper();
            t_vsB.text = cB.myPrio.text.ToUpper();
            
            yield return new WaitForSeconds(1.2f);
            
            // "A... vs. ... B"
            anim_aVsB.Play();
            yield return new WaitForSeconds(3.8f);
            
            // Animate in fighters
            fighterA.AnimateIn(0);
            fighterB.AnimateIn(0.9f);
            yield return new WaitForSeconds(2.4f);
            
            // "Ready..."
            anim_readyGoText.Play();
            yield return new WaitForSeconds(2.2f);
            
            // Show attack buttons
            go_attackButtons.SetActive(true);
            //anim_fighterA.Play();
            //anim_fighterB.Play();
        }
        
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnFighterDied(FighterView loser) {
            FighterView winner = OtherFighter(loser);
            
            // Hide attack buttons
            go_attackButtons.SetActive(false);
            // Update the contestants!
            loser.MyContestant.SetMyStatus(Contestant.Status.Eliminated);
            winner.MyContestant.NumBattlesWon ++;
            
            // Update fighters
            winner.WinAnimation();
            loser.LoseAnimation(winner);
            
            // Show "GAME!" texts.
            t_winnerName.text = winner.MyContestant.myPrio.text;
            anim_battleOver.Play();
            
            // Show "NEXT" button
            Invoke("ShowAdvanceSeqStepButton", 3.2f);
            
            // Tell TourneyController!
            tourneyCont.OnBattleOutcome();//fighter.MyContestant);
        }
        
        private void ShowAdvanceSeqStepButton() {
            b_advanceSeqStep.gameObject.SetActive(true);
            //LeanTween.cancel(b_advanceSeqStep.gameObject);
            //LeanTween.alpha(b_advanceSeqStep.gameObject, 1
        }
        
        
        // ----------------------------------------------------------------
        //  Button Events
        // ----------------------------------------------------------------
        public void OnClick_AttackA() {
            fighterA.OnAttack();
            fighterB.TakeDamage(1);
        }
        public void OnClick_AttackB() {
            fighterB.OnAttack();
            fighterA.TakeDamage(1);
        }
        
    }
}

