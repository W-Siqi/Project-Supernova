using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGVariableTable : MonoBehaviour {
    public static PCGVariableTable _instance = null;

    public float wiseTraitAmplifyRate = 0.3f;
    public float slicentTraitSlicenceProbility = 0.3f;
    public int corrputTraitMoneyPerRound = 2;
    public int cruelTraitPeopleValuePerDecision = 5;


    public static PCGVariableTable instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PCGVariableTable>();
                if (_instance == null) {
                    var GO = new GameObject("PCG Table");
                    _instance = GO.AddComponent<PCGVariableTable>();
                }
            }
            return _instance;
        }
    }
}
