using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 负责调用viewManager绘制
// TBD 重构
//public class UIController : MonoBehaviour
//{
//    private void Start() {
//        ViewManager.instance.armyValue.SetMaxValue(100f);
//        ViewManager.instance.moneyValue.SetMaxValue(100f);
//        ViewManager.instance.peopleValue.SetMaxValue(100f);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        var status = PlayData.instance.gameState.statusVector;
//        ViewManager.instance.armyValue.SetCurrentValue(status.army);
//        ViewManager.instance.moneyValue.SetCurrentValue(status.money);
//        ViewManager.instance.peopleValue.SetCurrentValue(status.people);
//    }
//}
