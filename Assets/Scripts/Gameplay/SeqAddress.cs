using UnityEngine;

[System.Serializable]
public struct SeqAddress {
	// Statics
	static public readonly SeqAddress undefined = new SeqAddress(-1, -1, -1);
	static public readonly SeqAddress zero = new SeqAddress(0, 0, 0);
	// Properties
    public int chapter;
	public int chunk;
	public int step;

	public SeqAddress (int chapter, int chunk, int step) {
        this.chapter = chapter;
		this.chunk = chunk;
		this.step = step;
	}

    //public LevelAddress NextLevel { get { return new LevelAddress(pack, level+1); } }
    //public LevelAddress PrevLevel { get { return new LevelAddress(pack, level-1); } }

	public override string ToString() { return chapter + "," + chunk + "," + step; }
	static public SeqAddress FromString(string str) {
		string[] array = str.Split(',');
		if (array.Length >= 3) {
			return new SeqAddress (int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
		}
		return SeqAddress.undefined; // Hmm.
	}

	public override bool Equals(object o) { return base.Equals (o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
	public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).

    public static bool operator == (SeqAddress a, SeqAddress b) {
        return a.Equals(b);
    }
	public static bool operator != (SeqAddress a, SeqAddress b) {
		return !a.Equals(b);
	}
    
    public static bool operator < (SeqAddress a, SeqAddress b) {
        return a.chapter<b.chapter || a.chunk<b.chunk || a.step<b.step;
    }
    public static bool operator > (SeqAddress a, SeqAddress b) {
        return a.chapter>b.chapter || a.chunk>b.chunk || a.step>b.step;
    }
}