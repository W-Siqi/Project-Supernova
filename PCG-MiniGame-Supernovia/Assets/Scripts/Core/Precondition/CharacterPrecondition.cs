using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public Trait requiredTrait;
    public override bool SatisfiedByCurrentContext() {
           
        foreach (var character in StoryContext.instance.characterDeck) {
            foreach (var personality in character.personalities) {
                // 有1个character的1个personality符合，就返回
                if (personality.trait == requiredTrait) {
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
        var bindingInfo= new BindingInfo();
        bindingInfo.bindedCharacter = StoryContext.instance.characterDeck[Random.Range(0, StoryContext.instance.characterDeck.Count)];
        bindingInfo.bindedPersonalityOfCharacter = bindingInfo.bindedCharacter.personalities[0];
        return bindingInfo;
    }
}
