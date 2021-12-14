using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager {
    // Getters (Private)
    private List<Priority> GetUserPrios() { return GameManagers.Instance.DataManager.UserData.userPrios; }
    private Dictionary<string, object> DefaultDict() {
        Dictionary<string, object> dict = new Dictionary<string, object> {
            { "AppVersion", Application.version }
        };
        return dict;
    }
    private string GetPriosRankedStr() {
        string str = "";
        List<Priority> userPrios = GetUserPrios();
        userPrios = UserData.GetPriosOrdered(userPrios); // order 'em for sure, just in case.
        for (int i=0; i<userPrios.Count; i++) {
            str += (i+1)+") " + userPrios[i].text;
            if (i < userPrios.Count-1) { str += ", "; }
        }
        return str;
    }
    private string GetPriosUnrankedStr() {
        string str = "";
        List<Priority> userPrios = GetUserPrios();
        for (int i=0; i<userPrios.Count; i++) {
            str += userPrios[i].text;
            if (i < userPrios.Count-1) { str += ", "; }
        }
        return str;
    }
    private string GetPriosEliminatedStr() {
        string str = "";
        List<Priority> prios = GameManagers.Instance.DataManager.UserData.userPriosEliminated;
        for (int i=0; i<prios.Count; i++) {
            str += prios[i].text;
            if (i < prios.Count-1) { str += ", "; }
        }
        return str;
    }
    
    
    
    
    
    public void OnSetInitialUserPrios() {
        List<Priority> userPrios = GetUserPrios();
        int numCustomPrios = 0;
        foreach (Priority prio in userPrios) {
            if (prio.IsCustom) { numCustomPrios ++; }
        }
        
        Dictionary<string, object> dict = DefaultDict();
        dict.Add("PriosInitial", GetPriosUnrankedStr());
        dict.Add("NumCustomPrios", numCustomPrios);
        CustomEvent("SetInitialPrios", dict);
    }
    
    public void OnBeginMinigameRound(int currRoundIndex) {
        Dictionary<string, object> dict = DefaultDict();
        dict.Add("RoundIndex", currRoundIndex);
        CustomEvent("BeginMinigameRound", dict);
    }
    public void OnCompleteMinigameRound(int currRoundIndex) {
        Dictionary<string, object> dict = DefaultDict();
        dict.Add("RoundIndex", currRoundIndex);
        CustomEvent("CompleteMinigameRound", dict);
    }
    
    public void OnOpenPriosFinalRank() {
        Dictionary<string, object> dict = DefaultDict();
        dict.Add("PriosRanked", GetPriosRankedStr());
        dict.Add("PriosEliminated", GetPriosEliminatedStr());
        CustomEvent("OpenPriosFinalRank", dict);
    }
    
    private void CustomEvent(string eventName, Dictionary<string,object> eventData) {
        //Analytics.CustomEvent(eventName, eventData);
        AnalyticsEvent.Custom(eventName, eventData);
    }
    
    
    
}
