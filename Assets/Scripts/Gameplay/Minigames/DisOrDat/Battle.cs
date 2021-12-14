using UnityEngine;

namespace MinigameNamespace.DisOrDatNS {
    [System.Serializable]
    public class Battle {
        // Constants
        static public readonly Battle undefined = new Battle(null,null);
        // Properties
        public Contestant ContestantA;// { get; private set; }
        public Contestant ContestantB;// { get; private set; }
        
        // Initialize
        public Battle(Contestant contA,Contestant contB) {
            this.ContestantA = contA;
            this.ContestantB = contB;
        }
    }
    
    
    [System.Serializable]
    public class SillyBattle : Battle {
        public SillyBattle(string nameA, string nameB) : base(new Contestant(new Priority(nameA)), new Contestant(new Priority(nameB))){
        }
    }
    
}