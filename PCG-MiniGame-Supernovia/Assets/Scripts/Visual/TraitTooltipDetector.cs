using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TraitTooltipDetector : MonoBehaviour
{
    [SerializeField]
    PersonalityViewerUGUI personalityViewer;
    [SerializeField]
    TraitTooltip traitTooltip;

    private void Awake() {
        traitTooltip.gameObject.SetActive(false);
    }

    public void OnPointerEnter() {
        Debug.Log("Mouse enter");
        traitTooltip.traitFullNameTMP.text = TraitUtils.GetFullName( personalityViewer.currentViewedTrait);
        traitTooltip.traitDescriptionTMP.text = TraitUtils.GetTooltip(personalityViewer.currentViewedTrait);
        traitTooltip.gameObject.SetActive(true);
    }
    public void OnPointerExit() {
        Debug.Log("Mouse exit");
        traitTooltip.gameObject.SetActive(false);
    }
}
