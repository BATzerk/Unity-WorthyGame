using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.UFOShootDownNS {
    public class Missile : BaseViewElement {
        // Components
        private Image i_body; // set in Awake.
        private Rigidbody2D myRigidbody; // set in Awake.
        // References
        //[SerializeField] private Sprite[] possibleSprites=null;
        private UFOShootDown myUFOMinigame;
        // Properties
        private bool didHitShip=false;
        
        // Getters (Private)
        private Ship ship { get { return myUFOMinigame.Ship; } }


        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        public void Initialize(UFOShootDown myUFOMinigame, RectTransform rt_parent, Vector2 _pos, int numMissilesFired) {
            myRigidbody = GetComponent<Rigidbody2D>();
            this.myUFOMinigame = myUFOMinigame;
            i_body = GetComponentInChildren<Image>();
            
            //i_body.sprite = possibleSprites[numMissilesFired%possibleSprites.Length];
            GameUtils.ParentAndReset(this.gameObject, rt_parent);
            AnchoredPos = _pos;
            myRigidbody.velocity = new Vector2(0, 11);
            myRigidbody.gravityScale = 0;
            myRigidbody.angularVelocity = Random.Range(-3f, 3f);
        }


        // ----------------------------------------------------------------
        //  Physics Events
        // ----------------------------------------------------------------
        private void OnCollisionEnter2D(Collision2D collision) {
            //if (myUFOMinigame.IsComplete) { return; } // Safety check.
            if (didHitShip) { return; } // Only register the FIRST hit, ok?
            
            if (collision.gameObject.GetComponentInParent<Ship>() != null) {
                didHitShip = true;
                myRigidbody.gravityScale = 0.3f; // once we hit it, enable gravity
                myRigidbody.angularVelocity = Random.Range(-500, 500);
                myUFOMinigame.OnMissileHitShip();
            }
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Offscreen? Destroy me.
            if (AnchoredPos.y > 1400 || AnchoredPos.y < -500) {
                Destroy(this.gameObject);
            }
        }


    }
}