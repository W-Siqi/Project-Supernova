using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CharacterCard : Card
{
    [System.Serializable]
    public class Attributes {
        public float competent = 0;
        public float loyal = 0;
        public float shrewdness = 0;
    }

    public Attributes attributes = new Attributes();
    public Deck<StratagemCard> deck = new Deck<StratagemCard>();
    public List<Qualifier> qualifiers;
}
