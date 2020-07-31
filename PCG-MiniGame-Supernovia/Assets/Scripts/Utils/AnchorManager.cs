using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorManager : MonoBehaviour
{
    private static AnchorManager _instance = null;
    public static AnchorManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<AnchorManager>();
            }
            return _instance;
        }
    }


    public AnchorPoint stratagemCardAnchor;
    public AnchorPoint characterCardAnchor;
    public AnchorPoint eventCardAnchor;
    public AnchorPoint deckSpawnAnchor;

    // 用于事件演出
    public AnchorPoint showCardLeftAnchor;
    public AnchorPoint showCardMiddleAnchor;
    public AnchorPoint showCardRightAnchor;

    // story book 相关
    public AnchorPoint storyBookClose;
    public AnchorPoint storyBookFar;

    // fight 相关
    public AnchorPoint fightLeftBottom;
    public AnchorPoint fightLeftUp;
    public AnchorPoint fightLeftSpawn;
    public AnchorPoint fightRightBottom;
    public AnchorPoint fightRightUp;
    public AnchorPoint fightRightSpawn;

    public string nameToAdd = "new anchor point";

    private List<AnchorPoint> hiddens = new List<AnchorPoint>();

    [ContextMenu("Add")]
    private void AddAnchor() {
        var GO = Instantiate(ResourceTable.instance.prefabPage.anchorPoint,transform.position,transform.rotation);
        var anchorPoint = GO.GetComponent<AnchorPoint>();
        anchorPoint.name = nameToAdd;
        anchorPoint.transform.parent = transform;
    }



    [ContextMenu("Hide ALL")]
    private void HideALLAcnhors() {
        foreach (var g in GameObject.FindGameObjectsWithTag(AnchorPoint.ANCHOR_TAG)) {
            g.SetActive(false);
        }
    }

    [ContextMenu("Show ALL")]
    private void ShowALLAcnhors() {
        foreach (var g in GameObject.FindGameObjectsWithTag(AnchorPoint.ANCHOR_TAG)) {
            g.SetActive(true);
        }
    }
}
