using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class NewRoundCurtain : BaseViewElement {
        // Components
        [SerializeField] private RectTransform rt_currRoundArrow=null; // the callout arrow pointing to the current round's dot.
        [SerializeField] private TextMeshProUGUI t_percentSorted=null;
        //[SerializeField] private TextMeshProUGUI t_topPrioTitle=null;
        //[SerializeField] private TextMeshProUGUI t_roundName=null;
        private NewRoundCurtainRoundIcon[] roundIcons; // set in Awake.
        // References
        [SerializeField] private MinigameController minigameController=null;
        
        // Getters (Private)
        private int CurrRoundIndex { get { return minigameController.CurrRoundIndex; } }



        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        override protected void Awake() {
            base.Awake();
            List<NewRoundCurtainRoundIcon> list = new List<NewRoundCurtainRoundIcon>(GetComponentsInChildren<NewRoundCurtainRoundIcon>());
            list.Reverse();
            roundIcons = list.ToArray();
        }


        // ----------------------------------------------------------------
        //  Show
        // ----------------------------------------------------------------
        public void Show() {
            SetVisible(true);
            //t_roundName.text = (CurrRoundIndex+1) + " / " + MinigameController.NumTotalRounds;//"ROUND " + 
            //t_roundName.text = "#" + (CurrRoundIndex+1);
            //t_topPrioTitle.text = dm.FillInBlanks("[TopPrioTitlePlain]");
            
            string percentStr = "<color=#FFFFFF>" + (ud.GetUserPriosPercentOrdered()*100) + "%</color>";
            t_percentSorted.text = "Your priorities\nare " + percentStr + " sorted";
            
            // Update roundIcons!
            RectTransform currRoundIconRT=null;
            for (int i=0; i<roundIcons.Length; i++) {
                roundIcons[i].UpdateVisuals(i, CurrRoundIndex);
                if (i == CurrRoundIndex) {
                    currRoundIconRT = roundIcons[i].GetComponent<RectTransform>();
                }
            }
            
            if (currRoundIconRT != null) { // Safety check.
                rt_currRoundArrow.SetParent(currRoundIconRT);
                rt_currRoundArrow.anchoredPosition = new Vector2(0, 0);
            }
        }
        public void Hide() {
            SetVisible(false);
        }
        
    }
}