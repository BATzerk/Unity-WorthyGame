using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class ComfyChair : Minigame {
        // Constants
        private readonly Vector2 slotAPos = new Vector2( 100,160-46);
        private readonly Vector2 slotBPos = new Vector2(-153,162-46);
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private Image i_limbsA=null;
        [SerializeField] private Image i_limbsB=null;
        [SerializeField] private RectTransform rt_contsInChairs=null; // the house when they're in a chair.
        [SerializeField] private RectTransform rt_contsInBottomUI=null; // the house when they're at the bottom UI.
        [SerializeField] private TextMeshProUGUI t_instructsA=null; // "drag one here"
        [SerializeField] private TextMeshProUGUI t_instructsB=null; // "drag one here"
        [SerializeField] private TextMeshProUGUI t_header=null;
        // References
        private ContestantDraggable slotContA; // who's in slotA
        private ContestantDraggable slotContB; // who's in slotB
        
        // Getters (Private)
        private ContestantDraggable OtherContDraggable(ContestantDraggable thisContDrag) {
            if (thisContDrag == ContViews[0]) { return ContViews[1] as ContestantDraggable; }
            return ContViews[0] as ContestantDraggable;
        }


        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            
            ShowNextButton();
            SetContViewsVisible(false);
            SetSlotContA(null);
            SetSlotContB(null);
            OnChangedContsInSlots();
            t_instructsA.gameObject.SetActive(false);
            t_instructsB.gameObject.SetActive(false);
            
            // Reset visuals.
            foreach (ContestantView cv in ContViews) { cv.Rotation = 0; }
            t_header.text = "Two of your friends wanna hang out!\n\nUnfortunately, one of your chairs is extremely uncomfortable.";
            myAnimator.Play("Reset", -1, 0); // Sigh. Wish I didn't have to have a separate clip just to reset the animation...
        }



        // ----------------------------------------------------------------
        //  UI Events
        // ----------------------------------------------------------------
        public override void OnClick_Next() {
            base.OnClick_Next();
            // Change visuals.
            HideNextButton();
            t_header.text = "";//"Please put each friend into the chair it deserves.";
            SetContViewsVisible(true);
            t_instructsA.gameObject.SetActive(true);
            t_instructsB.gameObject.SetActive(true);
            // Start dat timer, Craig!
            StartTimer();
        }
        override public void OnBeginDrag_ContDraggable(ContestantDraggable cont) {
            base.OnBeginDrag_ContDraggable(cont);
            // This is slotContA or slotContB? Null the ref out.
            if (cont == slotContA) { SetSlotContA(null); }
            if (cont == slotContB) { SetSlotContB(null); }
        }
        override public void OnEndDrag_ContDraggable(ContestantDraggable cont) {
            base.OnEndDrag_ContDraggable(cont);
            // Dragged into SlotA?
            if (Vector2.Distance(cont.transform.localPosition,slotAPos) < 150f) {
                SetSlotContA(cont);
            }
            else if (Vector2.Distance(cont.transform.localPosition,slotBPos) < 150f) {
                SetSlotContB(cont);
            }
            else {
                TweenContToBottomUI(cont);
            }
        }
        
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        private void OnChangedContsInSlots() {
            // Update if we've got ourselves a little date-aroonie!
            if (slotContA != null && slotContB != null) {
                StartCoroutine(Coroutine_BeginRideAwaySeq());
            }
        }
        protected override void OnOutOfTime() {
            // We didn't specify ANY slotConts. Make it the usual tie.
            if (slotContA==null && slotContB==null) {
                base.OnOutOfTime();
                SetContViewsVisible(false);
                t_instructsA.gameObject.SetActive(false);
                t_instructsB.gameObject.SetActive(false);
                t_header.text = "You stand there, speechless, unable to decide.\n\nIt is so awkward that your friends leave.";
            }
            // Do we have ONE slotCont? Auto-set the other!
            else if (slotContA!=null) {
                SetSlotContB(OtherContDraggable(slotContA));
            }
            else if (slotContB!=null) {
                SetSlotContA(OtherContDraggable(slotContB));
            }
        }
        

        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void TweenContToBottomUI(ContestantDraggable cont) {
            cont.transform.SetParent(rt_contsInBottomUI);
            cont.Rotation = 0;
            cont.transform.localScale = Vector3.one;
            cont.AnimateToPosNeutral();
        }
        private void SetSlotContA(ContestantDraggable cont) {
            if (cont!=null && slotContA!=null && slotContA!=cont) { // We're usurping another Draggable??
                TweenContToBottomUI(slotContA);
            }
            slotContA = cont;
            if (cont != null) {
                slotContA.AnimateToPos(slotAPos);
                slotContA.transform.SetParent(rt_contsInChairs);
                slotContA.Rotation = 16;
                slotContA.transform.localScale = Vector3.one * 0.9f;
                i_limbsA.gameObject.SetActive(true);
                t_instructsA.gameObject.SetActive(false);
            }
            else {
                i_limbsA.gameObject.SetActive(false);
                t_instructsA.gameObject.SetActive(true);
            }
            OnChangedContsInSlots();
        }
        private void SetSlotContB(ContestantDraggable cont) {
            if (cont!=null && slotContB!=null && slotContB!=cont) { // We're usurping another Draggable??
                TweenContToBottomUI(slotContB);
            }
            slotContB = cont;
            if (cont != null) {
                slotContB.AnimateToPos(slotBPos);
                slotContB.transform.SetParent(rt_contsInChairs);
                slotContB.Rotation = 5;
                slotContB.transform.localScale = Vector3.one * 0.9f;
                i_limbsB.gameObject.SetActive(true);
                t_instructsB.gameObject.SetActive(false);
            }
            else {
                i_limbsB.gameObject.SetActive(false);
                t_instructsB.gameObject.SetActive(true);
            }
            OnChangedContsInSlots();
        }
        
        
        private IEnumerator Coroutine_BeginRideAwaySeq() {
            // Save values, mon!
            SetOutcome(slotContA.MyContestant);
            t_header.text = "";
            
            // Animate!
            myAnimator.Play("RideAway", -1, 0);
            yield return new WaitForSeconds(12.5f);
            
            // All right, consider this donezo.
            OnMinigameComplete();
        }
        
        
    }
}