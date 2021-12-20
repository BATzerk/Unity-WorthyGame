using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_RunARace : Minigame {
        // Components
        [SerializeField] private TextMeshProUGUI t_header;
        // Properties
        private bool isTimerActive;
        private float timeLeft = 30;



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            t_header.enabled = true;
            isTimerActive = false;

            ShowNextButton("READY");
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            HideNextButton();
            t_header.enabled = false;

            minigameCont.PlayAnim_321Go();
        }
        override public void OnCountdownComplete() {
            isTimerActive = true;
        }

        private void OnOutOfTime() {
            timeLeft = 0;
            isTimerActive = false;
            //t_header.text = "Race complete";
        }




        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (isTimerActive) {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) {
                    OnOutOfTime();
                }
            }
        }





    }
}
