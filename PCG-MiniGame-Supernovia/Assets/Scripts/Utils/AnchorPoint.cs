using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnchorPoint : MonoBehaviour
{
    public string name = "anchor point";
    public TextMesh textMesh = null;

    private void Update() {
        gameObject.name = ("anchor - " + name);
        textMesh.text = name;
    }
}
