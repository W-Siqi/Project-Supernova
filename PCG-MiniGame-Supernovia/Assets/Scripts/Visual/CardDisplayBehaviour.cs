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
    [SerializeField]
    private PersonalityViewer[] personalityViewers = new PersonalityViewer[CharacterCard.PERSONALITY_COUNT];
    private Dictionary<Personality, PersonalityViewer> personalityViewerDict = new Dictionary<Personality, PersonalityViewer>();
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

    public static CardDisplayBehaviour Create(Card card, AnchorPoint spawnPoint) {
        return Create(card, spawnPoint.position, spawnPoint.rotation);
    }

    public void UpdateHPValue(float newHP) {
        hpVal.text = newHP.ToString();
    }

    /// <summary>
    /// personality传进来只是当索引，不会修改personality的值，只管显示
    /// </summary>
    /// <param name="hookedPersonality"></param>
    /// <param name="trait"></param>
    public void UpdatePersonality(Personality hookedPersonality,Trait newTrait) {
        personalityViewerDict[hookedPersonality].TransferTo(newTrait);
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
            // init values
            atkVal.text = characterCard.attributes.atkVal.ToString();
            hpVal.text = characterCard.attributes.HP.ToString();
            // init personalities
            for (int i = 0; i < CharacterCard.PERSONALITY_COUNT; i++) {
                personalityViewerDict[characterCard.personalities[i]] = personalityViewers[i];
                personalityViewers[i].InitTo(characterCard.personalities[i].currentTrait);
            }
        }
        else {
            atkVal.enabled = false;
            hpVal.enabled = false;
            foreach (var p in personalityViewers) {
                p.gameObject.SetActive(false);
            }
        }
    }
}
