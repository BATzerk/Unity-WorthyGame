using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfettiBursts : MonoBehaviour {
    // Components
    [SerializeField] private Animator myAnimator;
    [SerializeField] private ParticleSystem ps_confetti0;
    [SerializeField] private ParticleSystem ps_confetti1;
    [SerializeField] private TextMeshProUGUI t_header0;
    [SerializeField] private TextMeshProUGUI t_header1;

    // Events
    public void PlayBurst(string percentChange, string worthyNoun) {
        // Update texts.
        string str = "<size=200>" + percentChange + "</size>\n" + worthyNoun;
        t_header0.text = str;
        t_header1.text = str;

        // Animation!
        myAnimator.Play("Burst", -1, 0);

        // Particles!
        ps_confetti0.Emit(100);
        ps_confetti1.Emit(100);
    }
}
