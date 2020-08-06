using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class EnvironmentPrecondition : Precondition{
    public List<Qualifier> qualifiers = new List<Qualifier>();

    public override bool SatisfiedAt(GameState givenState) {
        throw new System.NotImplementedException();
    }
}
