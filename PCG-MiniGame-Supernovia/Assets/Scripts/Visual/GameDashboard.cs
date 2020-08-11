using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDashboard : MonoBehaviour
{
    public GameStageViewer councialStageViewer;
    public GameStageViewer eventStreamStageViewer;
    public TextMeshProUGUI roundText;
    public Slider processSlider;

    public void UpdateState(int curRound, int totalRound, bool councilStage) {
        roundText.text = "第" + (curRound+1).ToString() + "回合";
        processSlider.value = (float)(curRound +1)/ (float)totalRound;
        if (councilStage) {
            councialStageViewer.Activate();
            eventStreamStageViewer.Disactivate();
        }
        else {
            councialStageViewer.Disactivate();
            eventStreamStageViewer.Activate();
        }
    }
}
