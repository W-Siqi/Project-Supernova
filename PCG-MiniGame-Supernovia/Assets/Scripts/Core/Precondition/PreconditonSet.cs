using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// TBD: evironment 和 event先废弃
[System.Serializable]
public class PreconditonSet
{
    public bool environmentEnabled = false;
    public bool characterEnabled = false;
    public bool eventEnabled = false;
    public List<CharacterPrecondition> characterPreconditions = new List<CharacterPrecondition>();
    public EnvironmentPrecondition environmentPrecondition = new EnvironmentPrecondition();
    public List<EventPrecondition> eventPreconditions = new List<EventPrecondition>();

    // TBD:
    // 多个precondition 可能会bind到同一个对象
    public BindingInfo[] Bind(GameState gameState) {
        var bindingInfos = new List<BindingInfo>();
        foreach (var preconditon in characterPreconditions) {
            bindingInfos.Add(preconditon.Bind(gameState));
        }
        return bindingInfos.ToArray();
    }

    /// <summary>
    /// TBD: 角色的判定会有问题，不能单独判
    /// </summary>
    /// <returns></returns>
    // TBD: evironment 和 event先废弃
    public bool SatisfiedAt(GameState givenState) {
        var charaSatisfied = true;
        foreach (var cha in characterPreconditions) {
            if (!cha.SatisfiedAt(givenState)) {
                charaSatisfied = false;
                break;
            }
        }

        return charaSatisfied;
    }
}
