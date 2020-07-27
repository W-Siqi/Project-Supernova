using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsequenceSet
{
    public bool statusConsequenceEnabled = false;
    public bool environmentConsequenceEnabled = false;
    public bool characterConsequenceEnabled = false;
    public bool fightConsequenceEnabled = false;
    public StatusConsequence statusConsequence = new StatusConsequence();
    public List<CharacterConsequence> characterConsequences = new List<CharacterConsequence>();
    public EnvironmentConsequence environmentConsequence = new EnvironmentConsequence();
    public FightConsequence fightConsequence = new FightConsequence();
}
