using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace ElimigameNS {
    abstract public class Elimigame : BaseViewElement, IStorySource {
        // Properties
        abstract protected int NumConts { get; }
        private Story myStory;
        // References
        [SerializeField] protected Animator myAnimator=null;
        [SerializeField] private TextAsset myStoryText=null;
        protected List<Contestant> conts;
        protected ElimigameController myEGCont { get; private set; }
        
        // Getters (Public)
        //public TextAsset GetMyStoryText() { return myStoryText; }
        // Getters (Protected)
        protected bool IsOpen { get { return this.gameObject.activeInHierarchy; } }
        protected BranchingStoryController StoryCont { get { return myEGCont.StoryCont; } }

        // Interface
        virtual public string FillInBlanks(string str) { return str; } // Override me!
        public void ResetMyStory() {
            myStory = new Story(myStoryText.text);
        }
        public Story MyStory { get { return myStory; } }
        virtual public bool DoFuncFromStory(string line) {
            if (line.StartsWith("Anim_", System.StringComparison.InvariantCulture)) {
                string animName = line.Substring(5).Trim();
                myAnimator.Play(animName);
                return true;
            }
            return false;
        }


        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        public void Initialize(ElimigameController myEGCont) {
            this.myEGCont = myEGCont;
            ResetMyStory();
            SetVisible(false);
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        virtual public void Open() {
            SetVisible(true);
            
            // Make contestants!
            conts = new List<Contestant>();
            for (int i=0; i<NumConts; i++) {
                conts.Add(new Contestant(userPrios[userPrios.Count-1-i])); // pull from the END of the prios list. Only bottom prios here.
            }
            
            StoryCont.SetCurrStorySource(this);
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnAnimFinished() {
            StoryCont.AdvanceStory();
        }
        protected void OnComplete() {
            myEGCont.OnElimigameComplete();
        }

    }
}