//using UnityEngine;

//namespace DisOrDatNamespace {
//    public class Battle {
//        // Constants
//        static public readonly Battle undefined = new Battle(null,null);
//        // Properties
//        //public bool IsOver { get; private set; } // Starts false. Set to TRUE when battle is over and we set the outcome.
//        public Contestant ContestantA { get; private set; }
//        public Contestant ContestantB { get; private set; }
//        //public Contestant Winner { get; private set; }
//        //public Contestant Loser { get; private set; }
        
        
//        // Initialize
//        public Battle(Contestant contA,Contestant contB) {
//            this.ContestantA = contA;
//            this.ContestantB = contB;
//        }
        
//        //// Events
//        //public void OnBattleOver(PrioButton prioButtonSelected) {
//        //    IsOver = true;
//        //    if (prioButtonSelected != null) {
//        //        Winner = prioButtonSelected.MyContestant;
//        //        Loser = OtherContestant(Winner);
//        //    }
//        //}
        
        
        
        
//        public override string ToString() { return ContestantA.myPrio.text + " vs " + ContestantB.myPrio.text; }
//        public override bool Equals(object o) { return object.ReferenceEquals(this, o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
//        public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
        
//        //public static bool operator == (Battle a, Battle b) {
//        //    return a.Equals(b);
//        //}
//        //public static bool operator != (Battle a, Battle b) {
//        //    return !a.Equals(b);
//        //}
//    }
    
    
    
//    public class SillyBattle : Battle {
//        public SillyBattle(string nameA, string nameB) : base(new Contestant(new Priority(nameA)), new Contestant(new Priority(nameB))){
            
//        }
//    }
    
//}