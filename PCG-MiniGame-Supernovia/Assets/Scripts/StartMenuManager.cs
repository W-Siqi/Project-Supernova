using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public float selectedDifficulty = 0.2f;

    public GameObject UserInterface;
    public GameObject PCGDashboard;


    [SerializeField]
    float minDifficulty = 0.3f;
    [SerializeField]
    float maxDifficulty = 0.9f;

    [SerializeField]
    private Slider difficlutySlider;
    [SerializeField]
    private TextMeshProUGUI difficultyValueText;
    [SerializeField]
    private Button startGameButton;

    private bool gameStart = false;

    private void Awake() {
        startGameButton.onClick.AddListener(() => { gameStart = true; });
    }

    private void Update() {
        selectedDifficulty = Mathf.Lerp(minDifficulty, maxDifficulty, difficlutySlider.value);
        difficultyValueText.text = string.Format("{0}%",(int)(selectedDifficulty*100));
    }

    public IEnumerator WaitStartGame() {
        while (!gameStart) {
            yield return null;
        }
    }
}
