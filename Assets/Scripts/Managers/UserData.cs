using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class UserData {
    // Properties
    public bool DidCompleteGame=false; // set to TRUE when we get to the end of the game!
    public SeqAddress CurrSeqAddr;
    public PrioListType listType;
    public int saveSlot;
    public Priority[] premadePrios; // the PREMADE list o' prios, based on my ListType. They're supplied by ContentManager.
    public List<Priority> userPrios=new List<Priority>(); // Index 0 is #1 priority. Last index is lowest priority.
    public List<Priority> userPriosEliminated=new List<Priority>(); // prios that've been ELIMINATED from userPrios go here.
    public Priority AbductedPrio = Priority.undefined;
    public List<MinigameNamespace.DisOrDatNS.SillyBattle> dodSillyBattles;
    public int dodSillyBattleIndex=0; // Used so we don't repeat silly-battle jokes.
    public string UserName = "User";
    public float percentPriosOrdered=-1; // 0% is every Contestant is equal. 100% is we know the most imp, second-most imp, etc. Based on NumBattlesWon.
    
    
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public UserData(PrioListType _listType){//, int _saveSlot) {
        this.listType = _listType;
        //this.SaveSlot = _saveSlot;
        this.CurrSeqAddr = new SeqAddress();
        this.premadePrios = ContentManager.GetPremadePrios(listType);
        dodSillyBattles = MinigameNamespace.DisOrDat.GetNewSillyBattleList();
    }
    
    
    // ----------------------------------------------------------------
    //  Getters
    // ----------------------------------------------------------------
    public int SaveSlot { get { return saveSlot; } }
    public PrioListType ListType { get { return listType; } }
    public Priority[] PremadePrios { get { return premadePrios; } }
    /// Returns from 0 to 100.
    public float GetDisplayProgress() {
        if (DidCompleteGame) { return 100; }
        if (userPrios.Count == 0) { return 0; }
        return Mathf.Clamp(percentPriosOrdered*100f, 0, 94);
    }
    public Priority UserPrioFirst() { return userPrios.Count==0 ? Priority.undefined : userPrios[0]; }
    public Priority UserPrioSecond() { return userPrios.Count<2 ? Priority.undefined : userPrios[1]; }
    public Priority UserPrioLast() { return userPrios.Count==0 ? Priority.undefined : userPrios[userPrios.Count-1]; }
    public Priority UserPrioSecondLast() { return userPrios.Count<2 ? Priority.undefined : userPrios[userPrios.Count-2]; }
    public Priority UserPrioEliminated(int index) { return userPriosEliminated.Count<=index ? Priority.undefined : userPriosEliminated[index]; }
    private bool IsUserPrio(string name) {
        foreach (Priority p in userPrios) {
            if (p.text == name) { return true; }
        }
        return false;
    }
    private Priority GetPremadePrio(string str) {
        // See if it exists in the premade list first.
        for (int i=0; i<premadePrios.Length; i++) {
            if (premadePrios[i].text == str) {
                return premadePrios[i];//.Clone();
            }
        }
        // Oh, it's not in the premade list. Make a new priority.
        Priority prio = new Priority(str);
        prio.IsCustom = true; // yep, it's a custom one! //NOTE: This doesn't get called the way the code works right now.
        return prio;
    }
    public string FillInBlanks(string str) {
        // Replace things I recognize!
        if (!str.Contains("[")) { return str; } // No replacement chars? Return string as it is!
        //str = str.Replace("[TopPrioTitle]", TopPrioTitle);
        //str = str.Replace("[TopPrioTitlePlain]", TopPrioTitlePlain);
        str = str.Replace("[UserPrioFirst]", UserPrioFirst().NameStyled);
        str = str.Replace("[UserPrioLast]", UserPrioLast().NameStyled);
        str = str.Replace("[UserPrioSecond]", UserPrioSecond().NameStyled);
        str = str.Replace("[UserPrioSecondLast]", UserPrioSecondLast().NameStyled);
        str = str.Replace("[UserPrioEliminated0]", UserPrioEliminated(0).NameStyled);
        str = str.Replace("[UserPrioEliminated1]", UserPrioEliminated(1).NameStyled);
        str = str.Replace("[UserPrioEliminated2]", UserPrioEliminated(2).NameStyled);
        str = str.Replace("[UserPrioEliminated3]", UserPrioEliminated(3).NameStyled);
        str = str.Replace("[AbductedPrio]", AbductedPrio.NameStyled);
        str = str.Replace("[UserName]", UserName); // Do this after the others, which may contain "[UserName]".
        
        str = str.Replace("[CharSpeech_PremadePrios]", CharSpeech_PremadePrios());
        if (str.Contains("[Speech_RankTidbit0]")) {
            str = str.Replace("[Speech_RankTidbit0]", Speech_RankTidbit0());
        }
        
        if (str.Contains("[")) { Debug.LogError("Error! Unhandled speech-text-fill-in in string: \"" + str + "\""); } // Safety check-- if there are still brackets, print an error.
        return str;
    }
    private string CharSpeech_PremadePrios() {
        int NumUserPrios = userPrios.Count;
        string str = "";
        for (int i=0; i<NumUserPrios; i++) {
            str += userPrios[i];
            if (i < NumUserPrios-2) { str += ", "; }
            else if (i == NumUserPrios-2) { str += ", and "; }
        }
        str += ".\n\nThat's it? Really?";
        return str;
    }
    private string Speech_RankTidbit0() {
        Priority pScnd = UserPrioSecond();
        Priority pSecondLast = UserPrioSecondLast();
        string str = "I'm looking through the numbers.\n\n";
        if (pSecondLast.NumBattlesWon <= 0) {
            str += "You haven't even picked " + pSecondLast.NameStyled + " yet.";
        }
        else {
            float ratio = pScnd.NumBattlesWon / pSecondLast.NumBattlesWon;
            string ratioStr = MathUtils.RoundTo1DP(ratio).ToString();
            str += "You've picked " + pScnd.NameStyled + " " + ratioStr + "x as many times as " + pSecondLast.NameStyled + ".";
        }
        //str += pScnd.NameStyled + " has only lost " + pScnd.NumBattlesLost + " and " + pLst.NameStyled + " has lost " + pLst.NumBattlesLost + ".";
        str += "\n\n...How surprising is that?";
        return str;
    }
    
    public float GetUserPriosPercentOrdered() {
        float percentOrdered=0;
        // Make a list of JUST the rank values, then set percentFullyOrdered to the size of that against the number of contestants!
        HashSet<int> ranks = new HashSet<int>();
        for (int i=0; i<userPrios.Count; i++) {
            ranks.Add(Mathf.RoundToInt(userPrios[i].NumBattlesWon)); // note: Require rounding to int, for greater orderedness precision.
        }
        percentOrdered = ranks.Count / (float)userPrios.Count;
        return percentOrdered;
    }
    
    
    // ----------------------------------------------------------------
    //  Adding / Setting Prios
    // ----------------------------------------------------------------
    public void AddUserPrios(Priority[] val) {
        for (int i=0; i<val.Length; i++) {
            AddUserPrio(val[i]);
        }
    }
    private void AddUserPrio(string prioName) {
        AddUserPrio(GetPremadePrio(prioName));
    }
    private void AddUserPrio(Priority prio) {
        // Add it if it DOESN'T already exist!
        if (!IsUserPrio(prio.text)) {
            userPrios.Add(prio);
        }
    }
    
    public void AddNewPriosFromStrings(params string[] strs) {
        //userPrios = new List<Priority>();
        for (int i=0; i<strs.Length; i++) {
            AddUserPrio(strs[i]);
        }
    }
    
    public void EliminateUserPrio(Priority prio) {
        userPrios.Remove(prio);
        userPriosEliminated.Add(prio);
    }
    
    
    
    public void Debug_AddMinimumTestUserPrios() {
        const int MinTestPrios = 2;
        //string[] possibleStrs = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };
        //for (int i=userPrios.Count; i<possibleStrs.Length; i++) {
        //    AddUserPrio(possibleStrs[i]);
        //}
        List<Priority> possiblePrios = new List<Priority>(ContentManager.GetPremadePrios(ListType));
        possiblePrios.Shuffle();
        for (int i=userPrios.Count; i<possiblePrios.Count&&i<MinTestPrios; i++) {
            AddUserPrio(possiblePrios[i].text);
        }
    }
    public void Debug_SetTestPriosWins() {
        for (int i=0; i<userPrios.Count; i++) {
            userPrios[i].NumBattlesWon = (userPrios.Count-i) * 10;
            userPrios[i].NumBattlesLost = i * 10;
        }
        OrderUserPriosByBattle();
    }
    
    
    // ----------------------------------------------------------------
    //  Ordering Prios
    // ----------------------------------------------------------------
    public static List<Priority> GetPriosOrdered(List<Priority> prios) {
        return prios.OrderByDescending(c => c.NumBattlesWon).ThenBy(c => c.NumBattlesLost).ToList();
    }
    public void OrderUserPriosByBattle() {
        // Order up!
        userPrios = GetPriosOrdered(userPrios);
        // Update percentPriosOrdered.
        UpdatePercentPriosOrdered();
        // Dispatch event!
        GameManagers.Instance.EventManager.OnSortedUserPrios();
    }
    private void UpdatePercentPriosOrdered() {
        // Make a list of JUST the rank values, then set percentPriosOrdered to the size of that against the number of contestants!
        HashSet<int> ranks = new HashSet<int>();
        foreach (Priority p in userPrios) {
            ranks.Add(Mathf.RoundToInt(p.NumBattlesWon)); // Note: DO round the values! Design choice: Decimals are usually determined by chance. Round to ints to demand higher level of ordering-accuracy.
        }
        percentPriosOrdered = ranks.Count / (float)userPrios.Count;
    }
    
}
