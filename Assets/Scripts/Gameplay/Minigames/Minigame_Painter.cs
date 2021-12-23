using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_Painter : Minigame {
        // Components
        [SerializeField] private Animator myAnimator;
        [SerializeField] private GameObject go_ui;
        [SerializeField] private Button b_undo;
        [SerializeField] private Button b_submitPainting;
        [SerializeField] private RectTransform rt_room; // so we know the scale of the room, to offset the brush pos.
        [SerializeField] private RectTransform[] rt_masterpieces; // includes the frame and canvas cloth.
        private ImageLines currBrushLine;
        private List<ImageLines> brushLines = new List<ImageLines>();
        // Properties
        private bool isBrushDown;
        private bool mayPaint;
        private Vector2 brushPosPrev;
        private int currMasterpieceIndex;
        private Color brushColor = new Color(0.1f, 0.1f, 0.1f);
        private float brushThickness = 4;
        private const float BrushSegmentMinLength = 2; // when the mouse moves this far, we add a new line segment.
        // References
        [SerializeField] private LineRenderer prefab_brushLine;
        private RectTransform currMasterpieceRT;


        // Getters
        public override string MyWorthyNoun { get { return "Praiseworthy"; } }
        private Vector2 GetBrushPos() {
            Vector2 pos = InputController.Instance.MousePosCanvasCentered;
            pos /= rt_room.localScale.x; // scale it correctly.
            pos += new Vector2(0, 10); // HARDCODED offset.
            return pos;
        }
        private bool IsMouseOverCanvasArea() {
            return currMasterpieceRT.rect.Contains(GetBrushPos());
        }



        // ----------------------------------------------------------------
        //  Begin
        // ----------------------------------------------------------------
        override public void Open() {
            base.Open();
            isBrushDown = false;
            mayPaint = false;
            SetCurrMasterpieceIndex(0);
            go_ui.SetActive(false);
        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetCurrMasterpieceIndex(int index) {
            currMasterpieceIndex = index;
            currMasterpieceRT = rt_masterpieces[currMasterpieceIndex];
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

        public void PauseAnimatior() {
            myAnimator.enabled = false;
        }
        private void UnpauseAnimator() {
            myAnimator.enabled = true;
        }



        // ----------------------------------------------------------------
        //  Events
        // ----------------------------------------------------------------
        override public void OnClick_Next() {
            //HideNextButton();
            gameController.AdvanceSeqStep();
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
            // Reset some values, and play the confetti burst.
            brushLines = new List<ImageLines>(); // reset the brushLines list once a painting's been locked in.
            go_ui.SetActive(false);
            mayPaint = false;
            minigameCont.confettiBursts.PlayBurst("+0%", MyWorthyNoun.ToUpper());
            yield return new WaitForSeconds(4f);

            //StepForward();
            gameController.StoryCont.AdvanceStory();
        }



        // ----------------------------------------------------------------
        //  Steps
        // ----------------------------------------------------------------
        override protected void SetCurrStep(int _currStep) {
            base.SetCurrStep(_currStep);
            switch (CurrStep) {
                // Zoom out to show the room.
                case 1:
                    UnpauseAnimator();
                    break;
                // Zoom in and allow painting #1!
                case 2:
                    mayPaint = true;
                    go_ui.SetActive(true);
                    UnpauseAnimator();
                    break;
                // Zoom out from Painting #1.
                case 3:
                    UnpauseAnimator();
                    break;
                // Zoom in and allow painting #2!
                case 4:
                    SetCurrMasterpieceIndex(currMasterpieceIndex + 1);
                    mayPaint = true;
                    go_ui.SetActive(true);
                    UnpauseAnimator();
                    break;
                // Zoom out from Painting #2.
                case 5:
                    UnpauseAnimator();
                    break;
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
                if (Vector2.Distance(brushPosPrev, brushPos) > BrushSegmentMinLength) {
                    ContinueBrushStroke(GetBrushPos());
                    brushPosPrev = GetBrushPos();
                }
            }
        }



    }
}







//// Animate it up to the top area.
//Vector2 pos = rt_masterpieceThumbnailPoses[currMasterpieceIndex].position;
//LeanTween.move(currMasterpieceRT.gameObject, pos, 1f).setEaseInOutQuad();
//LeanTween.scale(currMasterpieceRT.gameObject, Vector3.one * 0.1f, 1f).setEaseInOutQuad();

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