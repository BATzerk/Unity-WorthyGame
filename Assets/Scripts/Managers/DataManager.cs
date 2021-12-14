using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


public class DataManager {
    // Properties
    public UserData UserData { get; private set; } // loaded from saveData!
    

    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public DataManager() {
        // Load save data!
        LoadUserData();
    }
    

    // ----------------------------------------------------------------
    //  Save / Load UserData
    // ----------------------------------------------------------------
    public void LoadUserData() {
        string saveKey = SaveKeys.UserData();
        // We HAVE save for this! Load it!
        if (SaveStorage.HasKey(saveKey)) {
            string jsonString = SaveStorage.GetString(saveKey);
            UserData = JsonUtility.FromJson<UserData>(jsonString);
        }
        // We DON'T have a save for this. Make a new UserData.
        else {
            UserData = new UserData();
        }
    }
    public void SaveUserData() {
        string saveKey = SaveKeys.UserData(UserData.SaveSlot);
        string jsonString = JsonUtility.ToJson(UserData);
        SaveStorage.SetString(saveKey, jsonString);
    }
    
    
    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void ClearAllSaveData() {
        // NOOK IT
        SaveStorage.DeleteAll();
        Debug.Log("All SaveStorage CLEARED!");
        LoadUserData(); // Reload UserData!
    }


}



