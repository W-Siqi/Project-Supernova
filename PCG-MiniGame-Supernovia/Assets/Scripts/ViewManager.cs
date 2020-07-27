using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager _instance = null;
    public static ViewManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ViewManager>();
            }
            return _instance;
        }
    }

    public ValueViewer armyValue;
    public ValueViewer financeValue;
    public ValueViewer luckValue;
}
