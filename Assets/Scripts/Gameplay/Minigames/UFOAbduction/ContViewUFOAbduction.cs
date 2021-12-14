using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigameNamespace.UFOAbductionNS {
    public class ContViewUFOAbduction : ContestantView {
        // Components
        //[SerializeField] private BoxCollider2D myCollider;
        [SerializeField] private Rigidbody2D myRigidbody=null;
        
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetPhysicsEnabled(bool val) {
            //myCollider.enabled = val;
            myRigidbody.bodyType = val ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

        
        
        
        
    }
}