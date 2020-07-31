using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBook : MonoBehaviour
{
    public class PageContent {
        public Texture image = null;
        public string text = "";

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
    private Camera mainCamera;
    [SerializeField]
    private RawImage contentPageImage;
    [SerializeField]
    private Text textContent;
    [SerializeField]
    private float turnPageDelay;
    [SerializeField]
    private AnimationCurve turnPageCurve;
    [SerializeField]
    BookControllerWrapper bookControllerWrapper;
    
    [ContextMenu("测试翻页")]
    private void Test() {
        StartCoroutine(TurnPage(new PageContent("测试")));
    }

    public IEnumerator TurnPage(PageContent pageContent) {
        // 书本镜头动画
        var turnPageTime = 1.5f;
        var closePos = AnchorManager.instance.storyBookClose.transform.position;
        var farPos = AnchorManager.instance.storyBookFar.transform.position;
        LerpAnimator.instance.LerpValues(turnPageCurve, turnPageTime,
            (float val) => { mainCamera.transform.position = Vector3.Lerp(closePos, farPos, val); });

        yield return new WaitForSeconds(turnPageDelay);
        // 翻页
        bookControllerWrapper.TurnNextPage();
        // 换内容
        ChangePageContent(pageContent);

        yield return new WaitForSeconds(turnPageTime);
    }

    private void ChangePageContent(PageContent pageContent) {
        if (pageContent.image != null) {
            contentPageImage.enabled = true;
            StartCoroutine(UpdatePageDelayed(pageContent.image));
        }
        else {
            contentPageImage.enabled = false;
        }

        textContent.text = pageContent.text;
    }

    IEnumerator UpdatePageDelayed(Texture pageContent) {
        yield return new WaitForEndOfFrame();
        contentPageImage.texture = pageContent;
    }
}
