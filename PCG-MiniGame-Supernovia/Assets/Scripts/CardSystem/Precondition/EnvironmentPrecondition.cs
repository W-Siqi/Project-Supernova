using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentPrecondition : Precondition{
    public List<Qualifier> qualifiers = new List<Qualifier>();
}
