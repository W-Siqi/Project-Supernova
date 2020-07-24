using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemCardInteraction : MonoBehaviour {
    public delegate void OnDecisionMade(bool isAgree);

    private StratagemCard stratagemCard;
    private OnDecisionMade decisionMade;
    private CardDisplayBehaviour cardDisplay;

    public static StratagemCardInteraction Create(StratagemCard stratagemCard, OnDecisionMade onDecisionMade) {
        var GO = new GameObject("StratagemCardInteraction - " + stratagemCard.name);
        var interaction = GO.AddComponent<StratagemCardInteraction>();
        interaction.stratagemCard = stratagemCard;
        interaction.decisionMade = onDecisionMade;
        interaction.StartInteraction();
        return interaction;
    }

    private void StartInteraction() {
        var anchor = AnchorManager.instance.stratagemCardAnchor;
        cardDisplay = CardDisplayBehaviour.Create(stratagemCard, anchor.transform.position, anchor.transform.rotation);
    }


    private void EndInteraction(bool agreed) {
        decisionMade(agreed);
        DestroyImmediate(cardDisplay.gameObject);
        DestroyImmediate(gameObject);
    }


    // 暂时先用按键代替，之后再改..
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            EndInteraction(true);
        }

        if (Input.GetMouseButtonDown(1)) {
            EndInteraction(false);
        }
    }
}
