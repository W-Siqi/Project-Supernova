﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnchorPoint : MonoBehaviour
{
    public const string ANCHOR_TAG = "anchor";

    public string name = "anchor point";
    public TextMesh textMesh = null;

    public Vector3 position { get { return transform.position; } }
    public Quaternion rotation { get { return transform.rotation; } }

    private void Start() {
        gameObject.tag = ANCHOR_TAG;
    }

    private void Update() {
        gameObject.name = ("anchor - " + name);
        textMesh.text = name;
    }
}
