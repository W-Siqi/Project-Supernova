﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterConsequence : Consequence{
    // TBD：（暂时的做法）
    // bindFlag就等于Precondition的下标
    public int bindFlag = 0;

    public TraitAlteration traitAlteration = new TraitAlteration();
    public int loyaltyAlteraion = 0;

    public string CreateDescription(BindingInfo bindingInfo) {
        return bindingInfo.bindedCharacter.name + " 发生了改变";
    }
}
