using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 负责调用viewManager绘制
public class UIController : MonoBehaviour
{
    private void Start() {
        ViewManager.instance.armyValue.SetMaxValue(100f);
        ViewManager.instance.financeValue.SetMaxValue(100f);
        ViewManager.instance.luckValue.SetMaxValue(100f);
    }

    // Update is called once per frame
    void Update()
    {
        var status = StoryContext.instance.statusVector;
        ViewManager.instance.armyValue.SetCurrentValue(status.army);
        ViewManager.instance.financeValue.SetCurrentValue(status.finance);
        ViewManager.instance.luckValue.SetCurrentValue(status.luck);
    }
}
