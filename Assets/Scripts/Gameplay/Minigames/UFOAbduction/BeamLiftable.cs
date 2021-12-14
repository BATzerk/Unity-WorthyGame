using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.UFOAbductionNS {
    public class BeamLiftable : BaseViewElement {
        // Components
        private Image i_body; // set in Awake.
        private Rigidbody2D myRigidbody; // set in Awake.
        // Properties
        private bool isAbducted; // NOTE: Not useful anymore! We just disable my whole GO now.
        private bool isInAbductionArea;
        private Color c_bodyNeutral;
        private float abductionLoc;
        // References
        private UFOAbduction myUFOMinigame; // set in Awake.
        
        // Getters (Private)
        private UFOAbductionShip ship { get { return myUFOMinigame.Ship; } }


        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        override protected void Awake() {
            base.Awake();
            myRigidbody = GetComponent<Rigidbody2D>();
            myUFOMinigame = GetComponentInParent<UFOAbduction>();
            i_body = GetComponentInChildren<Image>();
            if (i_body != null) {
                c_bodyNeutral = i_body.color;
            }
        }


        // ----------------------------------------------------------------
        //  Physics Events
        // ----------------------------------------------------------------
        private void OnTriggerStay2D(Collider2D collision) {
            if (collision.tag == Tags.MG_UFO_Beam) {
                myRigidbody.velocity *= 0.9f;
                //this.transform.localPosition += new Vector3(0, 5);
                myRigidbody.velocity += new Vector2(0, 0.3f);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (myUFOMinigame.IsComplete) { return; } // Safety check.
            if (collision.tag == Tags.MG_UFO_AbductionArea) {
                isInAbductionArea = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.tag == Tags.MG_UFO_AbductionArea) {
                isInAbductionArea = false;
            }
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (!isAbducted) {
                // Hacky safety check.
                if (isInAbductionArea && myUFOMinigame.IsComplete) { isInAbductionArea = false; }
                
                if (isInAbductionArea && ship.IsBeamOn) {
                    abductionLoc += 0.02f;
                    if (abductionLoc >= 1) {
                        GetAbducted();
                    }
                }
                else {
                    abductionLoc = Mathf.Max(0, abductionLoc-0.1f);
                }
                if (i_body != null) {
                    i_body.color = Color.Lerp(c_bodyNeutral, Color.white, abductionLoc*0.7f);
                }
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void GetAbducted() {
            isAbducted = true;
            //myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            //myRigidbody.angularVelocity = 0;
            //myRigidbody.velocity = Vector2.zero;
            //this.transform.SetParent(ship.transform);
            //AnchoredPos = new Vector2(0,0);
            SetVisible(false);
            myUFOMinigame.OnBeamLiftableGetAbducted(this);
        }


    }
}