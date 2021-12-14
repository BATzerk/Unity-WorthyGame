using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData {
    // Properties
    public int RoundIndex { get; private set; }
    public string[] minigameNames { get; private set; } // in order.
    public int CurrMinigameIndex;
    
    // Getters (Public)
    //public bool IsAnotherMinigame() { return CurrMinigameIndex < minigameNames.Length; }
    public bool IsLastMinigame() { return CurrMinigameIndex >= minigameNames.Length-1; }
    public int NumMinigames { get { return minigameNames.Length; } }
    public string CurrMinigameName() { return minigameNames[CurrMinigameIndex]; }
    
    // Initialize
    public RoundData(int RoundIndex, string[] minigameNames) {
        this.RoundIndex = RoundIndex;
        this.minigameNames = minigameNames;
        CurrMinigameIndex = 0;
    }
}
