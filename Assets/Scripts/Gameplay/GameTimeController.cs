using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Add this to GameController GameObject.
 * I manage pausing, slow-mo, edit-mode-pausing, etc. */
[RequireComponent(typeof(GameController))]
public class GameTimeController : MonoBehaviour {
    // Components
    private GameController gameController; // set in Awake.
    // Properties
    public bool IsDebugPriosListOpen { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsFastMo { get; private set; }
    public bool IsSlowMo { get; private set; }


    // ----------------------------------------------------------------
    //  Awake / Destroy
    // ----------------------------------------------------------------
    private void Awake() {
        gameController = GetComponent<GameController>();
        UpdateTimeScale();
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    //public void SetIsDebugPriosListOpen(bool val) {DISABLED for now.
    //    IsDebugPriosListOpen = val;
    //    UpdateTimeScale();
    //}
    private void SetIsPaused(bool val) {
        IsPaused = val;
        UpdateTimeScale();
        //GameManagers.Instance.EventManager.OnSetPaused(IsPaused);
    }
    public void TogglePause() {
        SetIsPaused(!IsPaused);
    }
    public void SetIsSlowMo(bool val) {
        IsSlowMo = val;
        UpdateTimeScale();
        SetIsPaused(false); // AUTOMATically unpause when we set slow-mo-ness.
    }
    public void SetIsFastMo(bool val) {
        IsFastMo = val;
        UpdateTimeScale();
    }
    

    // ----------------------------------------------------------------
    //  Doers (Private)
    // ----------------------------------------------------------------
    private void UpdateTimeScale() {
        if (IsPaused || IsDebugPriosListOpen) { Time.timeScale = 0; }
        else if (IsSlowMo) { Time.timeScale = 0.07f; }
        else if (IsFastMo) { Time.timeScale = 10f; }
        else { Time.timeScale = 1; }
    }




}
