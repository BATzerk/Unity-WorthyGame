using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSavePopup : BasePopup {
    public override PopupIDs MyID() { return PopupIDs.DeleteSaveData; }
    
    public void OnClick_Yes() {
        dm.ClearAllSaveData();
        SceneHelper.ReloadScene();
    }
    //public void OnClick_
}
