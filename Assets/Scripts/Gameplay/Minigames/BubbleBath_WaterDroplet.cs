using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBath_WaterDroplet : MonoBehaviour
{
    // Components
    [SerializeField] private RectTransform myRT;
    [SerializeField] private Rigidbody2D myRigidbody2D;
    // References
    private RectTransform rt_parentUnder; // I'll swap to this once I cross the threshold of the bath rim.
    // Properties
    private bool hasCrossedBathRim;



    public void Initialize(RectTransform rt_parentOver, RectTransform rt_parentUnder, RectTransform rt_spawn) {
        this.rt_parentUnder = rt_parentUnder;

        GameUtils.ParentAndReset(this.gameObject, rt_parentOver);
        this.name = "WaterDroplet";
        float angle = rt_spawn.eulerAngles.z + Random.Range(-0.3f, 0.3f);
        float speed = Random.Range(6, 12);
        myRigidbody2D.velocity = new Vector2(Mathf.Cos(angle)*speed, Mathf.Sign(angle)*speed);
        myRT.position = rt_spawn.position;
        myRT.rotation = rt_spawn.rotation;
        Invoke("DestroySelf", 6);
    }
    private void DestroySelf() {
        Destroy(this.gameObject);
    }



    private void Update() {
        if (!hasCrossedBathRim) {
            if (myRT.anchoredPosition.y > -540) {
                hasCrossedBathRim = true;
                this.transform.parent = rt_parentUnder;
            }
        }
    }
}
