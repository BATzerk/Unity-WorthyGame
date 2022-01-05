using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinigameNamespace {
    public class BubbleBath_Hose : MonoBehaviour {
        // Components
        [SerializeField] private RectTransform myRT;
        [SerializeField] private RectTransform rt_nozzlePos;
        [SerializeField] private ParticleSystem ps_waterTest;
        // Properties
        private bool isFlowing;
        // References
        [SerializeField] private GameObject prefab_waterDroplet;
        [SerializeField] private RectTransform rt_bathContentsUnder;
        [SerializeField] private RectTransform rt_bathContentsOver;


        private Vector2 GetMousePos() {
            Vector2 pos = InputController.Instance.MousePosCanvasCentered;
            //pos /= rt_room.localScale.x; // scale it correctly.
            //pos += new Vector2(0, 10); // HARDCODED offset.
            return pos;
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Input
            if (Input.GetMouseButtonDown(0)) {
                isFlowing = true;
            }
            else if (Input.GetMouseButtonUp(0)) {
                isFlowing = false;
            }

            if (isFlowing) {
                // Put hydrant in the right spot!
                Vector2 mousePos = GetMousePos();
                myRT.localPosition = mousePos;

                if (Time.frameCount % 2 == 0) {
                    SpewAWaterDroplet();
                }
            }
        }


        private void SpewAWaterDroplet() {
            //ps_waterTest.Emit(1);
            BubbleBath_WaterDroplet obj = Instantiate(prefab_waterDroplet).GetComponent<BubbleBath_WaterDroplet>();
            obj.Initialize(rt_bathContentsOver, rt_bathContentsUnder, rt_nozzlePos);
        }
    }
}