using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StratagemCardInteraction : MonoBehaviour {
    const float DISPEAR_ANIMATION_VALUE = 50f;
    const float DISPEAR_ANIMATION_TIME = 1f;

    public delegate void OnDecisionMade(bool isAgree);

    [SerializeField]
    RawImage image;
    [SerializeField]
    Text yesText;
    [SerializeField]
    Text noText;
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    private Swipe swipe;
    [SerializeField]
    [ReadOnlyInspector]
    private float acutalSwipeValue;
    [SerializeField]
    private Animator cardAnimator;

    private OnDecisionMade decisionMade;

    private bool onInteraction = true;

    public static StratagemCardInteraction Create(StratagemCard stratagemCard, OnDecisionMade onDecisionMade) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.startagemInteraction);
        var interaction = GO.GetComponent<StratagemCardInteraction>();
        interaction.decisionMade = onDecisionMade;

        var canvas = ResourceTable.instance.sceneReferencePage.swipeCanvas;
        interaction.transform.parent = canvas.transform;
        interaction.transform.localPosition = Vector3.zero;

        var tex = stratagemCard.GetAvatarImage();
        interaction.image.texture = tex;

        interaction.titleText.text = stratagemCard.name;
        interaction.descriptionText.text = stratagemCard.description;
        return interaction;
    }

    private void Update() {
        if (onInteraction) {
            acutalSwipeValue = swipe.getScaledSwipeVector().x;
            cardAnimator.SetFloat("CardPos", acutalSwipeValue);
        }
    }

    public void OnSwipeLeft() {
        if (!onInteraction) {
            return;
        }
        onInteraction = false;

        LerpAnimator.instance.LerpValues(
            acutalSwipeValue,
            -DISPEAR_ANIMATION_VALUE,
            DISPEAR_ANIMATION_TIME,
            (float val)=> { cardAnimator.SetFloat("CardPos", val); });

        decisionMade(true);

        Destroy(gameObject, DISPEAR_ANIMATION_TIME);
    }

    public void OnSwipeRight() {
        if (!onInteraction) {
            return;
        }
        onInteraction = false;

        LerpAnimator.instance.LerpValues(
            acutalSwipeValue,
            DISPEAR_ANIMATION_VALUE,
            DISPEAR_ANIMATION_TIME,
            (float val) => { cardAnimator.SetFloat("CardPos", val); });

        decisionMade(false);

        Destroy(gameObject, DISPEAR_ANIMATION_TIME);
    }
}
