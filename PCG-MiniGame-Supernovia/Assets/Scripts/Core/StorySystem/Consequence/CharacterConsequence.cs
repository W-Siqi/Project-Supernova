using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterConsequence : Consequence{
    // TBD：（暂时的做法）
    //  0代表随机角色对象(废弃，不应该0，如果没有角色做前提，那么这个后果就不应该存在！)
    //  x (1 <= x < n) ， x-1 == preconditon里面人物变量对应的下标
    public int bindFlag = 0;
    public List<QualiferAlteration> qualiferAlterations = new List<QualiferAlteration>();

    public CharacterCard GetBindedCharacter(CharacterCard[] bindedCharacters) {
        return bindedCharacters[bindFlag - 1];
    }
}
