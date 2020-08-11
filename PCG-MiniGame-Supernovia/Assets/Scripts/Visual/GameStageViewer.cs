using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStageViewer : MonoBehaviour
{
    public SizeTween toActivateTween;
    public SizeTween toDisactivateTween;
    public RawImage stageImage;
    public Text stageText;

    public void Activate() {
        stageImage.color = Color.white;
        stageText.color = Color.white;
        toActivateTween.Play();
    }

    public void Disactivate() {
        stageImage.color = 0.3f * Color.white;
        stageText.color = 0.1f *  Color.white;
        toDisactivateTween.Play();
    }
}
