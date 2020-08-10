using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

/// <summary>
/// TBD: 应该叫eventSconsequecneSet
/// </summary>
[System.Serializable]
public class ConsequenceSet
{
    public bool statusConsequenceEnabled = false;
    public bool characterConsequenceEnabled = false;
    public bool keywordConsequenceEnabled = false;
    public StatusConsequence statusConsequence = new StatusConsequence();
    public List<CharacterConsequence> characterConsequences = new List<CharacterConsequence>();
    public KeywordConsequence keywordConsequence = new KeywordConsequence();

    //public void Apply(BindingInfo[] bindingInfos,GameState gameStateToApply) {
    //    if (statusConsequenceEnabled) {
    //        gameStateToApply.statusVector += statusConsequence.delta;
    //        ViewManager.instance.statusVectorPannel.ViewStatusVectorChange(statusConsequence.delta);
    //    }
    //}
}
