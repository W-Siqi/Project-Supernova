using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public bool isRandom = false;
    public Trait requiredTrait;

    // 动画(c0) ,c代表character, 0 代表bindinginfo的下标
    // 动画(h[charaIndex][trait-转ASCII]),h是hightlight
    // 动画(r[charaIndex][trait-转ASCII]),r是remove trait
    // 动画(a[charaIndex][trait-转ASCII]),a是add trait
    public string CreateDescription(GameState gameState, int bindedCharacterIndex) {
        var characterShowupStr = string.Format("(c{0})", bindedCharacterIndex);
        var traitAnimationStr = "";
        if (!isRandom) { 
            traitAnimationStr = string.Format("(h{0}{1})", bindedCharacterIndex, (char)requiredTrait);
        }
       
        var finalDescription = string.Format("{0}{1}{2}", characterShowupStr,traitAnimationStr, gameState.characterDeck[bindedCharacterIndex].name);
        return finalDescription;
    }
}
