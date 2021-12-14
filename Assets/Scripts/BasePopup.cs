using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupIDs {
    Undefined,

    DeleteSaveData,
    EmailFeedback,
}

abstract public class BasePopup : MonoBehaviour {
    // Overrideable Properties!
    abstract public PopupIDs MyID();
    virtual protected bool DoIgnoreRaycastsWhenClosed() { return true; } // If FALSE, then we WON'T change myCanvasGroup.blocksRaycasts.
    // Components
    [SerializeField] protected CanvasGroup myCanvasGroup=null;
    [SerializeField] protected Image i_scrim=null;
    //[SerializeField] protected RectTransform rt_popup=null;
    // Properties
    public bool IsOpen { get; private set; }
    
    // Getters (Protected)
    protected bool IsGameplayScene { get { return SceneHelper.IsGameplayScene(); } }
    protected DataManager dm { get { return GameManagers.Instance.DataManager; } }
    protected EventManager em { get { return GameManagers.Instance.EventManager; } }


    // ----------------------------------------------------------------
    //  Start / Destroy
    // ----------------------------------------------------------------
    virtual protected void Start() { }
    virtual protected void Awake() {
        Close(0);
        
        //// Add event listeners!
        //em.OpenPopupEvent += OnOpenPopupRequest;
    }
    //private void OnDestroy() {
    //    // Remove event listeners!
    //    em.OpenPopupEvent -= OnOpenPopupRequest;
    //}
    
    //// Events
    //private void OnOpenPopupRequest(PopupIDs _id) {
    //    if (MyID() == _id) {
    //        Open();
    //    }
    //}


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void SetMyCanvasGroupAlpha(float val) {
        myCanvasGroup.alpha = val;
    }
    virtual protected void RefreshVisuals() { }
    
    
    // ----------------------------------------------------------------
    //  Open / Close
    // ----------------------------------------------------------------
    private void SetIsOpen(bool _isOpen) {
        IsOpen = _isOpen;
        i_scrim.enabled = IsOpen;
        if (DoIgnoreRaycastsWhenClosed()) {
            myCanvasGroup.blocksRaycasts = IsOpen;
        }
        //rt_popup.gameObject.SetActive(IsOpen);
        //// Dispatch event!
        //em.OnPopupSetIsOpen(this);
    }

    public void OpenWithDelay(float delay) { Invoke("Open", delay); }
    public void Open() {
        RefreshVisuals();
        SetIsOpen(true);
        AnimateOpen();
    }
    virtual public void Close(float animScale=1) {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject,SetMyCanvasGroupAlpha,1,0, 0.06f*animScale).setOnComplete(OnCompleteClose);
    }
    protected void OnCompleteClose() {
        SetIsOpen(false);
    }

    //protected void OpenPopup(PopupIDs _id, bool doCloseMe=true) {
    //    if (doCloseMe) {
    //        Close();
    //    }
    //    GameManagers.Instance.EventManager.OpenPopup(_id);
    //}


    // ----------------------------------------------------------------
    //  Animating Open / Close
    // ----------------------------------------------------------------
    virtual protected void AnimateOpen() {
        SetMyCanvasGroupAlpha(0);
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, SetMyCanvasGroupAlpha, 0,1, 0.1f);
    }
    
}
