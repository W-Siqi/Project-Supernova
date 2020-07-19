using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck<T>
{
    [SerializeField]
    private List<T> cards;
    public T[] GetAll() {
        return cards.ToArray();
    }
}
