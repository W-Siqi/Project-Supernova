using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimator : MonoBehaviour
{
    private static LerpAnimator _instance = null;

    public static LerpAnimator instance {
        get {
            if (_instance == null) {
                var GO = new GameObject("LerpAnimator");
                _instance = GO.AddComponent<LerpAnimator>();
            }
            return _instance;
        }
    }

    public void LerpPosition(GameObject target, Vector3 from, Vector3 to, float time) { 
    
    }
}
