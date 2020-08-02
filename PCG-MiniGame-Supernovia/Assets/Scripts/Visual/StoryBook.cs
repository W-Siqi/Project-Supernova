using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBook : MonoBehaviour
{
    private const string DISSOLVE_SHADER_PROPERTY = "_SliceAmount";
    [System.Serializable]
    private class PageConentShowPos {
        public AnchorPoint camAnchor;
        public RawImage contentPageImage;
        public Text textContent;
        [HideInInspector]
        public Material conetentPageMat = null;

        public void Init() {
            conetentPageMat = new Material(contentPageImage.material);
            contentPageImage.material = conetentPageMat;
        }

        public void ClearContent() {
            contentPageImage.enabled = false;
            textContent.text = "";
        }
    }

    [System.Serializable]
    public class PageContent {
        public string text = "";
        public Texture image = null;

        public PageContent(Texture image) {
            this.image = image;
        }

        public PageContent(string text) {
            this.text = text;
        }

        public PageContent(Texture image, string text) {
            this.image = image;
            this.text = text;
        }
    }

    private static StoryBook _instance = null;
    public static StoryBook instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<StoryBook>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private List<PageConentShowPos> showPoses = new List<PageConentShowPos>();
    private int curPosIndex = 0; 

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float turnPageDelay;
    [SerializeField]
    BookControllerWrapper bookControllerWrapper;

    private void Awake() {
        foreach (var showPos in showPoses) {
            showPos.Init();
        }
    }

    public IEnumerator ViewContent(PageContent pageContent) {
        if (curPosIndex >= showPoses.Count) {
            // clear 
            foreach (var showPos in showPoses) {
                showPos.ClearContent();
            }
            // turn
            yield return StartCoroutine(TurnPage());
            curPosIndex = 0;
        }
        else {
            // move to ...
            LerpAnimator.instance.LerpPosition(mainCamera.transform,showPoses[curPosIndex].camAnchor.position,1f);
        }

        ShowContentOn(pageContent, showPoses[curPosIndex++]);
        yield return new WaitForSeconds(4f);
    }

    public IEnumerator TurnPage() {
        // 书本镜头动画
        var turnPageTime = 1.5f;
        var farPos = AnchorManager.instance.storyBookFar.transform.position;
        var targetPos = showPoses[0].camAnchor.position;
        LerpAnimator.instance.LerpPosition(mainCamera.transform, farPos, turnPageTime/2,
            () => {
                LerpAnimator.instance.LerpPosition(mainCamera.transform, targetPos, turnPageTime/2);
            });

        yield return new WaitForSeconds(turnPageDelay);
        // 翻页
        bookControllerWrapper.TurnNextPage();
    }

    private void ShowContentOn(PageContent pageContent,PageConentShowPos conentShowPos) {
        if (pageContent.image != null) {
            conentShowPos.contentPageImage.enabled= true;
            StartCoroutine(UpdatePageDelayed(conentShowPos, pageContent.image));
            var dissolveDuration = 3f;
            LerpAnimator.instance.LerpValues(1, 0, dissolveDuration,
                (v) => {
                    conentShowPos.conetentPageMat.SetFloat(DISSOLVE_SHADER_PROPERTY, v);
                });
        }
        else {
            conentShowPos.contentPageImage.enabled = false;
        }

        conentShowPos.textContent.text = pageContent.text;
    }



    IEnumerator UpdatePageDelayed(PageConentShowPos showPos, Texture pageContent) {
        yield return new WaitForEndOfFrame();
        showPos.contentPageImage.texture = pageContent;
    }
}
