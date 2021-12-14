using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailFeedbackPopup : BasePopup {
    public override PopupIDs MyID() { return PopupIDs.EmailFeedback; }
    
    public void OnClick_Yes() {
        string emailTo = "brett@mydogzorro.com";
        string subject = "Priorities Game Feedback v" + Application.version;
        string body = "";
        GameUtils.SendEmail(emailTo, subject, body);
        Close();
    }
    //public void OnClick_
}
