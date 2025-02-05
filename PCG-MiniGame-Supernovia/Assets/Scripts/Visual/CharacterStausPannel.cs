﻿using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不会主动update去监听状态，而是需要手动的调用API
/// </summary>
public class CharacterStausPannel : MonoBehaviour {
    public Transform upperLeftAnchor;
    public float standardSpace;
    public float selectedSizeAmplify = 1.2f;
    public float selectedXOffset;
    public PositionTween showupTween;

    private CharacterCard selectedCharacter;
    private List<CharacterStatusViewer> characterStatusViewers = new List<CharacterStatusViewer>();
    private Dictionary<CharacterCard, CharacterStatusViewer> characterStatusViewerDict = new Dictionary<CharacterCard, CharacterStatusViewer>();

    public void Init(CharacterCard[] characters) {
        // clear old
        foreach (var v in characterStatusViewers) {
            Destroy(v.gameObject);
        }
        characterStatusViewerDict.Clear();
        characterStatusViewers.Clear();

        foreach (var charcater in characters) {
            var GO = Instantiate(ResourceTable.instance.prefabPage.characterStatusViewer);
            var viewer = GO.GetComponent<CharacterStatusViewer>();
            characterStatusViewerDict[charcater] = viewer;
            characterStatusViewers.Add(viewer);

            viewer.HookTo(charcater);
            viewer.transform.SetParent(transform);
        }
    }

    public void ForceSync() {
        foreach (var viewer in characterStatusViewers) {
            viewer.ForceSync();
        }
    }

    // 会把这个角色状态放大突出
    public void OnSelect(CharacterCard characterCard) {
        selectedCharacter = characterCard;
    }

    public void ViewSentance(CharacterCard character, string sentance) {
        StartCoroutine(characterStatusViewerDict[character].ViewCharacterSentance(sentance));
    }

    public void ActivateTrait(CharacterCard character,Trait trait,string tooltip) {
        characterStatusViewerDict[character].ActivateTrait(trait,tooltip);
    }
    public void DisactivateAllTraits() {
        foreach (var viewer in characterStatusViewers) {
            viewer.DisactivateAllTraits();
        }
    }

    public IEnumerator ViewLoyaltyChange(CharacterCard character, int loyaltyDiff) {
        var characterViewer = characterStatusViewerDict[character];
        yield return StartCoroutine(characterViewer.ViewLoyaltyDelta(loyaltyDiff));
    }

    public IEnumerator ViewTraitChange(CharacterCard character, int personalityIndex, Trait newTrait) {
        var characterViewer = characterStatusViewerDict[character];
        yield return StartCoroutine(characterViewer.ViewTraitChange(personalityIndex,newTrait));
    }

    public void HightlightTrait(CharacterCard character, Trait trait) {
        characterStatusViewerDict[character].HightlightTrait(trait);
    }

    public void Hide() {
        showupTween.Play(true);
    }

    public void Showup() {
        showupTween.Play();
    }

    private void Update() {
        var preY = 0f;
        for (int i =0; i < characterStatusViewers.Count; i++) {
            var viewer = characterStatusViewers[i];
            var posOffset = Vector3.zero;
            posOffset.y += preY;
            if (viewer.GetHooedCharacter() == selectedCharacter) {
                posOffset.x += selectedXOffset;
                posOffset.y += standardSpace * ((selectedSizeAmplify - 1)/2);
                viewer.transform.localScale = Vector3.one * selectedSizeAmplify;
                preY += standardSpace * selectedSizeAmplify;
            }
            else {
                viewer.transform.localScale = Vector3.one;
                preY += standardSpace;
            }
            viewer.transform.position = upperLeftAnchor.transform.position + posOffset;
        }
    }
}
