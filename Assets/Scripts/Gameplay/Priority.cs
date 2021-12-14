 // TO DO: Move into its own class if we keep this.
public enum PrioRank { Any, Top, Bottom }

[System.Serializable]
public enum PrioListType {
    Numbers, // for testing.
    Animals, // for testing.
    Life,
    Partner,
}


[System.Serializable]
public class Priority {
    // Readonlies
    static public readonly Priority undefined = new Priority("undefined");
    // Properties
    public bool IsCustom = false; // will be TRUE if this Priority was inputted manually by the user.
    public string text;
    public float NumBattlesWon = 0;
    public float NumBattlesLost = 0;
    public float NumBattlesTied = 0;
    public int MySource_PremadePriosShowIndex=-1; // (idk how to use this yet.) Prios that were added in first round, understandably, more important than latter ones.
    
    // Initialize
    public Priority(string text) {
        this.text = text;
    }
    public Priority(string text, bool isCustom) {
        this.text = text;
        this.IsCustom = isCustom;
    }
    
    //// Clone
    //public Priority Clone() {
    //    return new Priority(text);
    //}
    
    // Getters (Public)
    /// Styled with TextMeshPro color tag.
    public string NameStyled { get { return "<color=#007799>" + text + "</color>"; } }

    
    public override string ToString() { return text; }
    public override bool Equals(object o) { return object.ReferenceEquals(this, o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
    public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
    
    public static bool operator == (Priority a, Priority b) {
        return a.Equals(b);
    }
    public static bool operator != (Priority a, Priority b) {
        return !a.Equals(b);
    }
    
    
    
    // OLD stuff
    /*
    public string trnyOtherLoseSpeech; // OLD! DNU.
    public string[] trnyHellos; // OLD! DNU.
    
    public Priority SetTrnyLoseOtherSpeech(string str) { trnyOtherLoseSpeech = str; return this; }
    public Priority SetTrnyHellos(params string[] strs) { trnyHellos = strs; return this; }
    
        trnyOtherLoseSpeech = "";
        trnyHellos = new string[0];
        
        .SetTrnyLoseOtherSpeech(trnyOtherLoseSpeech).SetTrnyHellos((string[])trnyHellos.Clone());
        */
}