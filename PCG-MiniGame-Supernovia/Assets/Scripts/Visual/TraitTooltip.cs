using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitTooltip : MonoBehaviour
{
    public TextMeshProUGUI traitFullNameTMP;
    public TextMeshProUGUI traitDescriptionTMP;

    private void Update() {
        transform.position = Input.mousePosition;
    }
}
