using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBook : MonoBehaviour
{
    private static StoryBook _instance = null;
    public static StoryBook instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<StoryBook>();
            }
            return _instance;
        }
    }

    public RenderTexture councilPage;
    public RenderTexture eventPage;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private RawImage contentPageImage;
    [SerializeField]
    private float turnPageDelay;
    [SerializeField]
    private AnimationCurve turnPageCurve;
    [SerializeField]
    BookControllerWrapper bookControllerWrapper;
    
    [ContextMenu("测试翻页")]
    private void Test() {
        StartCoroutine(TurnPage(councilPage));
    }

    public IEnumerator TurnPage(RenderTexture pageContent) {
        // 书本镜头动画
        var turnPageTime = 1.5f;
        var closePos = AnchorManager.instance.storyBookClose.transform.position;
        var farPos = AnchorManager.instance.storyBookFar.transform.position;
        LerpAnimator.instance.LerpValues(turnPageCurve, turnPageTime,
            (float val) => { mainCamera.transform.position = Vector3.Lerp(closePos, farPos, val); });

        yield return new WaitForSeconds(turnPageDelay);
        // 翻页
        bookControllerWrapper.TurnNextPage();
        // 翻页不久后替换内容页的texture
        StartCoroutine(UpdatePageDelayed(pageContent));

        yield return new WaitForSeconds(turnPageTime);
    }

    IEnumerator UpdatePageDelayed(RenderTexture pageContent) {
        yield return new WaitForEndOfFrame();
        contentPageImage.texture = pageContent;
    }
}
