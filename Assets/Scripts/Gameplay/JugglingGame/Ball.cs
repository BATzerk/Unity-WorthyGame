using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JugglingGameNamespace {
    public class Ball : BaseViewElement {
        // Components
        [SerializeField] private TextMeshProUGUI t_myText=null;
        // Properties
        public bool isActivated { get; private set; } // if FALSE, we won't show or update until set to true.
        public bool IsDead { get; private set; }
        private int index;
        private Vector2 vel;
        private float boundsL,boundsR,boundsD,boundsU;
        // References
        private JugglingGame myJugGame;
        
        
        // ----------------------------------------------------------------
        //  Initialize
        // ----------------------------------------------------------------
        public void Initialize(JugglingGame myJugGame, int index) {
            this.myJugGame = myJugGame;
            this.index = index;
            boundsL = 80;
            boundsR = MainCanvas.Width - 80;
            boundsD = -100;
            boundsU = MainCanvas.Height + 50;
            
            // Parent jazz.
            GameUtils.ParentAndReset(this.gameObject, myJugGame.transform);
            
            t_myText.text = userPrios[index].text;
            vel = new Vector2(Random.Range(-1, 1), Random.Range(1, 5));
            Rotation = Random.Range(-20f, 20f);
            myRT.anchoredPosition = new Vector2(Random.Range(boundsL,boundsR), Random.Range(0.6f,1f)*MainCanvas.Height);
            SetVisible(false);
        }
        
        
        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        public void Activate() {
            if (isActivated) { return; } // Safety check.
            isActivated = true;
            SetVisible(true);
            myJugGame.numBallsActivated++;
        }
        
        
        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        public void OnTapped() {
            vel.x = Random.Range(-4, 4);
            vel.y = 5;
            myJugGame.OnTappedBall();
        }
        private void OnDroppedOffscreen() {
            IsDead = true;
            myJugGame.OnDroppedBall(index);
            SetVisible(false);
        }

        
        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        public void FixedUpdate() {
            // Apply gravity
            vel += new Vector2(0, myJugGame.Gravity);
            // Apply vel
            AnchoredPos += vel;

            // Bounds
            if (AnchoredPos.x < boundsL) { AnchoredPos = new Vector2(boundsL,AnchoredPos.y); vel.x *= -1; }
            if (AnchoredPos.x > boundsR) { AnchoredPos = new Vector2(boundsR,AnchoredPos.y); vel.x *= -1; }
            if (AnchoredPos.y > boundsU) { AnchoredPos = new Vector2(AnchoredPos.x, boundsU); }
            if (AnchoredPos.y < boundsD) {
                OnDroppedOffscreen();
            }
        }
    }
}