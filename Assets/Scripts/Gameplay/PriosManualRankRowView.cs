using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PriosManualRankRowView : BaseViewElement, IDragHandler {
    // Enums
    public enum Types { Undefined, Auto,Manual }
    // Components
    [SerializeField] private CanvasGroup myCG=null;
    [SerializeField] private TextMeshProUGUI t_prioName=null;
    // Properties
    private bool IsDragging=false;
    private Types myType;
    // References
    public Priority MyPrio { get; private set; }
    private PriosManualRankView myRankView;
    
    

    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public void Initialize(PriosManualRankView myRankView, Transform tf_parent, Priority myPrio, Types myType) {
        this.myRankView = myRankView;
        this.MyPrio = myPrio;
        this.myType = myType;
        
        // Parent jazz!
        GameUtils.ParentAndReset(this.gameObject, tf_parent);
        this.name = "RowManual " + myPrio.text;
        myRT.offsetMin = new Vector2(20, myRT.offsetMin.y);
        myRT.offsetMax = new Vector2(-20, myRT.offsetMax.y);
        
        // Update visuals
        if (myType == Types.Manual) {
            RevealMyPrioText();
        }
        else {
            t_prioName.text = "???";
        }
    }
    
    

    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void SetRank(int rankIndex) {
        //// Text
        //t_rankValue.text = (rankIndex+1) + ".";
        // Position
        if (!IsDragging) {
            float posY = -rankIndex*PriosManualRankView.RowSpacingY;
            AnimatePosY(posY);
        }
    }
    private void AnimatePosY(float posYTarget) {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, SetPosY, AnchoredPos.y,posYTarget, 0.3f).setEaseOutQuart();
    }
    private void SetPosY(float posY) {
        AnchoredPos = new Vector2(AnchoredPos.x, posY);
    }
    
    public void RevealMyPrioText() {
        t_prioName.text = MyPrio.text;
        t_prioName.alpha = 1;
    }
    
    

    // ----------------------------------------------------------------
    //  UI Events
    // ----------------------------------------------------------------
    public void OnDrag(PointerEventData eventData) {
        if (myType == Types.Manual && !myRankView.DidRevealAnswers) {
            //this.transform.position += new Vector3(0, eventData.delta.y, 0);
            //this.transform.localPosition = new Vector3(0, this.transform.localPosition.y); // lock x pos.
            myRankView.UpdateRanksByYPoses();
        }
    }
    
    public void OnPointerDown() {
        StartDrag();
    }
    public void OnPointerUp() {
        EndDrag();
    }
    
    private void StartDrag() {
        IsDragging = true;
        //mouseYPosOnDown = Input.mousePosition.y;
        myCG.alpha = 0.7f;
        
    }
    private void EndDrag() {
        IsDragging = false;
        myCG.alpha = 1f;
        myRankView.UpdateRanksByYPoses();
    }



    //public void OnClick_Promote() {
    //    myRankView.PromoteRowView(this);
    //}
    //public void OnClick_Demote() {

    //}



}
