using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public List<Qualifier> qualifiers = new List<Qualifier>();
}
