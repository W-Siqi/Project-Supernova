using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDisplayBehaviour : MonoBehaviour
{
    [SerializeField]
    private RuntimeMaterial frontMaterial;
    public static CardDisplayBehaviour Create(Card card, Vector3 positon, Quaternion rotation) {
        return Create(card.name, card.GetAvatarImage(), positon, rotation);
    }

    public static CardDisplayBehaviour CreateDummyCard(Vector3 positon, Quaternion rotation) {
        return Create("dummy", null, positon, rotation);
    }

    public void SetFrontImage(Texture2D image) {
        frontMaterial.runtimeMat.mainTexture = image;
    }

    private static CardDisplayBehaviour Create(string name, Texture2D image, Vector3 positon, Quaternion rotation) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.cardDisplay,positon,rotation);
        GO.name = name;
        var displayBehvaiour = GO.GetComponent<CardDisplayBehaviour>();
        displayBehvaiour.Init();
        if (image) {
            displayBehvaiour.SetFrontImage(image);
        }
        return displayBehvaiour;
    }

    private void Init() {
        frontMaterial.Init();
    }
}
