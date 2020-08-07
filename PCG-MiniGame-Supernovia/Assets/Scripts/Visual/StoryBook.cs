using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBook : MonoBehaviour
{
    private const string DISSOLVE_SHADER_PROPERTY = "_SliceAmount";
    [System.Serializable]
    public class PageConentShowPos {
        public AnchorPoint camAnchor;
        public RawImage contentPageImage;
        public TextMeshProUGUI textContent;
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

    public PageConentShowPos ViewContent(PageContent pageContent) {
        if (curPosIndex >= showPoses.Count) {
            // clear 
            foreach (var p in showPoses) {
                p.ClearContent();
            }
            // turn
            StartCoroutine(TurnPage());
            curPosIndex = 0;
        }
        else {
            // move to ...
            LerpAnimator.instance.LerpPosition(mainCamera.transform,showPoses[curPosIndex].camAnchor.position,0.3f);
        }

        var showPos = showPoses[curPosIndex++];
        StartCoroutine(ShowContentOn(pageContent, showPos));
        return showPos;
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

    private IEnumerator ShowContentOn(PageContent pageContent,PageConentShowPos conentShowPos) {
        conentShowPos.textContent.text = pageContent.text;

        if (pageContent.image != null) {
            if (pageContent.text != null) {
                yield return new WaitForSeconds(1f);
            }
            conentShowPos.contentPageImage.enabled= true;
            StartCoroutine(UpdatePageDelayed(conentShowPos, pageContent.image));
            var dissolveDuration = 1f;
            LerpAnimator.instance.LerpValues(1, 0, dissolveDuration,
                (v) => {
                    conentShowPos.conetentPageMat.SetFloat(DISSOLVE_SHADER_PROPERTY, v);
                });
        }
        else {
            conentShowPos.contentPageImage.enabled = false;
        }

        yield return null;
    }

    IEnumerator UpdatePageDelayed(PageConentShowPos showPos, Texture pageContent) {
        yield return new WaitForEndOfFrame();
        showPos.contentPageImage.texture = pageContent;
    }
}
