using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeMaterial : MonoBehaviour
{
    [SerializeField]
    private Renderer targetRenderer;

    public Material runtimeMat { get; private set; } = null;

    private void Awake() {
        if (runtimeMat == null) {
            Init();
        }
    }

    public void Init() {
        runtimeMat = new Material(targetRenderer.sharedMaterial);
        targetRenderer.sharedMaterial = runtimeMat;
    }
}
