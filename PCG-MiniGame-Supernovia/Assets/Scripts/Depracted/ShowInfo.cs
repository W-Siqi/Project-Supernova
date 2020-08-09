using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class ShowInfo
{
    public enum Type { 
        none,die,add,showAndBack
    }

    public CharacterCard target = new CharacterCard();
    public Type type = Type.none;
}
