using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public bool isRandom = false;
    public Trait requiredTrait;
    public override bool SatisfiedAt(GameState givenState){
           
        foreach (var character in givenState.characterDeck) {
            if (isRandom) {
                return true;
            }

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
    public BindingInfo Bind(GameState gameState) {
        var bindingInfo= new BindingInfo();
        bindingInfo.bindedCharacter = gameState.characterDeck[Random.Range(0, gameState.characterDeck.Count)];
        bindingInfo.bindedPersonalityOfCharacter = bindingInfo.bindedCharacter.personalities[0];
        return bindingInfo;
    }

    public string CreateDescription(BindingInfo bindingInfo) {
        return bindingInfo.bindedCharacter.name;
    }
}
