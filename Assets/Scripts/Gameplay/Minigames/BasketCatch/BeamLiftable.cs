using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.BasketCatchNS {
    public class BeamLiftable : MonoBehaviour {
        // Components
        private Image i_body; // set in Awake.
        private Rigidbody2D myRigidbody; // set in Awake.
        // Properties
        private bool canBeCaught; // set to TRUE if we have a ContViewBasketCatch component, too!
        private bool isCaught;
        private bool isInCatchArea;
        private Color c_bodyNeutral;
        private float catchLoc;
        // References
        private BasketCatch myBasketMinigame; // set in Awake.
        
        // Getters (Private)
        private Basket basket { get { return myBasketMinigame.Basket; } }


        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        private void Awake() {
            canBeCaught = GetComponent<ContViewBasketCatch>() != null;
            myRigidbody = GetComponent<Rigidbody2D>();
            myBasketMinigame = GetComponentInParent<BasketCatch>();
            i_body = GetComponentInChildren<Image>();
            if (i_body != null) {
                c_bodyNeutral = i_body.color;
            }
        }


        // ----------------------------------------------------------------
        //  Physics Events
        // ----------------------------------------------------------------
        private void OnTriggerStay2D(Collider2D collision) {
            if (collision.tag == Tags.MG_BasketCatch_Beam) {
                //myRigidbody.velocity *= 0.9f;
                myRigidbody.velocity += new Vector2(0, 0.8f);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (myBasketMinigame.IsComplete) { return; } // Safety check.
            if (collision.tag == Tags.MG_BasketCatch_CatchArea) {
                isInCatchArea = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.tag == Tags.MG_BasketCatch_CatchArea) {
                isInCatchArea = false;
            }
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (!isCaught && canBeCaught) {
                // Hacky safety check.
                if (isInCatchArea && myBasketMinigame.IsComplete) { isInCatchArea = false; }
                
                if (isInCatchArea) {// && basket.IsBeamOn) {
                    catchLoc += 0.02f;
                    if (catchLoc >= 1) {
                        GetCaught();
                    }
                }
                else {
                    catchLoc = Mathf.Max(0, catchLoc-0.1f);
                }
                if (i_body != null) {
                    i_body.color = Color.Lerp(c_bodyNeutral, Color.white, catchLoc*0.7f);
                }
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void GetCaught() {
            isCaught = true;
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.transform.SetParent(basket.transform); // lock me in place in the basket.
            this.transform.SetAsFirstSibling(); // put behind other sprites.
            myRigidbody.velocity = Vector2.zero;
            myBasketMinigame.OnBeamLiftableGetCaught(this);
        }


    }
}