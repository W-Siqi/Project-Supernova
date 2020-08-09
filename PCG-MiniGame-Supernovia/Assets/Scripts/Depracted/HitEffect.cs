using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField]
    TextMeshPro meshPro;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;
    [SerializeField]
    AnimationCurve sizeCurve;
    [SerializeField]
    AnimationCurve colorCurve;
    public static void Create(int hitValue,Vector3 postion) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.hitEffect,postion,Quaternion.identity);
        GO.GetComponent<HitEffect>().PlayHit(hitValue);
    }

    private void PlayHit(int hitValue) {
        float sustainTime = 2f;
        meshPro.text = "-" + hitValue.ToString();

        //color
        LerpAnimator.instance.LerpValues(
            colorCurve,
            sustainTime,
            (float val) => { meshPro.color = Color.Lerp(startColor, endColor, val); });

        // size
        var normalSize = meshPro.transform.localScale;
        LerpAnimator.instance.LerpValues(
            sizeCurve,
            sustainTime,
            (float val) => { meshPro.transform.localScale = Vector3.Lerp(Vector3.zero, normalSize,val);});

        Destroy(gameObject, sustainTime + 1f);
    }
}
