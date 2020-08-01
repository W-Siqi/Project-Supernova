using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 负责调用viewManager绘制
public class UIController : MonoBehaviour
{
    private void Start() {
        ViewManager.instance.foodValue.SetMaxValue(100f);
        ViewManager.instance.moneyValue.SetMaxValue(100f);
        ViewManager.instance.authorityValue.SetMaxValue(100f);
    }

    // Update is called once per frame
    void Update()
    {
        var status = StoryContext.instance.statusVector;
        ViewManager.instance.foodValue.SetCurrentValue(status.food);
        ViewManager.instance.moneyValue.SetCurrentValue(status.money);
        ViewManager.instance.authorityValue.SetCurrentValue(status.authority);
    }
}
