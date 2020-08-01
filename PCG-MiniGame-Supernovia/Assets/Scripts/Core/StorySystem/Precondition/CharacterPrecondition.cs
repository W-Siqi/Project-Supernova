using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public Trait requiredTrait;
    // 如果角色不是当前的状态，那么搜他旁边的，但是需要一个最大距离限制
    public int topologyDistanceAllowed;
    public override bool SatisfiedByCurrentContext() {
           
        foreach (var character in StoryContext.instance.characterDeck) {
            foreach (var personality in character.personalities) {
                // 有1个character的1个personality符合，就返回
                if (personality.TopologyDistanceFromCurrent(requiredTrait) <= topologyDistanceAllowed) {
                    return true;
                }
            }
        }

        return false;
    }
    
    /// <summary>
    /// 绑定失败，则返回一个空对象
    /// </summary>
    /// <returns></returns>
    public BindingInfo Bind() {
        return new BindingInfo();
    }
}
