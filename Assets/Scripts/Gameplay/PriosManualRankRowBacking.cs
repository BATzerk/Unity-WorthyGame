using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriosManualRankRowBacking : BaseViewElement {
    // Components
    [SerializeField] private TextMeshProUGUI t_rankValue=null;
    // References
    private PriosManualRankView myRankView;
    

    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public void Initialize(PriosManualRankView myRankView, Transform tf_parent, int rowIndex) {
        this.myRankView = myRankView;
        
        // Parent jazz!
        GameUtils.ParentAndReset(this.gameObject, tf_parent);
        //this.name = "RowBacking " + rowIndex;
        myRT.offsetMin = new Vector2(0, myRT.offsetMin.y);
        myRT.offsetMax = new Vector2(0, myRT.offsetMax.y);
        float posY = -rowIndex*PriosManualRankView.RowSpacingY;
        AnchoredPos = new Vector2(AnchoredPos.x, posY);
        
        // Text
        t_rankValue.text = (rowIndex+1) + ".";
        t_rankValue.enabled = rowIndex < 4; // only show first 4 prio #s.
    }
    
    
    
}
