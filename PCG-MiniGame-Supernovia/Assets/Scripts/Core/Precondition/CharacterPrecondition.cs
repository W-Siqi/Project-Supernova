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

    // 动画(c0) ,c代表character, 0 代表bindinginfo的下标
    // 动画(h[bindinfoIndex][trait-转ASCII]),h是hightlight
    // 动画(r[bindinfoIndex][trait-转ASCII]),r是remove trait
    // 动画(a[bindinfoIndex][trait-转ASCII]),a是add trait
    public string CreateDescription(BindingInfo bindingInfo, int indexInBindingSequence) {
        var characterShowupStr = string.Format("(c{0})",indexInBindingSequence.ToString());
        var traitAnimationStr = "";
        if (!isRandom) { 
            traitAnimationStr = string.Format("(h{0}{1})", indexInBindingSequence.ToString(), (char)requiredTrait);
        }
       
        var finalDescription = string.Format("{0}{1}{2}", characterShowupStr,traitAnimationStr, bindingInfo.bindedCharacter.name);
        return finalDescription;
    }
}
