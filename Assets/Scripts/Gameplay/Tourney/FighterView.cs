using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TourneyNamespace {
    public class FighterView : BaseViewElement {
        // Constants
        private int MaxHealth = 4; // how many hits it takes to get to the center of a Tootsie Pop.
        // Components
        //[SerializeField] private Image i_fill=null;
        //[SerializeField] private Image i_stroke=null;
        [SerializeField] private Image i_crossout=null;
        [SerializeField] private Image i_healthFill=null;
        [SerializeField] private TextMeshProUGUI t_prioName=null;
        [SerializeField] private TextMeshProUGUI t_speech=null;
        // Properties
        [SerializeField] private bool isOnLeft=false; // either the left or right fighter.
        private int health;
        private Vector2 barSizeFull;
        // References
        [SerializeField] private BattleController battleCont=null;
        public Contestant MyContestant { get; private set; }
        
        // Getters (Private)
        private Vector3 posOffscreen { get { return new Vector3(700*(isOnLeft?-1:1), myRT.localPosition.y); } }
        private Vector3 posInPosition { get { return new Vector3(250*(isOnLeft?-1:1), myRT.localPosition.y); } }
        
        private string GetLoseSpeech(Priority winningPrio) {
            return "disabled this";
            //string str = winningPrio.trnyOtherLoseSpeech;
            //// It's NOT provided? Ok, make up my own.
            //if (string.IsNullOrEmpty(str)) {
            //    string winPrioName = winningPrio.text;
            //    float rand = Random.Range(0,1f);
            //    if (rand < 0.33f) {
            //        str = winPrioName + "... I guess that IS kinda important...";
            //    }
            //    else if (rand < 0.66f) {
            //        str = "Well... *cough*... at least I was defeated by " + winPrioName + "...";
            //    }
            //    else {
            //        //str = "I guess I'm just not as important as " + winPrioName + "...";
            //        str = "Ugh... I can't believe I lost...";
            //    }
            //}
            //return str;
        }
        private string GetHelloSpeech() {
            return "disabled this";
            //if (MyContestant.myPrio.trnyHellos.Length == 0) {
            //    return battleCont.GetNextGenericFighterHello();
            //}
            //// Otherwise, pick the next hello from the list!
            //string[] hellos = MyContestant.myPrio.trnyHellos;
            //int index = Mathf.Min(hellos.Length-1, MyContestant.NumBattlesWon);
            //return hellos[index];
        }
        private static readonly string[] ouchTexts = { "Ow", "Ow!", "Ouch", "Oof", "Owie", "Unh", "You call that a punch?", "Hrrn", "Hmp!", "Hey watch it", "Ugh!", "Agh", "Ack", };
        private string GetOuchText() {
            return ouchTexts[Random.Range(0,ouchTexts.Length)];
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void Reset(Contestant contestant) {
            this.MyContestant = contestant;
            barSizeFull = i_healthFill.transform.parent.GetComponent<RectTransform>().rect.size;
            t_prioName.text = MyContestant.myPrio.text;
            t_speech.text = "";
            //i_fill.color = dm.GetPrioColor(contestant.myPrio);TO DO: These colors, yo
            i_crossout.enabled = false;
            health = MaxHealth;
            SetHealthBarFillFromHealth(health);
            this.transform.localPosition = posOffscreen;
        }
        
        public void AnimateIn(float delay) {
            LeanTween.cancel(this.gameObject);
            LeanTween.moveLocal(this.gameObject, posInPosition, 0.6f).setEaseOutQuad().setDelay(delay);
            // Set hello text!
            t_speech.text = GetHelloSpeech();
        }
        //public void Appear(float delay) {
        //    SetVisible(true);
        //    CanvasGroup myCanvasGroup = GetComponent<CanvasGroup>();
        //    myCanvasGroup.alpha = 0;
        //    LeanTween.alphaCanvas(myCanvasGroup, 1, 0.4f).setDelay(delay);
        //    this.gameObject.transform.localPosition += new Vector3(-80,0,0);
        //    LeanTween.moveLocalX(this.gameObject, this.gameObject.transform.localPosition.x+80, 0.4f).setEaseOutQuad().setDelay(delay);
        //}
        private void SetHealthBarFillFromHealth(float _health) {
            float fillPercent = _health / (float)MaxHealth;
            GameUtils.SizeUIGraphic(i_healthFill, new Vector2(barSizeFull.x*fillPercent, barSizeFull.y));
        }
        
        private void AnimateHealthBarFill() {
            LeanTween.cancel(this.gameObject);
            LeanTween.value(this.gameObject, SetHealthBarFillFromHealth, health+1, health, 0.3f);
        }
        
        
        public void OnAttack() {
            t_speech.text = "";
        }
        public void TakeDamage(int damage) {
            t_speech.text = GetOuchText();
            
            health -= damage;
            AnimateHealthBarFill();
            if (health <= 0) {
                battleCont.OnFighterDied(this);
            }
        }
        
        public void WinAnimation() {
            //TO DO: Show little happy arms on the PriorityFighter
        }
        public void LoseAnimation(FighterView winner) {
            i_crossout.enabled = true;
            t_speech.text = GetLoseSpeech(winner.MyContestant.myPrio);
        }
        
        
        
    }
}