using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class CaptchaFill : Minigame {
        // Overrides
        public override int NumContestants() { return 2; }
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Button b_submit=null;
        [SerializeField] private InputField myInputField=null;
        [SerializeField] private TextMeshProUGUI t_prioA=null;
        [SerializeField] private TextMeshProUGUI t_prioB=null;
        
        
        // Getters (Private)
        private string CaptchifyStr(string str) {
            //str = str.ToUpper();
            str = str.Replace('A', '4');
            str = str.Replace('a', '4');
            str = str.Replace('B', '8');
            str = str.Replace('b', '8');
            str = str.Replace('E', '3');
            str = str.Replace('e', '3');
            //str = str.Replace('I', 'Y');
            //str = str.Replace('i', 'y');
            str = str.Replace('L', '1');
            str = str.Replace('l', '1');
            str = str.Replace('O', '0');
            str = str.Replace('o', '0');
            //str = str.Replace('S', '$');
            //str = str.Replace('s', '$');
            //str = str.Replace('S', 'Z');
            //str = str.Replace('s', 'z');
            //str = str.Replace('T', '7');
            //str = str.Replace('t', '7');
            return str;
        }
        
        private bool IsCaptchaMatchGreat(string strDesired, string strEntered) {
            return CaptchaMatchPercent(strDesired, strEntered) >= 1; // this one is perfect!!
        }
        private bool IsCaptchaMatchGoodEnough(string strDesired, string strEntered) {
            return CaptchaMatchPercent(strDesired, strEntered) > 0.7f; // ehh, good enough. We know what they meant to type.
        }
        private float CaptchaMatchPercent(string strDesired, string strEntered) {
            strDesired = CaptchifyStr(strDesired).ToUpper();
            strEntered = CaptchifyStr(strEntered).ToUpper();
            strDesired.Replace(" ", "");
            strEntered.Replace(" ", "");
            // How CLOSE a match is this?
            List<char> charsA = new List<char>(strDesired.ToCharArray());
            List<char> charsB = new List<char>(strEntered.ToCharArray());
            int numCharMatches = 0;
            foreach (char c in charsA) {
                if (charsB.Contains(c)) {
                    charsB.Remove(c);
                    numCharMatches ++;
                }
            }
            float percentMatch = numCharMatches / (float)strDesired.Length;
            return percentMatch;
        }



        // ----------------------------------------------------------------
        //  Prep
        // ----------------------------------------------------------------
        public override void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            t_prioA.text = CaptchifyStr(contestants[0].myPrio.text).ToUpper();
            t_prioB.text = CaptchifyStr(contestants[1].myPrio.text).ToLower();
        }


        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public void OnInputFieldChanged() {
            // Does it match a captcha??
            if (IsCaptchaMatchGreat(t_prioA.text, myInputField.text)) {
                OnEnteredGreatCaptcha(contestants[0]);
            }
            else if (IsCaptchaMatchGreat(t_prioB.text, myInputField.text)) {
                OnEnteredGreatCaptcha(contestants[1]);
            }
        }
        public void OnClick_Submit() {
            // TRY to set the outcome, if we've got any good-enough captchas.
            if (!DidSetOutcome) {
                if (IsCaptchaMatchGoodEnough(t_prioA.text, myInputField.text)) {
                    SetOutcome(contestants[0]);
                }
                else if (IsCaptchaMatchGoodEnough(t_prioB.text, myInputField.text)) {
                    SetOutcome(contestants[1]);
                }
            }
            
            // Have we now succeeded setting the outcome? Cool, Beans. Submit.
            if (DidSetOutcome) {
                StartCoroutine(Coroutine_SubmitSeq());
            }
        }
        
        

        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void OnEnteredGreatCaptcha(Contestant cont) {
            SetOutcome(cont);
            myInputField.DeactivateInputField();
            myInputField.interactable = false;
            
            b_submit.interactable = true;
        }
        
        
        private IEnumerator Coroutine_SubmitSeq() {
            myAnimator.Play("Processing");
            yield return new WaitForSeconds(3.5f);
            
            //if (DidSetOutcome) {
                myAnimator.Play("ProcessingComplete");
                yield return new WaitForSeconds(1.8f);
                OnMinigameComplete();
            //}
            //else {
            //    myAnimator.StopPlayback();
            //}
        }
        
        
    }
}