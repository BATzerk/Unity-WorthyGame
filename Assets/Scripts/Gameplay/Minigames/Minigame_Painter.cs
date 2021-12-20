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
        [SerializeField] private RectTransform[] rt_masterpieces; // includes the frame and canvas cloth.
        [SerializeField] private RectTransform[] rt_masterpieceThumbnailPoses; // where we tween them each to.
        //private LineRenderer currBrushLine;
        //private List<LineRenderer> brushLines=new List<LineRenderer>();
        private ImageLines currBrushLine;
        private List<ImageLines> brushLines = new List<ImageLines>();
        // Properties
        private bool isBrushDown;
        private bool mayPaint;
        private Vector2 brushPosPrev;
        private int currMasterpieceIndex;
        private const int NumMasterpiecesRequired = 3;
        private Color brushColor = new Color(0.1f, 0.1f, 0.1f);
        private float brushThickness = 4;
        // References
        [SerializeField] private LineRenderer prefab_brushLine;
        private RectTransform currMasterpieceRT;


        // Getters
        public override string MyWorthyNoun { get { return "Praiseworthy"; } }
        private Vector2 GetBrushPos() {
            return InputController.Instance.MousePosCanvasCentered + new Vector2(0, 60); // HARDCODED offset.
        }
        private bool IsMouseOverCanvasArea() {
            return rt_canvasPaint.rect.Contains(GetBrushPos());
        }



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            t_header.enabled = true;
            isBrushDown = false;
            worthyMeter.Show();
            worthyMeter.SetPercentFull(1);
            mayPaint = true;
            SetCurrMasterpieceIndex(0);
        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetCurrMasterpieceIndex(int index) {
            currMasterpieceIndex = index;
            currMasterpieceRT = rt_masterpieces[currMasterpieceIndex];
            int numLeft = NumMasterpiecesRequired - currMasterpieceIndex;
            t_header.text = "Paint " + numLeft + " masterpiece" + (numLeft==1?"":"s") + ".";
            UpdateButtonsInteractable();
        }

        private void StartNewBrushStroke() {
            //LineRenderer newObj = Instantiate(prefab_brushLine).GetComponent<LineRenderer>();
            //GameUtils.ParentAndReset(newObj.gameObject, currMasterpieceRT);
            //newObj.name = "BrushStroke";
            //currBrushLine = newObj;
            //brushLines.Add(newObj);
            //UpdateButtonsInteractable();
            ImageLines newObj = Instantiate(ResourcesHandler.Instance.ImageLines).GetComponent<ImageLines>();
            GameUtils.ParentAndReset(newObj.gameObject, currMasterpieceRT);
            newObj.name = "BrushStroke";
            newObj.SetColor(brushColor);
            newObj.SetThickness(brushThickness);
            currBrushLine = newObj;
            brushLines.Add(newObj);
            UpdateButtonsInteractable();
        }
        private void ContinueBrushStroke(Vector2 pos) {
            currBrushLine.AddPoint(pos);
            //currBrushLine.positionCount++;
            //currBrushLine.SetPosition(currBrushLine.positionCount - 1, pos);
        }
        private void UpdateButtonsInteractable() {
            b_undo.interactable = brushLines.Count > 0;
            b_submitPainting.interactable = brushLines.Count > 0;
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
                ImageLines brushLine = brushLines[brushLines.Count-1];
                brushLines.RemoveAt(brushLines.Count - 1);
                Destroy(brushLine.gameObject);
            }
            UpdateButtonsInteractable();
        }
        public void OnClick_SubmitPainting() {
            StartCoroutine(Coroutine_SubmitPainting());
        }
        private IEnumerator Coroutine_SubmitPainting() {
            // Reset the brush strokes list!
            brushLines = new List<ImageLines>();
            // Do confetti burst, bro!
            minigameCont.confettiBursts.PlayBurst(MyWorthyNoun.ToUpper());
            mayPaint = false;
            UpdateButtonsInteractable();
            yield return new WaitForSeconds(2f);

            mayPaint = true;

            // Animate it up to the top area.
            Vector2 pos = rt_masterpieceThumbnailPoses[currMasterpieceIndex].position;
            LeanTween.move(currMasterpieceRT.gameObject, pos, 1f).setEaseInOutQuad();
            LeanTween.scale(currMasterpieceRT.gameObject, Vector3.one * 0.1f, 1f).setEaseInOutQuad();

            // Allow starting the next masterpiece!
            if (currMasterpieceIndex < NumMasterpiecesRequired-1) {
                SetCurrMasterpieceIndex(currMasterpieceIndex + 1);
            }
            else {
                t_header.text = "All masterpieces completed!";
                b_undo.gameObject.SetActive(false);
                b_submitPainting.gameObject.SetActive(false);
            }
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        private void Update() {
            // Input
            if (Input.GetMouseButtonDown(0)) {
                if (mayPaint && IsMouseOverCanvasArea()) {
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







//private void AddBrushStroke(Vector2 posALocal, Vector2 posBLocal) {
//    ImageLine newObj = Instantiate(ResourcesHandler.Instance.ImageLine).GetComponent<ImageLine>();
//    GameUtils.ParentAndReset(newObj.gameObject, rt_canvasPaint);
//    newObj.name = "BrushStroke";
//    Vector2 posA = posALocal - MainCanvas.Size * 0.5f;// / MainCanvas.Scaler.scaleFactor;
//    Vector2 posB = posBLocal - MainCanvas.Size * 0.5f;/// MainCanvas.Scaler.scaleFactor;
//    //newObj.transform.localPosition -= new Vector3(rt_canvasPaint.rect.width, rt_canvasPaint.rect.height) * 0.5f; // offset for global position.
//    newObj.Initialize(posA, posB);
//    newObj.SetColor(brushColor);
//    newObj.SetThickness(brushThickness);
//    brushLines.Add(newObj);

//    UpdateButtonsInteractable();
//}


//private void ClearMyCanvas() {
//    //int numChildren = rt_canvasPaint.childCount;
//    //for (int i = numChildren-1; i>=0; --i) {
//    //    Destroy(rt_canvasPaint.GetChild(i).gameObject);
//    //}
//    for (int i=brushLines.Count-1; i>=0; --i) {
//        Destroy(brushLines[i].gameObject);
//    }
//    brushLines = new List<LineRenderer>();
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