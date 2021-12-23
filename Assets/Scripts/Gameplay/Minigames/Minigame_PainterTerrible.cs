using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinigameNamespace {
    public class Minigame_PainterTerrible : Minigame {
        // Components
        [SerializeField] private TextMeshProUGUI t_header;
        [SerializeField] private Button b_undo;
        [SerializeField] private Button b_submitPainting;
        [SerializeField] private RectTransform[] rt_paintings; // includes the frame and canvas cloth.
        [SerializeField] private RectTransform[] rt_paintingThumbnailPoses; // where we tween them each to.
        [SerializeField] private ConfettiBursts myConfetti;
        private ImageLines currBrushLine;
        private List<ImageLines> brushLines = new List<ImageLines>();
        private List<Image> pooStamps=new List<Image>();
        // Properties
        private bool isBrushDown;
        private bool mayPaint;
        private Vector2 brushPosPrev;
        private int currPaintingIndex;
        private const int NumPaintingsRequired = 3;
        private Color brushColor = new Color255(109, 61, 18, 200).ToColor();
        private float brushThickness = 7;
        // References
        //[SerializeField] private LineRenderer prefab_brushLine;
        [SerializeField] private Sprite sprite_pooEmoji;
        private RectTransform currPaintingRT;


        // Getters
        public override string MyWorthyNoun { get { return "Praiseworthy"; } }
        private Vector2 GetBrushPos() {
            return InputController.Instance.MousePosCanvasCentered + new Vector2(0, 60); // HARDCODED offset.
        }
        private bool IsMouseOverCanvasArea() {
            return currPaintingRT.rect.Contains(GetBrushPos());
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
            SetCurrPaintingIndex(0);
        }


        // ----------------------------------------------------------------
        //  Doers
        // ----------------------------------------------------------------
        private void SetCurrPaintingIndex(int index) {
            currPaintingIndex = index;
            currPaintingRT = rt_paintings[currPaintingIndex];
            int numLeft = NumPaintingsRequired - currPaintingIndex;
            t_header.text = "Paint " + numLeft + " disasterpiece" + (numLeft==1?"":"s") + ".";
            UpdateButtonsInteractable();
        }

        private void StartNewBrushStroke() {
            // BrushLine
            ImageLines imgLine = Instantiate(ResourcesHandler.Instance.ImageLines).GetComponent<ImageLines>();
            GameUtils.ParentAndReset(imgLine.gameObject, currPaintingRT);
            imgLine.name = "BrushStroke";
            imgLine.SetColor(brushColor);
            imgLine.SetThickness(brushThickness);
            currBrushLine = imgLine;
            brushLines.Add(imgLine);
            UpdateButtonsInteractable();
            // Stamp
            Image stamp = new GameObject().AddComponent<Image>();
            stamp.sprite = sprite_pooEmoji;
            stamp.name = "PooStamp";
            GameUtils.ParentAndReset(stamp.gameObject, currPaintingRT);
            stamp.rectTransform.localEulerAngles = new Vector3(0,0, Random.Range(-90,90));
            stamp.rectTransform.anchoredPosition = GetBrushPos();
            stamp.rectTransform.localScale = Vector3.one * 0.5f;
            pooStamps.Add(stamp);
        }
        private void ContinueBrushStroke(Vector2 pos) {
            currBrushLine.AddPoint(pos);
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
            gameController.AdvanceSeqStep();
        }
        public void OnClick_Undo() {
            if (brushLines.Count > 0) {
                // Remove brush line.
                ImageLines brushLine = brushLines[brushLines.Count-1];
                brushLines.RemoveAt(brushLines.Count - 1);
                Destroy(brushLine.gameObject);
                // Remove stamp.e
                Image stamp = pooStamps[pooStamps.Count-1];
                pooStamps.RemoveAt(pooStamps.Count - 1);
                Destroy(stamp.gameObject);
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
            //minigameCont.confettiBursts.PlayBurst("-0%", MyWorthyNoun.ToUpper());
            myConfetti.PlayBurst("-0%", MyWorthyNoun.ToUpper());
            mayPaint = false;
            UpdateButtonsInteractable();
            yield return new WaitForSeconds(2f);

            mayPaint = true;

            // Animate it up to the top area.
            Vector2 pos = rt_paintingThumbnailPoses[currPaintingIndex].position;
            LeanTween.move(currPaintingRT.gameObject, pos, 1f).setEaseInOutQuad();
            LeanTween.scale(currPaintingRT.gameObject, Vector3.one * 0.1f, 1f).setEaseInOutQuad();

            // Allow starting the next painting!
            if (currPaintingIndex < NumPaintingsRequired-1) {
                SetCurrPaintingIndex(currPaintingIndex + 1);
            }
            else {
                t_header.text = "All disasterpieces completed.";
                b_undo.gameObject.SetActive(false);
                b_submitPainting.gameObject.SetActive(false);
                ShowNextButton();
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