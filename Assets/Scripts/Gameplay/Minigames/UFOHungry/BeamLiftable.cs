using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinigameNamespace.UFOHungryNS {
    public class BeamLiftable : BaseViewElement {
        // Components
        private Image i_body; // set in Awake.
        private Image i_highlight; // CREATED in Awake.
        private Outline outline; // set in Awake. Only CAKE has this.
        private Rigidbody2D myRigidbody; // set in Awake.
        // Properties
        public bool IsCake=false;
        private bool isInBeam;
        private bool isAbducted; // NOTE: Not useful anymore! We just disable my whole GO now.
        private bool isInAbductionArea;
        private Color c_bodyNeutral;
        private float abductionLoc;
        // References
        private UFOHungry myUFOMinigame; // set in Awake.
        
        // Getters (Private)
        private bool isBeingAbducted { get { return isInAbductionArea && ship.IsBeamOn; } }
        private Ship ship { get { return myUFOMinigame.Ship; } }


        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        //override protected void Awake() {
        //base.Awake();
        private void Start() {
            outline = GetComponent<Outline>();
            myRigidbody = GetComponent<Rigidbody2D>();
            myUFOMinigame = GetComponentInParent<UFOHungry>();
            i_body = GetComponentInChildren<Image>();
            if (i_body != null) {
                c_bodyNeutral = i_body.color;
                // Add i_highlight!
                GameObject newGO = new GameObject();
                GameUtils.ParentAndReset(newGO, this.transform);
                GameUtils.FlushRectTransform(newGO.AddComponent<RectTransform>());
                i_highlight = newGO.AddComponent<Image>();
                i_highlight.sprite = i_body.sprite;
                i_highlight.material = rh.m_Additive;
                i_highlight.name = "Highlight";
                i_highlight.enabled = false;
            }
        }


        // ----------------------------------------------------------------
        //  Physics Events
        // ----------------------------------------------------------------
        private void FixedUpdate() {
            SetIsInBeam(false);
        }
        private void OnTriggerStay2D(Collider2D collision) {
            if (collision.tag == Tags.MG_UFO_Beam) {
                SetIsInBeam(true);
                myRigidbody.velocity *= 0.9f;
                //this.transform.localPosition += new Vector3(0, 5);
                myRigidbody.velocity += new Vector2(0, 0.3f);
                myRigidbody.bodyType = RigidbodyType2D.Dynamic; // set to Dynamic (movable) once the beam hits me!
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
        
        private void SetIsInBeam(bool val) {
            isInBeam = val;
            if (i_highlight != null) {
                i_highlight.enabled = isInBeam;
            }
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (!isAbducted) {
                if (isBeingAbducted) {
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
                if (i_highlight != null) {
                    float alpha = abductionLoc;
                    if (isInBeam) { alpha = Mathf.Max(0.2f, alpha); }
                    GameUtils.SetUIGraphicAlpha(i_highlight, alpha);
                }
                UpdateOutlineColor();
            }
        }
        private void UpdateOutlineColor() {
            if (outline == null) { return; } // No outline? Do nada.
            if (isInBeam) {
                GameUtils.SetOutlineAlpha(outline, 0);
            }
            else {
                outline.effectColor = new Color255(255,0,216).ToColor();
                float alpha = MathUtils.SinRange(0.2f,0.8f, Time.time*12f);
                GameUtils.SetOutlineAlpha(outline, alpha);
            }
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void SetOutlineEnabled(bool val) {
            if (outline != null) { outline.enabled = val; }
        }
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