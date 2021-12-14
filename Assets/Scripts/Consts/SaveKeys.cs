using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveKeys {
    // Gameplay
    public static string UserData(int saveSlot=0) {// NOTE: saveSlot is here in case we wanna add save-slots in the future!
        return "UserData_slot" + saveSlot;
    }
    //public const string LastPlayedLevelAddress = "LastPlayedLevelAddress";
    //public static string LastPlayedLevelInPack(int packIndex) { return "LastPlayedLevelInPack_" + packIndex; }
    //public static string DidCompleteLevel(LevelAddress address) { return "DidCompleteLevel_" + address.ToString(); }
}
