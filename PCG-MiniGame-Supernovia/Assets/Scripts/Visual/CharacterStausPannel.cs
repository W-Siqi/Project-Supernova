using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStausPannel : MonoBehaviour
{
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
    public void OnSelect(CharacterCard characterCard) {
        selectedCharacter = characterCard;
    }

    public void HightLightCharacterTrait(CharacterCard characterCard, Trait trait) {
        characterStatusViewerDict[characterCard].HightlightTrait(trait);
    }

    public void Hide() {
        showupTween.Play(true);
    }

    public void Showup() {
        showupTween.Play();
    }

    private void Update() {
        var preY = 0f;
        foreach (var viewer in characterStatusViewers) {
            var posOffset = Vector3.zero;
            posOffset.y += preY;
            if (viewer.GetHooedCharacter() == selectedCharacter) {
                posOffset.x += selectedXOffset;
                posOffset.y += standardSpace * (selectedSizeAmplify - 1);
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
