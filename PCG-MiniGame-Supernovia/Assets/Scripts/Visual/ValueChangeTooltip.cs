using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueChangeTooltip : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private void Update() {
        transform.position = Input.mousePosition;
    }
}
