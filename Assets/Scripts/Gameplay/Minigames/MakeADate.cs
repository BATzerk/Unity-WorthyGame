using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class MakeADate : Minigame {
        //// Overrides
        //public override int NumContestants() { return 4; }
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Image i_slotA=null;
        [SerializeField] private Image i_slotB=null;
        [SerializeField] private TextMeshProUGUI t_header=null;
        // References
        private ContestantDraggable dateA;
        private ContestantDraggable dateB;


        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            NullifyDateA();
            NullifyDateB();
            foreach (ContestantView cv in ContViews) { cv.Rotation = 0; }//(cv as ContestantDraggable).GoToPosNeutral(); }
            OnChangedDates();
            
            if (minigameCont.couples.Count == 0) {
                t_header.text = "Time to play Cupid!\n\nPlease set up TWO priorites you think make a compatible couple.";
            }
            else {
                t_header.text = "Time to play Cupid again!!\n\nPut TWO priorities on a date. Make a cute couple!";
            }
            // Reset visuals.
            //myAnimator.Play("BeginDate", -1, 0);
            //myAnimator.StopPlayback();//note: doesn't seem to have an effect...
            //myAnimator.Rebind();
            myAnimator.Play("Reset"); // Sigh. Wish I didn't have to have a separate clip just to reset the animation...
        }
        //override public void Begin() {
        //    base.Begin();
        //}
        
        
    
        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        override public void OnBeginDrag_ContDraggable(ContestantDraggable cont) {
            base.OnBeginDrag_ContDraggable(cont);
            // This is dateA or dateB? Null the ref out.
            if (cont == dateA) { NullifyDateA(); }
            if (cont == dateB) { NullifyDateB(); }
        }
        override public void OnEndDrag_ContDraggable(ContestantDraggable cont) {
            base.OnEndDrag_ContDraggable(cont);
            // Dragged into SlotA?
            if (Vector3.Distance(cont.transform.localPosition,i_slotA.transform.localPosition) < 150f) {
                SetDateA(cont);
            }
            else if (Vector3.Distance(cont.transform.localPosition,i_slotB.transform.localPosition) < 150f) {
                SetDateB(cont);
            }
            else {
                cont.AnimateToPosNeutral();
            }
        }
        
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnChangedDates() {
            //b_startDate.gameObject.SetActive(dateA!=null && dateB!=null);
            // Update if we've got ourselves a little date-aroonie!
            if (dateA != null && dateB != null) {
                StartCoroutine(Coroutine_BeginDateSeq());
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetDateA(ContestantDraggable cont) {
            if (dateA != null && dateA!=cont) { // We're usurping another Draggable??
                dateA.AnimateToPosNeutral();
                dateA.Rotation = 0;
            }
            dateA = cont;
            dateA.AnimateToPos(i_slotA.transform.localPosition);
            dateA.Rotation = 80;
            i_slotA.gameObject.SetActive(false);
            OnChangedDates();
        }
        private void SetDateB(ContestantDraggable cont) {
            if (dateB != null && dateB!=cont) { // We're usurping another Draggable??
                dateB.AnimateToPosNeutral();
                dateB.Rotation = 0;
            }
            dateB = cont;
            dateB.AnimateToPos(i_slotB.transform.localPosition);
            dateB.Rotation = -80;
            i_slotB.gameObject.SetActive(false);
            OnChangedDates();
        }
        
        
        private void NullifyDateA() {
            dateA = null;
            i_slotA.gameObject.SetActive(true);
        }
        private void NullifyDateB() {
            dateB = null;
            i_slotB.gameObject.SetActive(true);
        }
        
        
        private IEnumerator Coroutine_BeginDateSeq() {
            // Save values, mon!
            SetOutcome(dateA.MyContestant, dateB.MyContestant);
            minigameCont.AddCouple(dateA.MyContestant, dateB.MyContestant);
            // Change visuals.
            t_header.text = "";
            foreach (ContestantView cv in ContViews) {
                if (cv!=dateA && cv!=dateB) { cv.SetVisible(false); }
            }
            // Animate!
            myAnimator.Play("BeginDate");
            yield return new WaitForSeconds(8f);
            
            // All right, consider this donezo.
            OnMinigameComplete();
        }
        
        
    }
}