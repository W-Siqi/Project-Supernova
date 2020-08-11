using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TraitTooltipDetector : MonoBehaviour
{
    public string toolTip = "";
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
        traitTooltip.traitDescriptionTMP.text = toolTip;
        traitTooltip.gameObject.SetActive(true);
    }
    public void OnPointerExit() {
        Debug.Log("Mouse exit");
        traitTooltip.gameObject.SetActive(false);
    }
}
