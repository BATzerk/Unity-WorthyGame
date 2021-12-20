using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_Painter : Minigame {
        // Components
        [SerializeField] private TextMeshProUGUI t_header;
        [SerializeField] private Button b_undo;
        [SerializeField] private Button b_submitPainting;
        [SerializeField] private RectTransform rt_canvasPaint; // where the paint is applied.
        private LineRenderer currBrushLine;
        private List<LineRenderer> brushLines=new List<LineRenderer>();
        // Properties
        private bool isBrushDown;
        private Vector2 brushPosPrev;
        // References
        [SerializeField] private LineRenderer prefab_brushLine;


        // Getters
        private Vector2 GetBrushPos() {
            return InputController.Instance.MousePosCanvas;
        }
        private bool IsMouseOverCanvas() {
            return rt_canvasPaint.rect.Contains(GetBrushPos());
        }



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            t_header.enabled = true;
            isBrushDown = false;

            //ShowNextButton("READY");
        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void ClearMyCanvas() {
            //int numChildren = rt_canvasPaint.childCount;
            //for (int i = numChildren-1; i>=0; --i) {
            //    Destroy(rt_canvasPaint.GetChild(i).gameObject);
            //}
            for (int i=brushLines.Count-1; i>=0; --i) {
                Destroy(brushLines[i].gameObject);
            }
            brushLines = new List<LineRenderer>();
        }
        private void StartNewBrushStroke() {
            LineRenderer newObj = Instantiate(prefab_brushLine).GetComponent<LineRenderer>();
            GameUtils.ParentAndReset(newObj.gameObject, rt_canvasPaint);
            newObj.name = "BrushStroke";
            currBrushLine = newObj;
            brushLines.Add(newObj);
        }
        private void ContinueBrushStroke(Vector2 pos) {
            currBrushLine.positionCount++;
            currBrushLine.SetPosition(currBrushLine.positionCount-1, pos);
        }



        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            HideNextButton();
            t_header.enabled = false;

            minigameCont.PlayAnim_321Go();
        }
        public void OnClick_Undo() {
            if (brushLines.Count > 0) {
                LineRenderer brushLine = brushLines[brushLines.Count-1];
                brushLines.RemoveAt(brushLines.Count-1);
                Destroy(brushLine.gameObject);
            }
        }
        public void OnClick_SubmitPainting() {
            // Clear the canvas!
            ClearMyCanvas();
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Input
            if (Input.GetMouseButtonDown(0)) {
                if (IsMouseOverCanvas()) {
                    isBrushDown = true;
                    brushPosPrev = GetBrushPos();
                    StartNewBrushStroke();
                }
            }
            else if (Input.GetMouseButtonUp(0)) {
                isBrushDown = false;
            }

            if (isBrushDown) {
                Vector2 brushPos = GetBrushPos();
                if (Vector2.Distance(brushPosPrev, brushPos) > 5f) {
                    ContinueBrushStroke(GetBrushPos());
                    brushPosPrev = GetBrushPos();
                }
            }
        }



    }
}






//private Color brushColor = new Color(0.1f, 0.1f, 0.1f);
//private float brushThickness = 4;


//private void AddBrushStroke(Vector2 posALocal, Vector2 posBLocal) {
//ImageLine newObj = Instantiate(ResourcesHandler.Instance.ImageLine).GetComponent<ImageLine>();
//GameUtils.ParentAndReset(newObj.gameObject, rt_canvasPaint);
//newObj.name = "BrushStroke";
//Vector2 posA = posALocal - MainCanvas.Size*0.5f;// / MainCanvas.Scaler.scaleFactor;
//Vector2 posB = posBLocal - MainCanvas.Size*0.5f;/// MainCanvas.Scaler.scaleFactor;
////newObj.transform.localPosition -= new Vector3(rt_canvasPaint.rect.width, rt_canvasPaint.rect.height) * 0.5f; // offset for global position.
//newObj.Initialize(posA, posB);
//newObj.SetColor(brushColor);
//newObj.SetThickness(brushThickness);
//}



//public void OnMyCanvasPointerDown() {
//    isBrushDown = true;
//    brushPosPrev = GetBrushPos();
//}
//public void OnMyCanvasPointerUp() {
//    isBrushDown = false;
//}
//public void OnMyCanvasPointerMove() {
//    if (isBrushDown) {
//        AddBrushStroke(brushPosPrev, GetBrushPos());
//        brushPosPrev = GetBrushPos();
//    }
//}