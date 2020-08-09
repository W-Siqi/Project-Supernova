using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusVectorPannel : MonoBehaviour
{
    public ValueViewer armyValue;
    public ValueViewer moneyValue;
    public ValueViewer peopleValue;

    public IEnumerator ViewStatusVectorChange(StatusVector delta) {
        float interval = 1f;
        if (delta.money != 0) {
            moneyValue.ApplyDiff(delta.money);
        }
        yield return new WaitForSeconds(interval);
        if (delta.army != 0) {
            armyValue.ApplyDiff(delta.army);
        }
        yield return new WaitForSeconds(interval);
        if (delta.people != 0) {
            peopleValue.ApplyDiff(delta.people);
        }
    }

    /// <summary>
    /// 因为trait为改变，会有不同的视觉特效
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="trait"></param>
    public IEnumerator ViewStatusVectorChange(StatusVector delta,Trait trait) {
        float interval = 1f;
        float waitTime = 1f;
        if (delta.money != 0) {
            moneyValue.ApplyDiff(delta.money, trait);
        }
        yield return new WaitForSeconds(interval);
        if (delta.army != 0) {
            armyValue.ApplyDiff(delta.army, trait);
        }
        yield return new WaitForSeconds(interval);
        if (delta.people != 0) {
            peopleValue.ApplyDiff(delta.people, trait);
        }
        yield return new WaitForSeconds(waitTime);
    }
}
