using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_JokeTeller : Minigame {
        // Components
        [SerializeField] private Button b_tellJoke;
        [SerializeField] private TextMeshProUGUI t_header;
        [SerializeField] private TextMeshProUGUI t_timeLeft;
        // Properties
        private bool isTimerActive;
        private int numJokesTold = 0;
        private float timeLeft = 12;
        // References
        [SerializeField] private GameObject prefab_JokeTextFx;



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            t_header.text = "Tell as many jokes as possible to get people to like you.";
            isTimerActive = false;
            b_tellJoke.gameObject.SetActive(false);

            ShowNextButton("READY");
        }


        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            HideNextButton();
            t_header.text = "Tell jokes!";
            b_tellJoke.gameObject.SetActive(true);
            isTimerActive = true;
        }
        public void OnButtonClick_TellJoke() {
            numJokesTold++;
            // Add a joke text.
            JokeTextFx obj = Instantiate(prefab_JokeTextFx).GetComponent<JokeTextFx>();
            obj.Initialize(this.myRT);
        }

        private void OnOutOfTime() {
            timeLeft = 0;
            isTimerActive = false;
            b_tellJoke.gameObject.SetActive(false);
            t_header.text = "You told " + numJokesTold + " jokes.\n\nEveryone is in stitches!";
        }




        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            if (isTimerActive) {
                timeLeft -= Time.deltaTime;
                t_timeLeft.text = TextUtils.ToTimeString_ms(timeLeft);
                if (timeLeft <= 0) {
                    OnOutOfTime();
                }
            }
        }





    }
}
