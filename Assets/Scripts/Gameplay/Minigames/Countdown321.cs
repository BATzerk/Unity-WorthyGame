using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown321 : MonoBehaviour {
    // Components
    [SerializeField] private ParticleSystem ps_confetti0;
    [SerializeField] private ParticleSystem ps_confetti1;
    // References
    [SerializeField] private MinigameController minigameController;


    // Events
    public void OnCountdownComplete() {
        // Tell MinigameController
        minigameController.OnCountdownComplete();

        // Particle bursts!
        ps_confetti0.Emit(100);
        ps_confetti1.Emit(100);
    }
}
