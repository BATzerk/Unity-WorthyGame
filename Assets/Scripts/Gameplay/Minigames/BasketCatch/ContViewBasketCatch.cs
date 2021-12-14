using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigameNamespace.BasketCatchNS {
    public class ContViewBasketCatch : ContestantView {
        // Components
        [SerializeField] private Rigidbody2D myRigidbody=null;
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetPhysicsEnabled(bool val) {
            myRigidbody.bodyType = val ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }
        
    }
}