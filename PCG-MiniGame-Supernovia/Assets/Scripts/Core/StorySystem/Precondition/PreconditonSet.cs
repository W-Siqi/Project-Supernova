using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

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
    public BindingInfo[] Bind() {
        var bindingInfos = new List<BindingInfo>();
        foreach (var preconditon in characterPreconditions) {
            bindingInfos.Add(preconditon.Bind());
        }
        return bindingInfos.ToArray();
    }

    /// <summary>
    /// TBD: 角色的判定会有问题，不能单独判
    /// </summary>
    /// <returns></returns>
    public bool SatisfiedByCurrentContext() {
        var charaSatisfied = true;
        var envSatisfied = true;
        var eventSatusfied = true;

        if (!environmentPrecondition.SatisfiedByCurrentContext()) {
            envSatisfied = true;
        }

        foreach (var cha in characterPreconditions ) {
            if (!cha.SatisfiedByCurrentContext()) {
                charaSatisfied = false;
                break;
            }
        }

        foreach (var eventPreconditon in eventPreconditions) {
            if (!eventPreconditon.SatisfiedByCurrentContext()) {
                envSatisfied = false;
                break;
            }
        }

        return charaSatisfied && envSatisfied && eventSatusfied;
    }
}
