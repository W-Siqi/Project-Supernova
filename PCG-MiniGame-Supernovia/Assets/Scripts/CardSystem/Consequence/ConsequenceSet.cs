using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsequenceSet
{
    public List<CharacterConsequence> characterConsequences = new List<CharacterConsequence>();
    public EnvironmentConsequence environmentConsequence = new EnvironmentConsequence();
}
