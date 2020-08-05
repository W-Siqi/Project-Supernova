using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusViewer : MonoBehaviour
{
    [SerializeField]
    private List<PersonalityViewerUGUI> personalityViewerUGUIs = new List<PersonalityViewerUGUI>();
    [SerializeField]
    private TextMeshProUGUI name;
    [SerializeField]
    private TextMeshProUGUI loyalty;
    [SerializeField]
    private Image avatarImage;

    private CharacterCard hookedCharacter = null;
    public void HookTo(CharacterCard character) {
        hookedCharacter = character;

        name.text = hookedCharacter.name;
        loyalty.text = hookedCharacter.loyalty.ToString();
        avatarImage.sprite = hookedCharacter.GetAvatarSprite();

        // personality
        if (hookedCharacter.personalities.Length != personalityViewerUGUIs.Count) {
            throw new System.Exception("UI个数不对");
        }
        for (int i = 0; i < hookedCharacter.personalities.Length; i++) {
            personalityViewerUGUIs[i].InitTo(hookedCharacter.personalities[i].trait);
        }
    }

    private void Update() {
        if (hookedCharacter != null) {
            loyalty.text = hookedCharacter.loyalty.ToString();

            for (int i = 0; i < hookedCharacter.personalities.Length; i++) {
                if (personalityViewerUGUIs[i].currentViewedTrait != hookedCharacter.personalities[i].trait) {
                    personalityViewerUGUIs[i].TransferTo(hookedCharacter.personalities[i].trait);
                }
            }
        }
    }
}
