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
        LoadUserData(PrioListType.Life);
    }
    

    // ----------------------------------------------------------------
    //  Save / Load UserData
    // ----------------------------------------------------------------
    public void LoadUserData(PrioListType listType) {
        string saveKey = SaveKeys.UserData(listType);
        // We HAVE save for this! Load it!
        if (SaveStorage.HasKey(saveKey)) {
            string jsonString = SaveStorage.GetString(saveKey);
            UserData = JsonUtility.FromJson<UserData>(jsonString);
        }
        // We DON'T have a save for this. Make a new UserData.
        else {
            UserData = new UserData(listType);
        }
    }
    public void SaveUserData() {
        string saveKey = SaveKeys.UserData(UserData.ListType, UserData.SaveSlot);
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
        LoadUserData(PrioListType.Life); // Reload UserData!
    }


}





    //// Prioriy Lists
    //public Priority[] PremadePrios { get; private set; } = ContentManager.PremadePrios_Life;// PremadePrios_Animals PremadePrios_Life // The premade list users may pick from.
    
    

    // ----------------------------------------------------------------
    //  Getters
    // ----------------------------------------------------------------
    //public LevelAddress GetLastPlayedLevelAddress() {
    //    // Save data? Use it!
    //    if (SaveStorage.HasKey (SaveKeys.LastPlayedLevelAddress)) { 
    //        return LevelAddress.FromString (SaveStorage.GetString (SaveKeys.LastPlayedLevelAddress));
    //    }
    //    // No save data. Default to the first level, I guess.
    //    else {
    //        return new LevelAddress(0, 0);
    //    }
    //}


    /** OLD
    public void MakePrioFirstPriority(Priority _prio) {
        List<Priority> prios = new List<Priority>(userPrios);
        prios.Remove(_prio);
        prios.Insert(0, _prio);
        userPrios = prios;
    }
    
    ///** Simply updates the existing list's text. * /
    //public void UpdatePriosTexts(string[] val) {
    //    for (int i=0; i<userPrios.Length; i++) {
    //        userPrios[i].text = val[i];
    //    }
    //}
    
    //static private List<Priority> GetPriosOrdered(List<Priority> priosOriginal, List<int> newOrder) {
    //    List<Priority> priosOrdered = new List<Priority>(priosOriginal);
    //    for (int i=0; i<priosOrdered.Count; i++) {
    //        int oldIndex = newOrder[i];
    //        priosOrdered[i] = priosOriginal[oldIndex];
    //    }
    //    return priosOrdered;
    //}
    
    */
    //public string TopPrioTitle { get; private set; } // with coloring included.
    //public string TopPrioTitlePlain { get; private set; } // without any color tags.
    
    //// Setters
    //public void SetTopPrioTitle(string str) {
    //    TopPrioTitle = str;
    //    TopPrioTitlePlain = str;
    //    // Remove "<color=#FF00FF>" and "</color>
    //    int index = TopPrioTitlePlain.IndexOf('>');
    //    TopPrioTitlePlain = TopPrioTitlePlain.Substring(index+1);
    //    index = TopPrioTitlePlain.IndexOf('<');
    //    TopPrioTitlePlain = TopPrioTitlePlain.Substring(0, index);
    //}


    /*
    public readonly Priority[] premadePrios = {
        //new Priority("Parents").SetTrnyLoseOtherSpeech("I guess they DID raise you after all...").SetTrnyHellos("Respect your elders!"),//.SetTrnyLoseSpeech("But they raised you into who you are today...").
        //new Priority("Siblings").SetTrnyLoseOtherSpeech("I guess your siblings ARE pretty important to you...").SetTrnyHellos("Time for some sibling rivalry!"),//.SetTourneyLoseSpeech("Talk about sibling rivalry..."),
        new Priority("Family"),
        new Priority("My kids").SetTrnyLoseOtherSpeech("I guess kids are pretty important...").SetTrnyHellos("The kids are all right!"),//.SetTourneyLoseSpeech("It's ok, they'll just raise themselves..."),
        new Priority("Partner").SetTrnyLoseOtherSpeech("Your partner will appreciate this...").SetTrnyHellos("It's like we're on a date, but only ONE of us can win!", "Isn't this romantic!"),//.SetTourneyLoseSpeech("I won't tell your partner they've been demoted..."),
        new Priority("Socializing").SetTrnyLoseOtherSpeech("You'll be so popular...").SetTrnyHellos("No! YOU'RE Chandler! I'M Phoebe!!", "Why can't WE be friends?"),
        //new Priority("Pets").SetTrnyLoseOtherSpeech("Me-OW...").SetTrnyHellos("This is gonna be ruff", "I put the OW in bow-ow!"),
        new Priority("Health").SetTrnyLoseOtherSpeech("Maybe I should have prioritized MY health...").SetTrnyHellos("Let's get physical!", "Do you even lift, brother?"),
        new Priority("Looks").SetTrnyLoseOtherSpeech("Damn you look fiiiine...").SetTrnyHellos("LOOKS like I made a pun! Ha ha ha ha ha"),
        new Priority("Dating").SetTrnyLoseOtherSpeech("Go... present yourself to the world...").SetTrnyHellos("It's just the two of us... so romantic :)", "I want to learn more about you"),
        new Priority("Eating furniture").SetTrnyLoseOtherSpeech("Why are you eating furniture? What is this?").SetTrnyHellos("OMNOMNOMNOM", "RAOR OM NOMNOMNOMNOM"),
        new Priority("School").SetTrnyLoseOtherSpeech("Looks like I just got schooled...").SetTrnyHellos("Recess is OUT", "You ain't too cool for school... no one is!"),
        new Priority("Boss").SetTrnyLoseOtherSpeech("I guess... *cough*... you ARE the boss of me... *cough*").SetTrnyHellos("I'm a BOSS", "I'm STILL the boss!"),
        new Priority("Coworkers").SetTrnyLoseOtherSpeech("Camaradere is pretty important...").SetTrnyHellos("What's a cow orker?..", "What are cow orkers?..."),
        new Priority("Employees").SetTrnyLoseOtherSpeech("Treat them well... *cough*... and fairly... *cough*").SetTrnyHellos("I'm about to employ my FIST into you!", "Now I'm gonna employ my LEG into YOU!"),
        new Priority("Earning money").SetTrnyLoseOtherSpeech("Go make your wallet fat...").SetTrnyHellos("$$! $$$!", "Step 4: Profit!!"),
        new Priority("Having enough money").SetTrnyLoseOtherSpeech("Keep those bank-account 0's up up up...").SetTrnyHellos("Pinch pennies, not cheeks!", "Pinch pennies, not pineapples!"),
        new Priority("Retirement").SetTrnyLoseOtherSpeech("70-year-old-you thanks you...").SetTrnyHellos("I'm gonna retire YOU!", "Retirement? More like reWOKEment!"),
        new Priority("Growing a second beard").SetTrnyLoseOtherSpeech("How is one beard not enough for you...").SetTrnyHellos("You can't spell BEARD without BEAR", "Did you know? Not all lumberjacks have beards!"),
        new Priority("Creating").SetTrnyLoseOtherSpeech("Liz Gilbert would be proud...").SetTrnyHellos("You know what I'm gonna make? I'm gonna make you <i>squeal</i>", "You'd better get creative with your strategy to beat ME!"),
        new Priority("Joy").SetTrnyLoseOtherSpeech("I guess I can't be angry at joy...").SetTrnyHellos("I'm just happy to be here", "This is exciting!"),
        new Priority("Play").SetTrnyLoseOtherSpeech("You're already playing a game, so that's a start...").SetTrnyHellos("You think this is a GAME?!?!?", "Oh I guess this IS a game..."),
        new Priority("Inner contentment").SetTrnyLoseOtherSpeech("Nama... *cough*... Namaste.").SetTrnyHellos("Inner peace? It's outer conflict <i>here</i>, bub!"),
        new Priority("Fitting inside a guitar").SetTrnyLoseOtherSpeech("Because fitting inside the guitar CASE wasn't enough of a challenge!").SetTrnyHellos(""),
        new Priority("Alone time").SetTrnyLoseOtherSpeech("I guess self-care is... pretty important...").SetTrnyHellos(""),
        new Priority("Reading").SetTrnyLoseOtherSpeech("Your brain is gonna be the size of a mountain with all that knowledge...").SetTrnyHellos("You're READING this text, so I am already winning!"),
        new Priority("Becoming an eggplant").SetTrnyLoseOtherSpeech("If YOU don't become the eggplant, then who WILL, I guess...").SetTrnyHellos("I am food group #1"),
        //"Naming someone Chyenne",
        //    "Going to bed angry",
        //    "Holding a trash can",
        //    "Spelling hors d'oeuvres",
        //    "Not picking this item",
        //    "Winking at nature",
    };
    */