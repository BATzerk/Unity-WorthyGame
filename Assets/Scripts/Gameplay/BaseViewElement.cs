using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseViewElement : MonoBehaviour {
    // Components
    public RectTransform myRT { get; private set; } // set in Awake.
    
    // Getters
    protected DataManager dm { get { return GameManagers.Instance.DataManager; } }
    protected UserData ud { get { return dm.UserData; } }
    protected EventManager em { get { return GameManagers.Instance.EventManager; } }
    protected ResourcesHandler rh { get { return ResourcesHandler.Instance; } }
    protected List<Priority> userPrios { get { return ud.userPrios; } }
    protected int NumUserPrios { get { return userPrios.Count; } }
    public bool IsVisible { get { return this.gameObject.activeInHierarchy; } }
    public Vector3 Pos {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    public Vector2 AnchoredPos {
        get { return myRT.anchoredPosition; }
        set { myRT.anchoredPosition = value; }
    }
    public float Rotation {
        get { return transform.localEulerAngles.z; }
        set { transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y, value); }
    }
    // Setters
    public void SetVisible(bool val) { this.gameObject.SetActive(val); }
    public void SetPos(Vector3 val) { Pos = val; }
    public void SetAnchoredPos(Vector2 val) { AnchoredPos = val; }
    
    // Awake
    virtual protected void Awake() {
        myRT = GetComponent<RectTransform>();
    }
    
}
