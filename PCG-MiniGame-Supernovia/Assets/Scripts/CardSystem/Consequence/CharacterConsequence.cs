using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterConsequence : Consequence{
    // TBD：（暂时的做法）
    //  0代表随机角色对象
    //  x (1 <= x < n) ， x-1 == preconditon里面人物变量对应的下标
    public int bindFlag = 0;
    public List<QualiferAlteration> qualiferAlterations = new List<QualiferAlteration>();

    public override void Apply() {
        throw new System.NotImplementedException();
    }
}
