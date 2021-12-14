namespace MinigameNamespace {
    
    // NOTE: Contestant class only still exists 'cause it'd be more work than I think it's worth to remove it.
    [System.Serializable]
    public class Contestant {
        // Readonlies
        static public readonly Contestant undefined = new Contestant(Priority.undefined);
        // Properties
        public bool HasPartner=false;
        // References
        public Priority myPrio;
        
        // Getters (Public)
        public string PrioNameStyled { get { return myPrio.NameStyled; } }
        
        // Initialize
        public Contestant(Priority myPrio) {
            this.myPrio = myPrio;
        }
    }



////TO DO: REMOVE Contestant class. Only use priorities.
//    [System.Serializable]
//    public class Contestant {
//        // Readonlies
//        static public readonly Contestant undefined = new Contestant(Priority.undefined);
//        // Properties
//        public bool HasPartner=false;
//        // References
//        //public Contestant MyPartner; // who I'm in an exclusive, committed relationship with (set from a match-making minigame).
//        public Priority myPrio;
        
//        // Getters (Public)
//        public string PrioNameStyled { get { return myPrio.NameStyled; } }
        
        
//        // Initialize
//        public Contestant(Priority myPrio) {
//            this.myPrio = myPrio;
//        }
        
        
//        // Basic Getters
//        public override string ToString() { return myPrio.text; }
//        public override bool Equals(object o) { return object.ReferenceEquals(this, o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
//        public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
        
//        //public static bool operator == (Contestant a, Contestant b) {
//        //    return a!=null && a.Equals(b);
//        //}
//        //public static bool operator != (Contestant a, Contestant b) {
//        //    return a!=null && !a.Equals(b);
//        //}
//    }
    
    
    public class ContCouple {
        // References
        public Contestant ContA { get; private set; }
        public Contestant ContB { get; private set; }
        
        // Initialize
        public ContCouple(Contestant contA, Contestant contB) {
            this.ContA = contA;
            this.ContB = contB;
            // I now pronounce you Contestant and Contestant.
            //ContA.MyPartner = ContB;
            //ContB.MyPartner = ContA;
            ContA.HasPartner = true;
            ContB.HasPartner = true;
        }
    }
    
    
}