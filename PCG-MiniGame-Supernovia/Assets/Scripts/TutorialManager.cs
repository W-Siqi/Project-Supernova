using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance = null;

    public SizeTween mainStatusTutorial;
    public SizeTween loyaltyStatusTutorial;
    public SizeTween stratagemTutorial;
    public SizeTween highlightTutorial;
    public SizeTween traitStoryTutorial;

    private void Awake() {
        instance = this;
    }

    public void OpenMainStatusTutorial(int round, int characterOrder) {
        if (characterOrder != 0 || round != 0 || !GlobalSettings.instance.openTutorial) {
            return;
        }
        mainStatusTutorial.gameObject.SetActive(true);
        mainStatusTutorial.Play();
    }

    public void OpenLoyaltyStatusTutorial(int round, int characterOrder) {
        if (characterOrder != 0 || round != 0 || !GlobalSettings.instance.openTutorial) {
            return;
        }
        loyaltyStatusTutorial.gameObject.SetActive(true);
        loyaltyStatusTutorial.Play();

    }
    public void OpenStratagemTutorial(int round, int characterOrder) {
        if (characterOrder != 1 || round != 0 || !GlobalSettings.instance.openTutorial) {
            return;
        }
        stratagemTutorial.gameObject.SetActive(true);
        stratagemTutorial.Play();
    }

    public void OpenHighliightTutorial(int round,int characterOrder) {
        if (characterOrder != 1 || round != 0 || !GlobalSettings.instance.openTutorial) {
            return;
        }
        highlightTutorial.gameObject.SetActive(true);
        highlightTutorial.Play();
    }

    public void OpenTraitStoryTutorial(int round) {
        if (round != 0 || !GlobalSettings.instance.openTutorial) {
            return;
        }
        traitStoryTutorial.gameObject.SetActive(true);
        traitStoryTutorial.Play();
    }
}
