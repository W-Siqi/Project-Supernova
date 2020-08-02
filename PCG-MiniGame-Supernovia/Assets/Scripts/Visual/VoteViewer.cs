using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoteViewer : MonoBehaviour
{
    [SerializeField]
    private VoterGroupViewer agreeGroup;
    [SerializeField]
    private VoterGroupViewer disagreeGroup;
    [SerializeField]
    private SizeTween voteTitleShowAndHide;
    [SerializeField]
    private SizeTween yourTurnTitleShowAndHide;
    public void ShowUp() {
        // show up voter Group
        agreeGroup.ShowUp();
        disagreeGroup.ShowUp();
        voteTitleShowAndHide.gameObject.SetActive(true);
        voteTitleShowAndHide.Play();
    }

    public void Hide() {
        voteTitleShowAndHide.gameObject.SetActive(false);
        agreeGroup.Hide();
        disagreeGroup.Hide();
    }

    public void NPCVote(bool voteForAgree,int voteNumber,CharacterCard voter) {
        if (voteForAgree) {
            agreeGroup.AddVote(voter.GetAvatarImage(), voteNumber);
        }
        else {
            disagreeGroup.AddVote(voter.GetAvatarImage(), voteNumber);
        }
    }

    public void PlayrVote(bool voteForAgree, int voteNumber) {
        if (voteForAgree) {
            agreeGroup.AddPlayerVote(voteNumber);
        }
        else {
            disagreeGroup.AddPlayerVote(voteNumber);
        }
    }

    public void ViewVoteResult(bool isAgreeGroupWin) {
        if (isAgreeGroupWin) {
            agreeGroup.ShowGroupWin();
        }
        else {
            disagreeGroup.ShowGroupWin();
        }
    }

    public void ViewBeforePlayerVote() {
        StartCoroutine(BeforePlayerVote());
    }

    IEnumerator BeforePlayerVote() {
        yourTurnTitleShowAndHide.gameObject.SetActive(true);
        yourTurnTitleShowAndHide.Play();

        yield return new WaitForSeconds(2f);

        // 权威显示的动画
    }
}
