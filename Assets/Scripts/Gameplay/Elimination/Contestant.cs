namespace ElimigameNS {

    public class Contestant {
        // Readonlies
        static public readonly Contestant undefined = new Contestant(Priority.undefined);
        // References
        public Priority myPrio { get; private set; }
        
        // Initialize
        public Contestant(Priority myPrio) {
            this.myPrio = myPrio;
        }
        
        
        // Basic Getters
        public override string ToString() { return myPrio.text; }
        public override bool Equals(object o) { return object.ReferenceEquals(this, o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
        public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
        
        //public static bool operator == (Contestant a, Contestant b) {
        //    return a!=null && a.Equals(b);
        //}
        //public static bool operator != (Contestant a, Contestant b) {
        //    return a!=null && !a.Equals(b);
        //}
    }
    
}