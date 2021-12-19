using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Kind of a ha-cky class to handle all text content.
 * Any content/text that's not in Inky can be hardcoded in me, below.
 */
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



    // ----------------------------------------------------------------
    //  Content!
    // ----------------------------------------------------------------
    public readonly SeqChapter[] seqChapters = {
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
            //new SeqChunk(new SeqStep[]{
            //    new SeqStep(Chars.BlandPopup, "This game is an experiment.\n\nIt's a work in progress, and I'm happily collecting feedback and ideas!").SetBtn("NEXT"),
            //}),
            // GameIntro
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("ShowUserNameEntry"),
                new SeqStep().SetDialogueTree("GameIntro"),
            }),
            // Joke Teller
            new SeqChunk(new SeqStep[]{
                new SeqStep().SetFunc("OpenMinigame_JokeTeller"),
                new SeqStep().SetDialogueTree("PostJokeTeller"),
                new SeqStep().SetFunc("OpenMinigame_PickUpFromAirport"),
            }),
            
            /*
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
            */
        
        }),
    };


            
            
}
