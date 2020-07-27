using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentPrecondition : Precondition{
    public List<Qualifier> qualifiers = new List<Qualifier>();

    public override bool SatisfiedByCurrentContext() {
        foreach (var qualifier in qualifiers) {
            if (!StoryContext.instance.environmentQualifiers.Exists((i) => i.name == qualifier.name)) {
                return false;
            }
        }
        return true;
    }
}
