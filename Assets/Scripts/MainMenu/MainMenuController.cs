using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : BaseViewElement {
    // Components
    [SerializeField] private TextMeshProUGUI t_progress=null;


    private void Start() {
        float progress = ud.GetDisplayProgress();
        t_progress.text = "progress: " + progress.ToString() + "%";
    }



    // ----------------------------------------------------------------
    //  UI Events
    // ----------------------------------------------------------------
    public void OnClick_Play() {
        SceneHelper.OpenScene(SceneNames.Gameplay);
    }
    
    public void OnClick_TestButton0() {
        //// TESTING
        //UserData testUD = new UserData(PrioListType.Life);
        //string jsonStr = JsonUtility.ToJson(testUD);
        //UserData recoveredUD = JsonUtility.FromJson<UserData>(jsonStr);
        GameUtils.ShowRateGamePopup();
    }
    
    public void OnClick_SetPremadePrios_Animals() {
        dm.LoadUserData(PrioListType.Animals);
        SceneHelper.OpenScene(SceneNames.Gameplay);
    }
    public void OnClick_SetPremadePrios_Life() {
        dm.LoadUserData(PrioListType.Life);
        SceneHelper.OpenScene(SceneNames.Gameplay);
    }
    public void OnClick_SetPremadePrios_Partner() {
        dm.LoadUserData(PrioListType.Partner);
        SceneHelper.OpenScene(SceneNames.Gameplay);
    }
    
    
}
