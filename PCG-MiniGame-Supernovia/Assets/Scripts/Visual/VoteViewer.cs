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

    public void ShowUp() {
        // show up voter Group
        agreeGroup.ShowUp();
        disagreeGroup.ShowUp();
    }

    public void Hide() {
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

    public void ViewPlayerVoteNumber() { 
    }
}
