using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PCG;

public class CardDisplayBehaviour : MonoBehaviour
{
    [SerializeField]
    private RuntimeMaterial frontMaterial;
    [SerializeField]
    private TextMeshPro name;
    [SerializeField]
    private TextMeshPro description;
    [SerializeField]
    private TextMeshPro atkVal;
    [SerializeField]
    private TextMeshPro hpVal;


    public static CardDisplayBehaviour CreateDummyCard(Vector3 positon, Quaternion rotation) {
        return Create(null,positon, rotation);
    }

    public static CardDisplayBehaviour CreateAnonymousCard(Vector3 positon, Quaternion rotation) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.cardDisplay, positon, rotation);

        var displayBehvaiour = GO.GetComponent<CardDisplayBehaviour>();
        displayBehvaiour.gameObject.name = "AnonymousCard";

        // image
        displayBehvaiour.frontMaterial.Init();
        displayBehvaiour.frontMaterial.runtimeMat.mainTexture = ResourceTable.instance.texturepage.aynominousCharacter;

        // name
        displayBehvaiour.name.text = "";
        // description
        displayBehvaiour.description.text = "";

        return displayBehvaiour;
    }

    public static CardDisplayBehaviour Create(Card card, Vector3 positon, Quaternion rotation) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.cardDisplay,positon,rotation);
        var displayBehvaiour = GO.GetComponent<CardDisplayBehaviour>();
        displayBehvaiour.Init(card);
        return displayBehvaiour;
    }

    private void Init(Card card) {
        if (card == null) {
            gameObject.name = "dummy";
            return;
        }
        else {
            gameObject.name = card.name;
        }

        // image
        frontMaterial.Init();
        if (card.GetAvatarImage()) {
            frontMaterial.runtimeMat.mainTexture = card.GetAvatarImage();
        }

        // name
        name.text = card.name;
        // description
        description.text = card.description;

        if (card is CharacterCard) {
            var characterCard = card as CharacterCard;
            atkVal.text = characterCard.attributes.atkVal.ToString();
            hpVal.text = characterCard.attributes.HP.ToString();
        }
        else {
            atkVal.enabled = false;
            hpVal.enabled = false;
        }
    }
}
