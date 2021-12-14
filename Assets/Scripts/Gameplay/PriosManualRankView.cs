using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PriosManualRankView : BaseViewElement {
    // Constants
    public const float RowSpacingY = 64;
    // Components
    [SerializeField] private Button b_done=null;
    [SerializeField] private Transform tf_rowBackings=null;
    [SerializeField] private Transform tf_rowsAuto=null;
    [SerializeField] private Transform tf_rowsManual=null;
    private PriosManualRankRowBacking[] backings;
    private PriosManualRankRowView[] autoRows;
    private PriosManualRankRowView[] manualRows;
    // Properties
    public bool DidRevealAnswers { get; private set; }
    private List<Priority> priosManualRanked;
    
    

    // ----------------------------------------------------------------
    //  Open / Close
    // ----------------------------------------------------------------
    public void Open() {
        SetVisible(true);
        DidRevealAnswers = false;
        b_done.gameObject.SetActive(false);
        
        // Make priosManualRanked!
        priosManualRanked = new List<Priority>(userPrios);
        priosManualRanked.Shuffle(); // shuffle it to throw off the sceeeent.
        
        // Destroy 'em.
        if (autoRows!=null) {
            foreach (PriosManualRankRowView row in autoRows) { Destroy(row.gameObject); }
        }
        if (manualRows!=null) {
            foreach (PriosManualRankRowView row in manualRows) { Destroy(row.gameObject); }
        }
        
        // Make 'em!
        int NumRows = userPrios.Count;
        // Make rows!
        backings = new PriosManualRankRowBacking[NumRows];
        autoRows = new PriosManualRankRowView[NumRows];
        manualRows = new PriosManualRankRowView[NumRows];
        for (int i=0; i<NumRows; i++) {
            // Backing
            PriosManualRankRowBacking backing = Instantiate(rh.PriosManualRankRowBacking).GetComponent<PriosManualRankRowBacking>();
            backing.Initialize(this, tf_rowBackings, i);
            backings[i] = backing;
            
            // Auto
            PriosManualRankRowView rowAuto = Instantiate(rh.PriosManualRankRowViewAuto).GetComponent<PriosManualRankRowView>();
            rowAuto.Initialize(this, tf_rowsAuto, priosManualRanked[i], PriosManualRankRowView.Types.Auto);
            autoRows[i] = rowAuto;
            autoRows[i].SetRank(i);
            
            // Manual
            PriosManualRankRowView rowManual = Instantiate(rh.PriosManualRankRowViewManual).GetComponent<PriosManualRankRowView>();
            rowManual.Initialize(this, tf_rowsManual, userPrios[i], PriosManualRankRowView.Types.Manual);
            manualRows[i] = rowManual;
        }
        UpdateManualRowVisuals();
    }
    public void Close() {
        SetVisible(false);
    }
    
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void UpdateManualRowVisuals() {
        for (int i=0; i<manualRows.Length; i++) {
            int prioIndex = priosManualRanked.IndexOf(manualRows[i].MyPrio);
            manualRows[i].SetRank(prioIndex);
        }
    }
    
    public void UpdateRanksByYPoses() {
        // REALLY brute-force for now.
        List<PriosManualRankRowView> rowsSorted = new List<PriosManualRankRowView>(manualRows);
        rowsSorted = rowsSorted.OrderByDescending(c => c.AnchoredPos.y).ToList<PriosManualRankRowView>();
        for (int i=0; i<rowsSorted.Count; i++) {
            rowsSorted[i].SetRank(i);
        }
        // Show done button, once they've ranked at least one.
        b_done.gameObject.SetActive(true);
    }
    
    public void RevealAnswers() {
        DidRevealAnswers = true;
        for (int i=0; i<autoRows.Length; i++) {
            autoRows[i].RevealMyPrioText();
        }
        // Hide done button.
        b_done.gameObject.SetActive(false);
    }
    
    
    
}

    
    
    //public void PromoteRowView(PriosManualRankRowView rowView) {
    //    int index = priosManualRanked.IndexOf(rowView.MyPrio);
    //    if (index > 0) {
    //        priosManualRanked.Remove(rowView.MyPrio);
    //        priosManualRanked.Insert(index - 1, rowView.MyPrio);
    //    }
    //    UpdateManualRowVisuals();
    //}