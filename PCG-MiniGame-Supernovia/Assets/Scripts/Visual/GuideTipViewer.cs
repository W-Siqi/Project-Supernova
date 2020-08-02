using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideTipViewer : MonoBehaviour
{
    [SerializeField]
    private PositionTween showUpTween;
    [SerializeField]
    private TextMeshProUGUI tipText;
    [SerializeField]
    private Button gotItButton;


    private void Awake() {
        gotItButton.onClick.AddListener(() => {
            showUpTween.Play(true);
        });
    }

    public void ViewTip(string tip) {
        tipText.text = tip;
        showUpTween.Play();
    }
}
