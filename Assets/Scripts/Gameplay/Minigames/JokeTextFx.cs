using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeTextFx : MonoBehaviour {
    // Components
    [SerializeField] private CanvasGroup myCG;
    [SerializeField] private RectTransform myRT;



    public void Initialize(RectTransform parentTF) {
        GameUtils.ParentAndReset(this.gameObject, parentTF);


        Vector2 pos = new Vector2(Random.Range(-120, 120), Random.Range(-120, 120));
        float rot = Random.Range(-10, 10);
        myRT.localPosition = pos;
        myRT.localEulerAngles = new Vector3(0,0,rot);
        LeanTween.moveLocal(this.gameObject, pos+new Vector2(0,120), 1f).setEaseInBack();
        LeanTween.rotateZ(this.gameObject, rot+Random.Range(-30,30), 1f).setEaseInCubic();
        LeanTween.alphaCanvas(myCG, 0, 1f);
        Invoke("DestroySelf", 1);
    }


    private void DestroySelf() {
        Destroy(this.gameObject);
    }


}
