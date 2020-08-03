using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VoterGroupViewer : MonoBehaviour
{
    private int DEFAULT_PENNAL_SZIE = 5;

    [SerializeField]
    private PositionTween showupTween;
    [SerializeField]
    private Transform voterSpawnAnchor;
    [SerializeField]
    private Transform voterFontAnchor;
    [SerializeField]
    private Transform voterBackAnchor;
    [SerializeField]
    private TextMeshProUGUI voteNumberTMP;
    [SerializeField]
    private SizeTween voteNumberTMPSizeTween;
    [SerializeField]
    private SizeTween groupWinShake;
    [SerializeField]
    private SizeTween groupWinTittleShowAndHide;

    private int curVoterCount = 0;
    private int curVoteNumber = 0;
    private List<GameObject> registerdVoterUI = new List<GameObject>();

    public void AddVote(Texture voterImage, int voteNumber) {
        // voter Show up
        var voterRawImageGO = Instantiate(
            ResourceTable.instance.prefabPage.characterUIRawImage
            ,voterSpawnAnchor.position
            ,voterSpawnAnchor.rotation);
        voterRawImageGO.transform.SetParent(voterSpawnAnchor);
        var voterRawImg = voterRawImageGO.GetComponent<RawImage>();
        voterRawImg.texture = voterImage;
        registerdVoterUI.Add(voterRawImageGO);

        // voter move to postion
        curVoterCount++;
        var destPos = Vector3.Lerp(voterFontAnchor.position, voterBackAnchor.position, (float)(curVoterCount - 1) / (float)DEFAULT_PENNAL_SZIE);
        LerpAnimator.instance.LerpPositionAndRotation(
            voterRawImageGO.transform,
            destPos,
            voterRawImageGO.transform.rotation,
            1f);
        // change vote number
        curVoteNumber += voteNumber;
        voteNumberTMP.text = curVoteNumber.ToString();
        voteNumberTMPSizeTween.Play();
    }

    public void AddPlayerVote(int voteNumber) {
        curVoteNumber += voteNumber;
        voteNumberTMP.text = curVoteNumber.ToString();
        voteNumberTMPSizeTween.Play();
    }

    public void ShowGroupWin() {
        groupWinTittleShowAndHide.gameObject.SetActive(true);
        groupWinShake.Play();
        groupWinTittleShowAndHide.Play();
    }


    public void ShowUp() {
        // play transform tween 
        showupTween.Play();
    }

    public void Hide() {
        showupTween.Play(true);
        foreach (var UI in registerdVoterUI) {
            Destroy(UI, 3f);
        }
    }
}
