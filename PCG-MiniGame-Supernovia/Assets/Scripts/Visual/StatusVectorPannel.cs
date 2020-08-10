using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusVectorPannel : MonoBehaviour
{
    public ValueViewer armyValue;
    public ValueViewer moneyValue;
    public ValueViewer peopleValue;

    // 会改变的值标出来
    public void ActivateRelatedValues(StatusVector delta) {
        if (delta.money != 0) {
            moneyValue.Activate();
        }
        if (delta.army != 0) {
            armyValue.Activate();
        }
        if (delta.people != 0) {
            peopleValue.Activate();
        }
    }

    public void ForceSync(StatusVector statusVector) {
        armyValue.ForceSync(statusVector.army);
        moneyValue.ForceSync(statusVector.money);
        peopleValue.ForceSync(statusVector.people);
    }

    public void DisactivateAllValues() {
        armyValue.Disactivate();
        moneyValue.Disactivate();
        peopleValue.Disactivate();
    }

    public IEnumerator ViewStatusVectorChange(StatusVector delta) {
        float interval = 0.5f;
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
        float interval = 0.5f;
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
