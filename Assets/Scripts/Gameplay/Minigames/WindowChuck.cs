using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    
    public class WindowChuck : Minigame {
        // Overrides
        override public int NumContestants() { return 3; }
        // Components
        [SerializeField] private Animator myAnimator=null;
        [SerializeField] private GameObject go_loserActor=null; // the "fake" version of the ContView we show for the animation after.
        //[SerializeField] private GameObject go_winnerActor=null; // the "fake" version of the ContView we show for the animation after.
        [SerializeField] private GameObject go_winnerHandA=null;
        [SerializeField] private GameObject go_winnerHandB=null;
        [SerializeField] private TextMeshProUGUI t_mainText=null;
        
        
        
        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Prep(List<Contestant> contestants) {
            base.Prep(contestants);
            t_mainText.text = ReplaceTextVariables("[Cont1] and [Cont2] have teamed up.\n\nThey're bullying [Cont0]!");
            go_winnerHandA.SetActive(false);
            go_winnerHandB.SetActive(false);
            // Hide contestants for now.
            SetContViewsVisible(false);
            myAnimator.Play("Empty", -1, 0);
            
            ShowNextButton();
        }
        override protected void PrepContViews() {
            ContViews[0].Prep(contestants[0]);
            ContViews[1].Prep(contestants[1], contestants[2]);
        }
        
    
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override protected void OnOutOfTime() {
            base.OnOutOfTime();
            t_mainText.text = ReplaceTextVariables("All 3 priorities continue to squabble; because you don't step in, everyone accidentally falls out the window.\n\nNice.");
        }
        
        override public void OnClick_Next() {
            // Show ContViews!
            SetContViewsVisible(true);
            HideNextButton();
            t_mainText.text = ReplaceTextVariables("THROW A GROUP out the window.");
            StartTimer();
        }
        override public void OnEndDrag_ContDraggable(ContestantDraggable cont) {
            base.OnEndDrag_ContDraggable(cont);
            // Dragged to the window??
            if (cont.transform.localPosition.y > -130f) {
                StartCoroutine(Coroutine_ThrowContOutWindow(cont));
            }
            else {
                cont.AnimateToPosNeutral();
            }
        }
        
        
        private IEnumerator Coroutine_ThrowContOutWindow(ContestantView cont) {
            SetOutcomeLoser(cont.MyContestants);
            t_mainText.text = "";
            
            // Play victory!
            cont.SetVisible(false); // hide the REAL draggable.
            go_loserActor.GetComponentInChildren<TextMeshProUGUI>().text = cont.t_PrioName.text;
            //go_winnerActor.GetComponentInChildren<TextMeshProUGUI>().text = cont.t_PrioName.text;
            myAnimator.Play("ThrowOutWindow", -1, 0);
            
            yield return new WaitForSeconds(3.5f);
            if (cont == ContViews[0]) { go_winnerHandB.SetActive(true); }
            else { go_winnerHandA.SetActive(true); }
            
            yield return new WaitForSeconds(1.5f);
            OnMinigameComplete();
        }
        
    }
}