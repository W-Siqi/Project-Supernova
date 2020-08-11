using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TraitTooltip : MonoBehaviour
{
    public TextMeshProUGUI traitFullNameTMP;
    public Text traitDescriptionTMP;

    private void Update() {
        transform.position = Input.mousePosition;
    }
}
