using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class UserData {
    // Properties
    public bool DidCompleteGame=false; // set to TRUE when we get to the end of the game!
    public SeqAddress CurrSeqAddr;
    public int saveSlot;
    public string UserName = "User";
    
    
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public UserData(){//, int _saveSlot) {
        //this.SaveSlot = _saveSlot;
        this.CurrSeqAddr = new SeqAddress();
    }
    
    
    // ----------------------------------------------------------------
    //  Getters
    // ----------------------------------------------------------------
    public int SaveSlot { get { return saveSlot; } }
    /// Returns from 0 to 100.
    public float GetDisplayProgress() {
        if (DidCompleteGame) { return 100; }
        return -1; // TEMP for now.
    }
    public string FillInBlanks(string str) {
        // Replace things I recognize!
        if (!str.Contains("[")) { return str; } // No replacement chars? Return string as it is!
        str = str.Replace("[UserName]", UserName); // Do this after the others, which may contain "[UserName]".
        
        if (str.Contains("[")) { Debug.LogError("Error! Unhandled speech-text-fill-in in string: \"" + str + "\""); } // Safety check-- if there are still brackets, print an error.
        return str;
    }
    
    
    
}
