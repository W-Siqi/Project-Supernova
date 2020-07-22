using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentConsequence : Consequence
{
    public List<QualiferAlteration> qualiferAlterations = new List<QualiferAlteration>();
    public override void Apply() {
        throw new System.NotImplementedException();
    }
}
