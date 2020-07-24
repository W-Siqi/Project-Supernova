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

    public string nameToAdd = "new anchor point";

    [ContextMenu("Add")]
    private void AddAnchor() {
        var GO = Instantiate(ResourceTable.instance.prefabPage.anchorPoint,transform.position,transform.rotation);
        var anchorPoint = GO.GetComponent<AnchorPoint>();
        anchorPoint.name = nameToAdd;
        anchorPoint.transform.parent = transform;
    }

    [ContextMenu("Hide ALL")]
    private void HideALLAcnhors() {
        foreach (var p in FindObjectsOfType<AnchorPoint>()) {
            p.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Show ALL")]
    private void ShowALLAcnhors() {
        foreach (var p in FindObjectsOfType<AnchorPoint>()) {
            p.gameObject.SetActive(true);
        }
    }
}
