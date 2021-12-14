//namespace DisOrDatNamespace {

//    public class Contestant {
//        static public readonly Contestant undefined = new Contestant(Priority.undefined);
//        //public enum Status { Undefined, Queued, Eliminated }
//        //public Status myStatus;
//        public int NumBattlesWon = 0;
//        public int NumBattlesLost = 0;
//        public int NumBattlesTied = 0;
//        public Priority myPrio { get; private set; }
//        public Contestant(Priority myPrio) {
//            this.myPrio = myPrio;
//            //this.myStatus = Status.Queued; // I'm ready for action!
//        }
//        //public void SetMyStatus(Status status) {
//        //    this.myStatus = status;
//        //}
        
//        public override string ToString() { return myPrio.text;}// + ". Status: " + myStatus; }
//        public override bool Equals(object o) { return object.ReferenceEquals(this, o); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
//        public override int GetHashCode() { return base.GetHashCode(); } // NOTE: Just added these to appease compiler warnings. I don't suggest their usage (because idk what they even do).
        
//        public static bool operator == (Contestant a, Contestant b) {
//            return a.Equals(b);
//        }
//        public static bool operator != (Contestant a, Contestant b) {
//            return !a.Equals(b);
//        }
//    }
    
//}