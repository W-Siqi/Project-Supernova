using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 专门用来显示牌堆
public class DeckDisplayBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 cardOffset;
    private Vector3 deckTopLocalPos = Vector3.zero;
    private Stack<CardDisplayBehaviour> dummyCards = new Stack<CardDisplayBehaviour>();

    public Vector3 deckTopPos { get { return deckTopLocalPos + transform.position; } }

    public void AddCard() {
        var dummyCard = CardDisplayBehaviour.CreateDummyCard(transform.position + deckTopLocalPos, transform.rotation);
        dummyCard.transform.SetParent(transform);
        dummyCards.Push(dummyCard);
        deckTopLocalPos += cardOffset;
    }

    public void DrawCard(out Vector3 postion, out Quaternion rotation) {
        postion = transform.position + deckTopLocalPos;
        rotation = transform.rotation;

        if (dummyCards.Count > 0) {
            deckTopLocalPos -= cardOffset;
            var topDummyCard = dummyCards.Pop();
            DestroyImmediate(topDummyCard.gameObject);
        }
    }
}
