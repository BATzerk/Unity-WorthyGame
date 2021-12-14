using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class is just to handle animation-complete events. */
public class RandoAnims : MonoBehaviour {
    
    public void OnAnimComplete() {
        //Debug.Log("YEAAAA boiiii");
        FindObjectOfType<GameController>().AdvanceSeqStep();
    }
    
}
