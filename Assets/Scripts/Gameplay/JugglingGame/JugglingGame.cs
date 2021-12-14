using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JugglingGameNamespace;

public class JugglingGame : BaseViewElement {
    // Properties
    private Ball[] balls=new Ball[0]; // made in Restart.
    private List<int> orderDropped; // list of indexes of balls dropped.
    public float Gravity;
    float durPlayed; // in SECONDS.
    private int numBallsTapped; // incremented every time we tap a Ball.
    public int numBallsActivated;


    // ----------------------------------------------------------------
    //  Restart
    // ----------------------------------------------------------------
    public void Restart() {
        orderDropped = new List<int>();
        Gravity = -0.07f; // start mild.
        durPlayed = 0;
        numBallsTapped = 0;
        numBallsActivated = 0;

        // Remake balls!
        RemakeBalls();
        
        // Show me!
        SetVisible(true);
    }
    private void RemakeBalls() {
        // Destroy 'em.
        if (balls != null) {
            for (int i=0; i<balls.Length; i++) { Destroy(balls[i].gameObject); }
        }
        
        // Make 'em!
        balls = new Ball[NumUserPrios];
        for (int i=0; i<balls.Length; i++) {
            balls[i] = Instantiate(rh.JugglingGame_Ball).GetComponent<Ball>();
            balls[i].Initialize(this, i);
        }
        // Start first Ball centered, and activated!
        balls[0].AnchoredPos = new Vector2(MainCanvas.Width*0.5f, MainCanvas.Height*0.75f);
        balls[0].Activate();
    }
    public void Close() {
        // Order priorities by jugglingGame outcome!
        //dm.OrderUserPrios(orderDropped);NOTE: Disabled this!
        SetVisible(false);
    }
    private void ActivateNextBall() {
        balls[numBallsActivated].Activate();
    }


    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnTappedBall() {
        numBallsTapped++;
        //    // Activate next ball, maybe!
        //    int nextIndex = floor(numBallsTapped*0.5);
        //    if (nextIndex < balls.length) {
        //      balls[nextIndex].Activate();
        //    }
        Gravity -= 0.005f;
    }
    public void OnDroppedBall(int ballIndex) {
        orderDropped.Insert(0, ballIndex); // insert at BEGINNING of list. so most important val is FIRST.
        if (orderDropped.Count >= balls.Length) {
            OnDroppedAllBalls();
        }
        //    Gravity -= 0.1f;
    }
    private void OnDroppedAllBalls() {
        // Real hacky finding.
        FindObjectOfType<GameController>().AdvanceSeqStep();
    }
    private void ForceDropBallsAlive() {
        for (int i=balls.Length-1; i>=0; i--) {
            if (!balls[i].IsDead) {
                balls[i].Activate();
                balls[i].Pos -= new Vector3(0, 500);
            }
        }
    }


    // ----------------------------------------------------------------
    //  FixedUpdate
    // ----------------------------------------------------------------
    public void FixedUpdate() {
        // Increment gravity!
        if (numBallsTapped > 0) {
            Gravity -= 0.0001f;
        }
        // Increment durPlayed.
        durPlayed += Time.deltaTime;
        // Maybe add a ball!
        if (numBallsActivated < balls.Length) {
            if (false) { }
            else if (numBallsActivated == 1 && durPlayed > 3) { ActivateNextBall(); }
            else if (numBallsActivated == 2 && durPlayed > 6) { ActivateNextBall(); }
            else if (numBallsActivated == 3 && durPlayed > 9) { ActivateNextBall(); }
            else if (numBallsActivated == 4 && durPlayed > 12) { ActivateNextBall(); }
            else if (durPlayed > 12) { ActivateNextBall(); }
        }
        // Maybe end the game!
        if (durPlayed > 18) {
            ForceDropBallsAlive();
        }
    }






}
