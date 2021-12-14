using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager {
    // Getters (Private)
    private Dictionary<string, object> DefaultDict() {
        Dictionary<string, object> dict = new Dictionary<string, object> {
            { "AppVersion", Application.version }
        };
        return dict;
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
    
    private void CustomEvent(string eventName, Dictionary<string,object> eventData) {
        //Analytics.CustomEvent(eventName, eventData);
        AnalyticsEvent.Custom(eventName, eventData);
    }
    
    
    
}
