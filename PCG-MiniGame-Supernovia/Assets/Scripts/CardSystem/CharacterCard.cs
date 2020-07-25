using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CharacterCard : Card
{
    [System.Serializable]
    public class Attributes {
        public int atkVal = 0;
        public int maxHP = 0;
        public int HP = 0;
    }

    public Attributes attributes = new Attributes();
    public List<Qualifier> qualifiers;
}
