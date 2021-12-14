using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.UFOShootDownNS {
    public class Ship : BaseViewElement {
        // Components
        [SerializeField] private Animator myAnimator=null;
        //[SerializeField] private Image i_body=null;
        // Properties
        public int Health { get; private set; } = 4;
        private Vector2 orbitCenter = new Vector2(0, -200);
        // References
        //[SerializeField] private UFOShootDown myUFOMinigame=null;
        

        // ----------------------------------------------------------------
        //  Start
        // ----------------------------------------------------------------
        public void Start() {
            UpdatePos(); // start in the right spot!
        }
        

        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            UpdatePos();
        }
        
        private void UpdatePos() {
            float radians = Time.time * 1.1f;
            AnchoredPos = orbitCenter + new Vector2(Mathf.Cos(radians)*130, Mathf.Sin(radians)*140);
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void GetHit() {
            Health -= 1;
            myAnimator.Play("GetHit", -1, 0);
        }
        
        
        
        
    }
}