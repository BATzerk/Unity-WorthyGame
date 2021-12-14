﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : BaseViewElement {
    // Constants
    [SerializeField] private float BottomUIFullHeight = 80;
    private const int NumTapsToOpen = 4; // num taps in the BOTTOM LEFT of screen.
    // Components
    [SerializeField] private CanvasGroup myCG=null;
    [SerializeField] private RectTransform rt_gameplay=null; // where ALL gameplay elements go!
    [SerializeField] private RectTransform rt_bottomUI=null; // my bottom UI business.
    [SerializeField] private TextMeshProUGUI t_seqAddr=null;
    // References
    [SerializeField] private GameController gameController=null;
    // Properties
    private bool isOpen;
    private float bottomUIOpenLoc;
    private int numTapsUntilOpen;
    
    
    // ----------------------------------------------------------------
    //  Awake / Destroy
    // ----------------------------------------------------------------
    override protected void Awake() {
        base.Awake();
        
        //Open();
        Close();
        SetIsPriosListOpen(false);
        
        // Add event listeners!
        em.SetCurrSeqAddrEvent += OnSetCurrSeqAddr;
        em.SortedUserPriosEvent += OnSortedUserPrios;
    }
    //private void Start() {
        //rt_gameplay.offsetMax = new Vector2(rt_gameplay.offsetMax.x, Screen.safeArea.yMin);// TEST!
    //}
    private void OnDestroy() {
        // Remove event listeners!
        em.SetCurrSeqAddrEvent -= OnSetCurrSeqAddr;
        em.SortedUserPriosEvent -= OnSortedUserPrios;
    }
    
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void ToggleIsOpen() { if (isOpen) { Close(); } else { Open(); } }
    private void Open() {
        numTapsUntilOpen = 0;
        isOpen = true;
        myCG.alpha = 1;
        //myCG.blocksRaycasts = true;
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, SetBottomUIOpenLoc, bottomUIOpenLoc,1, 0.4f).setEaseOutBack();
        // Default open PriosList too.
        SetIsPriosListOpen(true);
    }
    private void Close() {
        numTapsUntilOpen = 0;
        isOpen = false;
        //myCG.alpha = 0;
        //myCG.blocksRaycasts = false;
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, SetBottomUIOpenLoc, bottomUIOpenLoc,0, 0.5f).setEaseOutQuart();
        // Default close PriosList too.
        SetIsPriosListOpen(false);
    }
    
    
    private void SetBottomUIOpenLoc(float val) {
        bottomUIOpenLoc = val;
        float height = bottomUIOpenLoc*BottomUIFullHeight;
        rt_bottomUI.anchoredPosition = new Vector2(0, height-BottomUIFullHeight);//sloppy. = new Vector2(0, height);
        rt_gameplay.offsetMin = new Vector2(rt_gameplay.offsetMin.x, height);
    }
    
    
    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    private void OnSetCurrSeqAddr(SeqAddress addr) {
        t_seqAddr.text = addr.ToString();
    }
    private void OnSortedUserPrios() {
        UpdatePriosListText();
    }
    // ----------------------------------------------------------------
    //  UI Events
    // ----------------------------------------------------------------
    public void OnClick_Close() { Close(); }
    public void OnClick_MainMenu() { SceneHelper.OpenScene(SceneNames.MainMenu); }
    public void OnClick_TogPriosList() { TogglePriosListOpen(); }
    
    public void OnClick_PrevChunk() { gameController.Debug_SetSeqPrevChunk(); }
    public void OnClick_PrevStep() { gameController.Debug_PrevGeneralStep(); }
    public void OnClick_NextStep() { gameController.Debug_NextGeneralStep(); }
    public void OnClick_NextChunk() { gameController.Debug_SetSeqNextChunk(); }
    
    
    
    // ----------------------------------------------------------------
    //  PriosList
    // ----------------------------------------------------------------
    // Components
    [SerializeField] private Image i_priosListScrim=null;
    [SerializeField] private RectTransform rt_priosList=null;
    [SerializeField] private TextMeshProUGUI t_priosList=null;
    // Properties
    private bool isPriosListOpen=false;
    // Events
    public void OnClick_Scrim() { SetIsPriosListOpen(false); }
    // Doers
    private void TogglePriosListOpen() {
        SetIsPriosListOpen(!isPriosListOpen);
    }
    private void SetIsPriosListOpen(bool val) {
        isPriosListOpen = val;
        // Hide/show visuals
        rt_priosList.gameObject.SetActive(isPriosListOpen);
        i_priosListScrim.gameObject.SetActive(isPriosListOpen);
        //gameController.GameTimeCont.SetIsDebugPriosListOpen(isPriosListOpen);
        if (isPriosListOpen) {
            UpdatePriosListText();
        }
    }
    private void UpdatePriosListText() {
        // Update text!
        string str = "";
        //dm.OrderUserPriosByBattle();
        //for (int i=userPrios.Count-1; i>=0; --i) {
        for (int i=0; i<userPrios.Count; i++) {
            Priority p = userPrios[i];
            str += (i+1) + ".  " + p.text;
            str += "   <color=#FFFFFF66>won</color> "+p.NumBattlesWon+", <color=#FFFFFF66>lost</color> "+p.NumBattlesLost;
            if (p.NumBattlesTied > 0) { str += " <color=#FFFFFF66>tied</color> "+p.NumBattlesTied; }
            str += "</color>\n";
        }
        t_priosList.text = str;
    }



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        // Accept input!
        if (InputController.IsKey_control && Input.GetKeyDown(KeyCode.D)) { Open(); }
        if (InputController.IsKey_control && Input.GetKeyUp(KeyCode.D)) { Close(); }
        if (Input.GetMouseButtonDown(0)) {
            OnMouseDown();
        }
    }
    
    private void OnMouseDown() {
        Vector2 mousePos = InputController.Instance.MousePosCanvas;
        if (mousePos.x<120 && mousePos.y<120) {
            if (++numTapsUntilOpen >= NumTapsToOpen) { ToggleIsOpen(); }
        }
        else { numTapsUntilOpen = 0; }
    }
    
    
    
}