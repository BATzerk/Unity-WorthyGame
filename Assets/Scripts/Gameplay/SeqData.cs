
// -------- CHAPTER ------------------------
public class SeqChapter {
    // Properties
    //public int index;// { get; private set; } // index in the content array.
    public SeqAddress MyAddr;
    public SeqChunk[] chunks { get; private set; }
    // Getters
    public int NumChunks { get { return chunks.Length; } }
    // Initialize
    public SeqChapter(SeqChunk[] chunks) {
        this.chunks = chunks;
    }
}

// -------- CHUNK ------------------------
public class SeqChunk {
    // Properties
    //public int index;// { get; private set; } // index in the content array.
    public SeqAddress MyAddr;
    public SeqStep[] steps { get; private set; }
    // Getters
    public int NumSteps { get { return steps.Length; } }
    // Initialize
    public SeqChunk(SeqStep[] steps) {
        this.steps = steps;
    }
}

// -------- STEP ------------------------
public class SeqStep {
    // Properties
    //public int index;// { get; private set; } // index in the content array.
    public SeqAddress MyAddr;
    public Chars myChar { get; private set; }
    public RoundData mgRoundData { get; private set; }
    public string speechText { get; private set; }
    public string nextBtnText { get; private set; }
    public string charImgName { get; private set; }
    public string funcToCallName { get; private set; }
    public string mainStoryKnot { get; private set; }
    
    // Setters
    //public SeqStep SetCharImgName(string val) {
    //    charImgName = val;
    //    return this;
    //}
    public SeqStep SetBtn(string val) {
        nextBtnText = val;
        return this;
    }
    public SeqStep SetFunc(string val) {
        funcToCallName = val;
        return this;
    }
    public SeqStep SetDialogueTree(string val) {
        mainStoryKnot = val;
        return this;
    }
    public SeqStep SetMinigameRound(int roundIndex, params string[] mgNames) {
        mgRoundData = new RoundData(roundIndex, mgNames);
        return this;
    }
    //public SeqStep SetMinigameRound(RoundData roundData) {//PrioRank rank, params string[] mgNames) {
    //    mgRoundData = roundData;
    //    return this;
    //}
    
    // Initialize!
    public SeqStep() {
        this.speechText = "";
        this.nextBtnText = "";
    }
    public SeqStep(Chars myChar, string speechText,string charImgName=null) {
        this.myChar = myChar;
        this.speechText = speechText;
        this.charImgName = charImgName;
    }
}

