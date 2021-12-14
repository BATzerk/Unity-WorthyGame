using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Kind of a ha-cky class to handle all text content. */
public class ContentManager {
    
    // Getters (Public)
    private SeqChapter GetChapter(int chapter) { return seqChapters[chapter]; }
    private SeqChunk GetChunk(int chapter,int chunk) { return seqChapters[chapter].chunks[chunk]; }
    public SeqStep GetStep(SeqAddress addr) { return GetStep(addr.chapter,addr.chunk,addr.step); }
    public SeqStep GetStep(int chapter,int chunk,int step) { return seqChapters[chapter].chunks[chunk].steps[step]; }
    
    public SeqAddress NextStep(SeqAddress addr) { return AddressWrap(addr.chapter, addr.chunk, addr.step+1); }
    public SeqAddress PrevStep(SeqAddress addr) { return AddressWrap(addr.chapter, addr.chunk, addr.step-1); }
    public SeqAddress NextChunk(SeqAddress addr) { return AddressWrap(addr.chapter, addr.chunk+1, addr.step); }
    public SeqAddress PrevChunk(SeqAddress addr) { return AddressWrap(addr.chapter, addr.chunk-1, addr.step); }
    public SeqAddress NextChapter(SeqAddress addr) { return AddressWrap(addr.chapter+1, addr.chunk, addr.step); }
    public SeqAddress PrevChapter(SeqAddress addr) { return AddressWrap(addr.chapter-1, addr.chunk, addr.step); }
    
    private SeqAddress AddressWrap(int chapter,int chunk,int step) {
        // Safety check: Can't go farther back? Return same address.
        if (chapter<=0 && chunk<=0 && step<=0) { return new SeqAddress(Mathf.Max(0,chapter),Mathf.Max(0,chunk),Mathf.Max(0,step)); }
        
        // Wrap Chunk BACKward?
        if (chunk < 0) {
            chapter = Mathf.Max(0, chapter-1); // Go to previous Chapter.
            chunk = seqChapters[chapter].NumChunks-1;
            step = GetChunk(chapter,chunk).NumSteps-1;
        }
        // Wrap Chunk FORward?
        else if (chunk >= GetChapter(chapter).NumChunks) {
            chapter ++;
            chunk = 0;
            step = 0;
        }
        
        // Wrap Step BACKward?
        if (step < 0) {
            chunk --; // go to previous Chunk.
            if (chunk < 0) { // Wrap Chunk BACKward?
                chapter = Mathf.Max(0, chapter-1); // Go to previous Chapter.
                chunk = seqChapters[chapter].NumChunks-1;
            }
            step = GetChunk(chapter,chunk).NumSteps-1;
        }
        // Wrap Step FORward?
        else if (step >= GetChunk(chapter,chunk).NumSteps) {
            step = 0;
            chunk ++;
            // Wrap Chunk FORward?
            if (chunk >= GetChapter(chapter).NumChunks) {
                chapter ++;
                chunk = 0;
                step = 0;
            }
        }
        
        return new SeqAddress(chapter,chunk,step);
    }
    
    // ----------------------------------------------------------------
    //  Initialize
    // ----------------------------------------------------------------
    public ContentManager() {
        // Init values for content.
        InitSeqContentAddresses();
    }
    /** Call this once when program starts. It's just to give all content its correct index. */
    private void InitSeqContentAddresses() {
        for (int chapInd=0; chapInd<seqChapters.Length; chapInd++) {
            SeqChapter chapter = seqChapters[chapInd];
            chapter.MyAddr = new SeqAddress(chapInd, 0,0);
            for (int chunkInd=0; chunkInd<chapter.NumChunks; chunkInd++) {
                SeqChunk chunk = chapter.chunks[chunkInd];
                chunk.MyAddr = new SeqAddress(chapInd, chunkInd, 0);
                for (int stepInd=0; stepInd<chunk.NumSteps; stepInd++) {
                    SeqStep step = chunk.steps[stepInd];
                    step.MyAddr = new SeqAddress(chapInd, chunkInd, stepInd);
                }
            }
        }
    }
    
    
    public static Priority[] GetPremadePrios(PrioListType listType) {
        switch (listType) {
            case PrioListType.Animals: return PremadePrios_Animals;
            case PrioListType.Numbers: return PremadePrios_Numbers;
            case PrioListType.Life: return PremadePrios_Life;
            case PrioListType.Partner: return PremadePrios_Partner;
            default: return null; // Oops.
        }
    }
    private static readonly Priority[] PremadePrios_Animals = { // for TESTING. So Brett's brain doesn't see life priorities as bland/trivial from all the testing.
        new Priority("Antelope"),
        new Priority("Butterfly"),
        new Priority("Caterpillar"),
        new Priority("Duck"),
        new Priority("Elephant"),
        new Priority("Fox"),
        new Priority("Giraffe"),
        new Priority("Horse"),
        new Priority("Iguana"),
        new Priority("Jackal"),
        new Priority("Kangaroo"),
        new Priority("Llama"),
        new Priority("Mouse"),
        new Priority("New Guinea Singing Dog"),
        new Priority("Ostrich"),
        new Priority("Panda"),
        new Priority("Quail"),
        new Priority("Rhino"),
        new Priority("Snake"),
        new Priority("Turtle"),
        new Priority("Unicorn"),
        new Priority("Vole"),
        new Priority("Wildebeest"),
        new Priority("Xoloitzcuintli"),
        new Priority("Yak"),
        new Priority("Zebra"),
    };
    private static readonly Priority[] PremadePrios_Numbers = {
        new Priority("1"),
        new Priority("2"),
        new Priority("3"),
        new Priority("4"),
        new Priority("5"),
        new Priority("6"),
        new Priority("7"),
        new Priority("8"),
        new Priority("9"),
        new Priority("10"),
        new Priority("11"),
        new Priority("12"),
        new Priority("13"),
        new Priority("14"),
        new Priority("15"),
        new Priority("16"),
        new Priority("17"),
        new Priority("18"),
        new Priority("19"),
        new Priority("20"),
    };
    private static readonly Priority[] PremadePrios_Life = { // the ACTUAL priorities options give to the player.
        new Priority("Family"),
        new Priority("My kids"),
        new Priority("My partner"),
        new Priority("Dating"),
        new Priority("Socializing"),
        new Priority("Earning money"),
        new Priority("Having enough money"),
        new Priority("Investments"),
        new Priority("Learning"),
        new Priority("My career"),
        new Priority("My employees"),
        new Priority("Being a leader"),
        new Priority("Creating"),
        new Priority("Joy and Playfulness"),
        new Priority("Find purpose"),
        new Priority("Mental health"),
        new Priority("Physical health"),
        new Priority("My looks"),
        new Priority("Self-control"),
        new Priority("Spirituality"),
        
        //new Priority("Self-care"),
        //new Priority("Volunteering"),
        //new Priority("Enjoy life"),
        //new Priority("My boss"),
        //new Priority("My coworkers"),
        ////new Priority("Reading"),
    };
    private static readonly Priority[] PremadePrios_Partner = { // not used yet, but maybe will in future!
        new Priority("Playful"),
        new Priority("Optimistic"),
        new Priority("Wealthy"),
        new Priority("Attractive"),
        new Priority("Physically satisfying"),
        new Priority("Wants kids"),
        new Priority("Openness"),
        new Priority("Trustworthiness"),
        new Priority("Maturity"),
        new Priority("Ambition"),
        new Priority("Respect"),
        new Priority("Empathy"),
        new Priority("Makes me laugh"),
        new Priority("Reliable"),
        new Priority("Growth-oriented"),
        new Priority("Kind"),
        new Priority("Selfless"),
        new Priority("Intelligent"),
        new Priority("Similar interests"),
        new Priority("Accepts me"),
        new Priority("Shows affection"),
        new Priority("Present"),
        new Priority("Best friends"),
        new Priority("Teaches me"),
        new Priority("Wants 'us time'"),
        new Priority("Okay alone"),
    };
    
    
    
    // ----------------------------------------------------------------
    //  Content!
    // ----------------------------------------------------------------
    public readonly SeqChapter[] seqChapters = {
        // ___________________________________________
        //  NEW CONTENT FORM (todo: clean alllll dis uppp)
        // ___________________________________________
        new SeqChapter(new SeqChunk[]{
            //// TESTING!!
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep().SetFunc("Debug_SetTestUserPrios"),
            //    // ---- ROUND 1 ----=
            //    new SeqStep().SetMinigameRound(1, PrioRank.Any,
            //        "UFOShootDown"
            //    ),
            //}),
            
            
            // Preface
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.BlandPopup, "This game is an experiment.\n\nIt's a work in progress, and I'm happily collecting feedback and ideas!").SetBtn("NEXT"),
                new SeqStep(Chars.BlandPopup, "Wanna help development?\n\nEmail your ideas/feedback to brett@mydogzorro.com!").SetBtn("OK"),
            }),
            // GameIntro
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("ShowUserNameEntry"),
                new SeqStep().SetDialogueTree("GameIntro"),
            }),
            // PremadePriosChoices!
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Now.\n\nPlease pick TEN things that matter to you.", "Zorro_Side0").SetBtn("NEXT"),
                new SeqStep().SetFunc("OpenPremadePriosChoices0"),
                new SeqStep(Chars.Narrator, "Great!\n\nLet the minigames begin!").SetBtn("NEXT"),
            }),
            
            
            new SeqChunk(new SeqStep[]{
                // ---- ROUND 1 ----
                new SeqStep().SetMinigameRound(0, PrioRank.Any,
                    "BurningBuilding",//TO DO: Remake this.
                    "HaveToPee",//TO DO: Remake this.
                    "DoD"
                ),
                
                
                // ---- ROUND 2 ----
                new SeqStep(Chars.Narrator, "Hey! It's Zorro again.", "Zorro_Front0").SetBtn("HI, ZORRO"),
                new SeqStep().SetDialogueTree("ComfortQuestion0"),
                new SeqStep(Chars.Narrator, "Without further ado, let's enter Round 2!").SetBtn("BEGIN"),
                new SeqStep().SetMinigameRound(1, PrioRank.Bottom,
                    "PatOnBack",
                    "DoD",
                    "ComfyChair"
                ),
                // Elimination #1: The Bachelor
                new SeqStep(Chars.Narrator, "Nice work! Now it's time to ELIMINATE 2 priorities from the game.").SetBtn("OH?"),
                new SeqStep().SetFunc("OpenElimigame_TheBachelor"),
                
                
                // ---- ROUND 3 ----
                new SeqStep().SetDialogueTree("ComfortQuestion_PostBachelor"),
                new SeqStep(Chars.Narrator, "Anyway. Let's spree into Round 3!").SetBtn("BEGIN ROUND 3"),
                new SeqStep().SetMinigameRound(2, PrioRank.Any,
                    "RhinoCharge",
                    "DoD",
                    "KnifeToMurder"
                ),
                
                
                // ---- ROUND 4 ----
                new SeqStep().SetDialogueTree("RankTidbit0"),
                new SeqStep().SetDialogueTree("ComfortQuestion2"),
                new SeqStep(Chars.Narrator, "Anywho. Let's pour into Round 4!").SetBtn("BEGIN ROUND 4"),
                new SeqStep().SetMinigameRound(3, PrioRank.Any,
                    "DoD",
                    "CaptchaFill",
                    "WindowChuck"
                ),
                // Elimination #2
                new SeqStep(Chars.Narrator, "Looks like it's time to eliminate TWO MORE priorities.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Let's take a visit to the local petting zoo!").SetBtn("GO TO PETTING ZOO"),
                new SeqStep().SetFunc("OpenElimigame_PettingZoo"),
                
                
                // ---- ROUND 5 ----
                new SeqStep().SetDialogueTree("DontCareMuchForLast"),
                new SeqStep(Chars.Narrator, "Anywhey. Let's jive into Round 5!").SetBtn("BEGIN ROUND 5"),
                new SeqStep().SetMinigameRound(4, PrioRank.Any,
                    "DoD"
                    //TODO: More minigames
                ),
                
                
                // ---- ROUND 6 ----
                new SeqStep().SetDialogueTree("ComfortQuestion3"),
                new SeqStep(Chars.Narrator, "Wow, final round!\n\nAfter this, we'll finally see your ranked priorities list!").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Let's get our kicks with Round 6!").SetBtn("BEGIN ROUND 6"),
                new SeqStep().SetMinigameRound(5, PrioRank.Any,
                    "DoD",
                    "UFOAbduction",
                    "UFOHungry",
                    "UFOShootDown"
                ),
                
            }),
            
            
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "And that's a wrap! Ready to see the results?").SetBtn("YES"),
                new SeqStep(Chars.Narrator, "Here are the results...").SetBtn("NEXT"),
                new SeqStep().SetFunc("OpenPriosFinalRank").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("reveal"),
                new SeqStep().SetFunc("RevealPriosFinalRankPrio").SetBtn("DONE"),
                new SeqStep().SetFunc("ShrinkPriosFinalRank"),
                new SeqStep().SetFunc("SetDidCompleteGameTrue"),
            }),
            
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "...So! This game is an experiment in progress, and Brett's deciding what he wants to do next with it.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "If you have feedback or ideas, email us at brett@mydogzorro.com.").SetBtn("NEXT"),
                new SeqStep().SetFunc("ShowRateGamePopup"),
                new SeqStep(Chars.Narrator, "Thanks for playing!\n\n<3").SetFunc("ShowPriosFinalRankAtMiniPos"),
            }),
        
        }),
            
            
            
            
            
            
            
            
            
                
                //new SeqStep(Chars.Narrator, "Hi, [UserName]! I'm Bratt Taylor.\nWelcome to The Priorities Game!", "Bratt0").SetBtn("NEXT"),
                ////new SeqStep(Chars.Narrator, "My goal is to improve the clarity of your life priorities today.", "Bratt1").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Here's how this game works:"),
                //new SeqStep(Chars.Narrator, "We're gonna play minigames...").SetFunc("ShowFinalRankSample"),
                //new SeqStep(Chars.Narrator, "...And at the end, we'll have a <color=#1F71DE>ranked list</color> of your life priorities today!", "Bratt1").SetBtn("NEXT").SetFunc("RevealMoreFinalRankSample"),
                //new SeqStep(Chars.Narrator, "The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.", "Bratt2").SetBtn("NEXT"),
                //new SeqStep().SetFunc("HideFinalRankSample"),
                
                //new SeqStep(Chars.Narrator, "[CharSpeech_PremadePrios]").SetBtn("I... I...."),
                //new SeqStep(Chars.Narrator, "It's tough only picking a few, huh?").SetBtn("YEAH"),
                //new SeqStep(Chars.Narrator, "Now we're gonna play minigames to determine the ORDER of your priorities!").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "By the end, we'll have an <b>ordered list</b> of your <b>top priorities</b>.").SetBtn("NEXT"),
            
                //new SeqStep().SetDialogueTree("PreRound2"),
                
                //new SeqStep().SetDialogueTree("PreTheBachelor"),
                //new SeqStep().SetDialogueTree("PostTheBachelor"),
                
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.Narrator, "And that's a wrap! Ready to see the results?").SetBtn("NO"),
            //    new SeqStep(Chars.Narrator, "I'm curious-- what do you THINK your priorities are?").SetBtn("NEXT"),
            //    new SeqStep().SetFunc("OpenPriosManualRankView"),
            //    new SeqStep(Chars.Narrator, "Let's see how you did...").SetBtn("NEXT"),
            //    new SeqStep().SetFunc("RevealPriosManualRankView").SetBtn("NEXT"),
            //    new SeqStep().SetFunc("ClosePriosManualRankView"),
            //}),
            
            
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.Narrator, "This was fun!\nI like making you productively uncomfortable.\n\nWe should do this again sometime.").SetBtn("NEXT"),
            //    new SeqStep(Chars.Narrator, "That's the end of this build.").SetBtn("NEXT"),
            //    new SeqStep(Chars.Narrator, "Thanks for playing!\n\n<3"),
            //}),
                
                
                //// ---- ROUND 7 ----
                //new SeqStep(Chars.Narrator, "TO DO: Write branching text about this being the final round!").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Let's crank it to eleven with Round 7!").SetBtn("BEGIN ROUND 7"),
                //new SeqStep().SetMinigameRound(6, PrioRank.Any,
                //    "DoD"
                //),
                
                
                
                
                //new SeqStep().SetFunc("OpenElimigame_SendToSpace"),
                
                
                    //"LazySusan",TO DO: Replace shark with a gnome
                    //"BasketCatch",
                    //"KingOfHill"
                    //"FreeTonight"//TO DO: Replace this with something more interesting!!
                    
                //new RoundData(PrioRank.Any,new string[] {
                //    "KissMarryKill",
                //    "VendingMachine",
                //    "ExamComfort",
                //    "SinkingShip",
                //    "SaveDrowning",
                //}),
                
                    //"DoD",
                    //"MakeADate",
                    //"PickDateLocation0",
                    //"DoD",
                    //"MakeADate",
                    //"PickDateLocation1",
                    //"WeddingInvites"
                    
                    
                //// Elimination #4
                ////new SeqStep(Chars.Narrator, "Whoa, aliens!").SetBtn("WHOA"),
                //new SeqStep(Chars.Narrator, "Umm, time for another test elimination now!").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "(NOTE: This part is in progress! Brett still has to make more elimigames.)").SetBtn("NEXT"),
                //new SeqStep().SetFunc("OpenElimigame_TestBasic"),//TO DO: Actual elimigame. Make VolcanoSacrifice!
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep().SetFunc("Debug_SetTestUserPrios"),
            //    new SeqStep().SetFunc("Debug_SetTestPriosWins"),
            
            //    new SeqStep().SetFunc("OpenMinigames"),
            //    new SeqStep().SetFunc("CloseMinigames"),
            //}),
        
        
        /*
        // ___________________________________________
        //  CHAPTER 1 OLD
        // ___________________________________________
        new SeqChapter(new SeqChunk[]{
            new SeqChunk(new SeqStep[]{
                //new SeqStep(Chars.Narrator, "Hi, [UserName]! I'm Bratt Taylor. Welcome to The Priorities Game!", "Bratt0").SetBtn("NEXT"),
                new SeqStep().SetFunc("ShowUserNameEntry"),
                new SeqStep().SetDialogueTree("GameIntro"),
                //new SeqStep(Chars.Narrator, "My goal with this game is to improve the clarity of your life priorities today.", "Bratt1").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.", "Bratt2").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Priorities shift, so don't worry about getting the order perfect.\n\nClarity for today will be enough for today.").SetBtn("NEXT"),
                
                //new SeqStep(Chars.Narrator, "Unrelated question: What do you say when you wanna dump someone?", "Bratt0").SetBtn("NEEEEEXT"),
                //new SeqStep(Chars.Narrator, "Wow, an effective strategy! I like you. Let's get started.").SetBtn("NEEEEEEEEEEXT"),
                //new SeqStep(Chars.Narrator, "Yep, I hear ya.\n\nFirst, please misspell \"TEXT\"").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Ha, what a clever alternate spelling!\n\nNow. Please find a creative place to insert the letter X in the word NET.", "Bratt2").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "What a special place to put the letter X! It certainly won't be lonely surrounded by its new friends.", "Bratt0").SetBtn("ACHOO"),
                //new SeqStep(Chars.Narrator, "Bless you.\n\nNow, please choose FIVE things that matter to you.").SetBtn("NEXT"),
            }),
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.Narrator, "Now.\n\nPlease pick TEN things that matter to you.").SetBtn("NEXT"),
            //    new SeqStep().SetFunc("OpenPremadePriosChoices0"),
            //    //new SeqStep(Chars.Narrator, "[CharSpeech_PremadePrios]").SetBtn("I... I...."),
            //    //new SeqStep(Chars.Narrator, "It's tough only picking a few, huh?").SetBtn("YEAH"),
            //    new SeqStep(Chars.Narrator, "Great!").SetBtn("GREAT."),
            //}),
            
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.Narrator, "Now pick FIVE more.\n\nYou can enter your own this time, too!").SetBtn("YAY!"),
            //    new SeqStep().SetFunc("OpenPremadePriosChoices1"),
            //    new SeqStep(Chars.Narrator, "Nice! I especially like how you added [UserPrioLast] and [UserPrioSecondLast].").SetBtn("NEXT"),
            //}),
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.Narrator, "Let's make this more pppeeerrsonal.\n\nADD ONE of your own!").SetBtn("NEXT"),
            //    new SeqStep().SetFunc("OpenPriosEntry"),
            //    new SeqStep().SetFunc("ClosePriosEntry"),
            //    new SeqStep(Chars.Narrator, "[UserPrioLast]? Love it.").SetBtn("THANKS"),
            //}),
            new SeqChunk(new SeqStep[]{
                //new SeqStep(Chars.Narrator, "Man... between [UserPrioSecond] and [UserPrioFirst]... I am honestly not sure which I'd go with.").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Maybe [UserPrioFirst].\n\nOr maybe not.").SetBtn("NEXT"),
                //new SeqStep(Chars.Narrator, "Don't worry. I'll be sure to make you uncomfortable with that match-up in some surprising way!").SetBtn("BRING IT ON"),
                //new SeqStep(Chars.Narrator, "Maybe things'll change in the next round, hm? Let's find out!").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Comfort Question: How do you feel when I ask you to pick between [UserPrioSecond] and [UserPrioFirst]?").SetFunc("ShowComfortScale0"),
                new SeqStep(Chars.Narrator, "[Speech_ComfortAwareness0]").SetBtn("HM"),
                new SeqStep(Chars.Narrator, "Anyway. Let's spree into Round 3!").SetBtn("BEGIN ROUND 3"),
                new SeqStep().SetFunc("OpenMinigames"),
                new SeqStep().SetFunc("CloseMinigames"),
            }),
            new SeqChunk(new SeqStep[]{
                //new SeqStep(Chars.Narrator, "Ah. I love it when two priorities fall in love.\n\nIt's so...\nSurreal.").SetBtn("YEP"),
                new SeqStep(Chars.Narrator, "All this matchmaking's given me an idea.").SetBtn("OH?"),
                new SeqStep(Chars.Narrator, "Yeah, I'm gonna put you on The Bachelorette.\n\nYou'll have to ELIMINATE two priorities from the game.").SetBtn("OH"),
                //new SeqStep(Chars.Narrator, "You'll get to choose how to handle it.").SetBtn("OKAY..."),
                new SeqStep(Chars.Narrator, "That should add some real quality drama, don'tcha think?\n\nGood luck!").SetBtn("BEGIN"),
                new SeqStep().SetFunc("OpenElimigame_TheBachelor"),
                new SeqStep().SetDialogueTree("PostTheBachelor"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetDialogueTree("RankTidbit0"),
                //new SeqStep(Chars.Narrator, "Let's talk about your feelings.").SetBtn("OK"),
                new SeqStep(Chars.Narrator, "Comfort Question: How do you feel when I ask you to pick between [UserPrioLast] and [UserPrioSecond]?").SetFunc("ShowComfortScale1"),
                new SeqStep(Chars.Narrator, "[Speech_ComfortAwareness1]").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Anywho. Let's pour into Round 4!").SetBtn("BEGIN ROUND 4"),
                new SeqStep().SetFunc("OpenMinigames"),
                new SeqStep().SetFunc("CloseMinigames"),
            }),
            
            
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetDialogueTree("DontCareMuchForLast"),
                new SeqStep(Chars.Narrator, "Comfort Question: How do you feel when I ask you to pick between [UserPrioFirst] and [UserPrioSecond]?").SetFunc("ShowComfortScale2"),
                new SeqStep(Chars.Narrator, "[Speech_ComfortAwareness2]").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Okay, final round! Sky high with Round 5!").SetBtn("BEGIN ROUND 5"),
                new SeqStep().SetFunc("OpenMinigames"),
                new SeqStep().SetFunc("CloseMinigames"),
                //ShowComfortScale3
                //new SeqStep(Chars.Narrator, "[Speech_ComfortAwareness2]").SetBtn("NEXT"),
            }),
            
            
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Okay, I've got a pretty good idea what matters to you.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "I'm curious-- what do you THINK your priorities are?").SetBtn("NEXT"),
                new SeqStep().SetFunc("OpenPriosManualRankView"),
                new SeqStep(Chars.Narrator, "Let's see how you did...").SetBtn("NEXT"),
                new SeqStep().SetFunc("RevealPriosManualRankView").SetBtn("NEXT"),
                new SeqStep().SetFunc("ClosePriosManualRankView"),
            }),
            
            
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "This was fun!\nI like making you productively uncomfortable.\n\nWe should do this again sometime.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "That's the end of this build.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Thanks for playing!\n\n<3"),
            }),
        }),*/
        
        
        
        //// ___________________________________________
        ////  CHAPTER 2: Minigames!
        //// ___________________________________________
        //new SeqChapter(new SeqChunk[]{
        //    new SeqChunk(new SeqStep[]{
        //        //new SeqStep(Chars.Narrator, "So, now that we've got a list of what you care about...").SetBtn("NEXT"),
        //        //new SeqStep(Chars.Narrator, "Let's pressure you into revealing which are MOST and LEAST important...\n\nby playing minigames!").SetBtn("BEGIN ROUND 1"),
        //        new SeqStep(Chars.Narrator, "All right, let the games begin!!").SetBtn("BEGIN ROUND 1"),
        //        new SeqStep().SetFunc("OpenMinigames"),
        //        new SeqStep().SetFunc("CloseMinigames"),
        //    }),
        //}),
        
        
        
        
        
        
        
        
        
        
        
        
        
        // ___________________________________________
        //  ~~~~ old dialogue ~~~~
        // ___________________________________________
        new SeqChapter(new SeqChunk[]{
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Now, let's play a little juggling game.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "TAP things to keep them in the air.").SetBtn("321 GO"),
                new SeqStep().SetFunc("StartJugglingGame"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("StopJugglingGame"),
                new SeqStep(Chars.Narrator, "Woohoo, that was chaotic!").SetBtn("ACK"),
                new SeqStep(Chars.Narrator, "It looks as if what's MOST important is [UserPrioFirst] and what's LEAST important is [UserPrioLast].").SetBtn("YES... WAIT NO"),
                new SeqStep(Chars.Narrator, "...Oh. No?").SetBtn("YES, NO."),
                new SeqStep(Chars.Narrator, "Okay, to clarify, are you saying yes or no?").SetBtn("NO"),
                new SeqStep(Chars.Narrator, "You're NOT saying yes or no? Umm, all right.\nThen what are you saying?").SetBtn("NEEEEXT"),
                new SeqStep(Chars.Narrator, "Oof. Loud and clear.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Chaos happens when you don't have a plan and everything comes at you at once and you don't have time to react.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Chaos is what happens when you let life dictate your priorities, instead of your priorities dictating your life.").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "You can end up spending too much time on [UserPrioLast] and not enough on [UserPrioFirst].").SetBtn("NEXT"),
                //    new SeqStep("If you let life dictate your priorities, instead of your priorities dictating your life, you'll end up making poorer decisions, and this is what your priorities will look like...", "NEXT"),
                //    new SeqStep("[filled in by func]", "NEXT").SetFuncToCallName("SetCharSpeechToPriorities1"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "*bring bring!*").SetBtn("NEXT"),
                new SeqStep(Chars.Narrator, "Oh, dear. I just got a call from N, E, and T. Remember when you inserted X into NET earlier? Well, NET says it doesn't want to be friends with X anymore.").SetBtn("NET"),
                new SeqStep(Chars.Narrator, "What a tragedy. Maybe X will find some nice friends soon.").SetBtn("NET"),
                new SeqStep(Chars.Narrator, "All right! Are you ready for...!").SetBtn("NET"),
                new SeqStep(Chars.Narrator, "...I'm sorry, seeing NET without the X is too tragic for me. Can you pick a different word to advance this text?").SetBtn("NO"),
                new SeqStep(Chars.Narrator, "No? Is that really the word you're gonna go with?").SetBtn("NO"),
                new SeqStep(Chars.Narrator, "Hmm.").SetBtn("NO"),
                new SeqStep(Chars.Narrator, "You know what, don't let me stop you from living your best life.").SetBtn("NO"),
                new SeqStep(Chars.Narrator, "That's the spirit.").SetBtn("NO"),
            }),
        }),
        
        
        
        // ___________________________________________
        //  CHAPTER 2: The Priority Tourney
        // ___________________________________________
        new SeqChapter(new SeqChunk[]{
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Let me check my watch real quick...").SetBtn("NEXT"),
                new SeqStep().SetFunc("ShowTourneyTimeWatchAnim"),
                new SeqStep(Chars.Narrator, "Whoa, it's Tourney Time already?? That was quick.\n\nAll right, let's get this show on the road!").SetBtn("OH OK"),
            }),
            
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("BeginTourneySequence"),
                new SeqStep(Chars.Trny0, "Welcome to The Priority Tourney! I’m your host, Olivia Speedbottom.", "Announcer0").SetBtn("NEXT"),
                new SeqStep(Chars.Trny1, "And I’m Olivia’s best friend, Candice Rachel Matthews! We met at summer camp!", "BestFriend0").SetBtn("NEXT"),
                new SeqStep().SetFunc("Trny_RevealPrioCandidates"),
                new SeqStep(Chars.Trny0, "In this Tourney, SIX Priorities will enter.... but only ONE will leave as CHAMPION.").SetBtn("NEXT"),
                // TO DO: Populate brackets with Priorities, and show ?-crown blinking on top of all of them.
                new SeqStep(Chars.Trny1, "I like fishing!").SetBtn("NEXT"),//TO DO: Hide blinking crowns
                new SeqStep(Chars.Trny0, "There are 5 total battles, where we’ll pit Priority against Priority.").SetBtn("NEXT"),
                new SeqStep(Chars.Trny0, "A fair warning for those with light stomachs... Things might get ugly.").SetBtn("NEXT"),
                new SeqStep(Chars.Trny1, "I speak four languages, and I can play five instruments! Follow me on Instagram!").SetBtn("NEXT"),
                new SeqStep(Chars.Trny0, "Now, while all these priorities of course matter...").SetBtn("NEXT"),
                new SeqStep(Chars.Trny1, "My dads own a grand piano! You’ll never guess what color it is!").SetBtn("NEXT"),
                new SeqStep(Chars.Trny0, "...Only ONE will emerge as the CHAMPION.").SetBtn("BEGIN TOURNEY"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("Trny_ShowUpcomingBattleBrackets").SetBtn("BEGIN"),
                new SeqStep().SetFunc("Trny_BeginNextBattle"),
                new SeqStep().SetFunc("Trny_ShowUpcomingBattleBrackets").SetBtn("BEGIN"),
                new SeqStep().SetFunc("Trny_BeginNextBattle"),
                new SeqStep().SetFunc("Trny_ShowUpcomingBattleBrackets").SetBtn("BEGIN"),
                new SeqStep().SetFunc("Trny_BeginNextBattle"),
                new SeqStep().SetFunc("Trny_ShowUpcomingBattleBrackets").SetBtn("BEGIN"),
                new SeqStep().SetFunc("Trny_BeginNextBattle"),
                new SeqStep().SetFunc("Trny_ShowUpcomingBattleBrackets").SetBtn("BEGIN"),
                new SeqStep().SetFunc("Trny_BeginNextBattle"),
                new SeqStep().SetFunc("Trny_RevealFinalResults").SetBtn("NEXT"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Trny0, "There you have it, folks. End of tourney. Winner: [UserPrioFirst].", "Announcer0").SetBtn("NEXT"),
            }),
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Trny0, "Press NEXT to close Priority Tourney.").SetBtn("NEXT"),
                new SeqStep().SetFunc("EndTourneySequence"),
            }),
        }),
        
        
        
        // ___________________________________________
        //  CHAPTER 3
        // ___________________________________________
        new SeqChapter(new SeqChunk[]{
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Tourney is over!").SetBtn("COOL"),
                new SeqStep(Chars.Narrator, "That's it, folks.").SetBtn("COOL"),
            }),
            
            new SeqChunk(new SeqStep[]{
                new SeqStep(Chars.Narrator, "Now, let's order them manually!\nPlease order your items by importance.").SetBtn("TO DO: This"),
                new SeqStep(Chars.Narrator, "TO DO: Player orders priorities list. UP/DOWN arrows on each priority button.").SetBtn("DONE"),
                new SeqStep(Chars.Narrator, "Oh, yeah. [UserPrioLast] is WAY less important than [UserPrioFirst].").SetBtn("NEXT"),
            }),
        }),
    };
    /*
        new SeqChapter({
      //new SeqSect(new SeqStep[]{
      //  new SeqStep("One", "START"),
      //  new SeqStep("TAP things to keep them in the air.", "321 GO"),
      //  new SeqStep("", "").SetFuncToCallName("StartJugglingGame"),
      //  new SeqStep("", "").SetFuncToCallName("OpenPremadePriosChoices"),
      //  new SeqStep("[CharSpeech_PremadePrios]", "I... I...."),
      //  new SeqStep("It's tough only picking a few, huh?", "YEAH"),
      //  new SeqStep("", "").SetFuncToCallName("OpenPriosEntry"),
      //  new SeqStep("Roger that!", "NEXT").SetFuncToCallName("ClosePriosEntry"),
      //  new SeqStep("I especially like how you added [UserPrioSecondLast] and [UserPrioLast].", "NEXT"),
      //  new SeqStep("Two", "NEXT"),
      //}),
    
      //new SeqSect(new SeqStep[]{
      //  new SeqStep("1-1", "11, MM"),
      //  new SeqStep("1-2! Yea, done!", "OK"),
      //}),
      
      //*
      
      new SeqChunk(new SeqStep[]{
        new SeqStep("- end of content for now-", ""),
      }),
    }
    );
    */
    
    /*
        // CHAPTER 1
    new SeqChapter(new SeqChunk[]{
        new SeqChunk(new SeqStep[]{
            new SeqStep("AAA Test 0-0-0", "NEXT").SetCharImgName("Bratt0"),
            new SeqStep("BBB 0-0-1", "NEXT").SetCharImgName("Bratt1"),
        }),
        new SeqChunk(new SeqStep[]{
            new SeqStep("CCC Test 0-1-0", "NEXT").SetCharImgName("Bratt0"),
            new SeqStep("DDD 0-1-1", "NEXT").SetCharImgName("Bratt1"),
        }),
    }),
    
    // CHAPTER 2
    new SeqChapter(new SeqChunk[]{
        new SeqChunk(new SeqStep[]{
            new SeqStep("EEE Test 1-0-0", "NEXT").SetCharImgName("Bratt0"),
            new SeqStep("FFF 1-0-1", "NEXT").SetCharImgName("Bratt1"),
        }),
        new SeqChunk(new SeqStep[]{
            new SeqStep("GGG Test 10-1-0", "NEXT").SetCharImgName("Bratt0"),
            new SeqStep("HHH 1-1-1", "NEXT").SetCharImgName("Bratt1"),
        }),
    }),
    */
}
