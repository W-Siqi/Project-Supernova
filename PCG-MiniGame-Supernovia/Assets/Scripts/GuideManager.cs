using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    [SerializeField]
    private GuideTipViewer tipViewer;
    [SerializeField]
    private List<StoryBook.PageContent> introPageContents;
    [SerializeField]
    private string campfireCharacter1Name;
    [SerializeField]
    private string campfireStratagem1Name;
    [SerializeField]
    private string campfireCharacter2Name;
    [SerializeField]
    private string campfireStratagem2Name;


    public IEnumerator RunGuidence() {
        //yield return StartCoroutine(EventStreamTutorial());

        //yield return StartCoroutine(VoteTutorial());
        // TMP 
        yield return StartCoroutine(IntroStage());
        yield return StartCoroutine(TutorialStage());
    }

    IEnumerator IntroStage() {
        foreach (var content in introPageContents) {
            yield return StartCoroutine(StoryBook.instance.ViewContent(content));
        }
    }

    IEnumerator TutorialStage() {
        // 人物介绍
        tipViewer.ViewTip("这些是避难所的成员");
        yield return ViewManager.instance.ViewCardsOnScreen(StoryContext.instance.characterDeck.ToArray());

        yield return StartCoroutine(CampfireTutorial());

        yield return StartCoroutine(EventStreamTutorial());

        yield return StartCoroutine(VoteTutorial());
    }

    IEnumerator CampfireTutorial() {

        // 营火介绍
        yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent("故事每天上演")));
        yield return StartCoroutine(ViewManager.instance.ViewReachNewStoryStageCoroutine(StoryStage.campfire));
        tipViewer.ViewTip("在营火,你和成员交谈");
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

        // 人物对话1
        yield return StartCoroutine(CharacterDialog(campfireCharacter1Name, campfireStratagem1Name));

        tipViewer.ViewTip("你的选择会对人物造成影响");
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(CharacterDialog(campfireCharacter2Name, campfireStratagem2Name));
    }

    IEnumerator CharacterDialog(string characterName, string stratagemName) {
        var character = DeckArchive.instance.FindCardByName(characterName) as CharacterCard;
        var stratagem = DeckArchive.instance.FindCardByName(stratagemName) as StratagemCard;

        ViewManager.instance.ViewCharacterOfDialog(character.GetAvatarImage());
        ViewManager.instance.ViewDialog(stratagem.description);

        yield return new WaitForSeconds(1f);

        // 生成交互
        bool decisionMade = false;
        bool agreeDecision = false;
        DecisionInteraction.Create(
            stratagem,
            (bool agree) => { decisionMade = true; agreeDecision = agree; });

        // 等待玩家输入
        while (!decisionMade) {
            yield return null;
        }

        // end show Dialog
        ViewManager.instance.EndViewDialog();
        yield return new WaitForSeconds(2f);
    }

    IEnumerator EventStreamTutorial() {
        yield return null;
    }

    IEnumerator VoteTutorial() {
        //投票介绍
        yield return StartCoroutine(ViewManager.instance.ViewReachNewStoryStageCoroutine(StoryStage.vote));
        yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent("-最近暴风雪连连，食物不足，人们决定驱逐一些人")));

        // show up viewer
        var voteViwer = ViewManager.instance.voteViewer;
        voteViwer.ShowUp();
        yield return new WaitForSeconds(2.5f);

        var voters = StoryContext.instance.characterDeck.ToArray();
        int playerVoteIndex = Random.Range(0, voters.Length);
        int agreeVoteNumber = 0;
        int disagreeVoteNumber = 0;
        for (int i = 0; i < voters.Length; i++) {
            // NPC Vote
            var NPCVoter = voters[i];
            var NPCVoteNumber = Random.Range(1, 7);
            var agree = Random.value < 0.5f;
            if (agree) {
                agreeVoteNumber += NPCVoteNumber;
            }
            else {
                disagreeVoteNumber += NPCVoteNumber;
            }
            voteViwer.NPCVote(agree, NPCVoteNumber, NPCVoter);
            yield return new WaitForSeconds(1.2f);

            // Player Vote
            if (i == playerVoteIndex) {
                // viewer 准备
                voteViwer.ViewBeforePlayerVote();
                yield return new WaitForSeconds(2f);

                // 玩家投票,生成交互
                bool decisionMade = false;
                bool agreeDecision = false;
                DecisionInteraction.Create(
                    ResourceTable.instance.texturepage.aynominousCharacter,
                    "拥有票数",
                    "20",
                    (ag) => { decisionMade = true; agreeDecision = ag; }); ;

                tipViewer.ViewTip("你的票数和你的威信有关");

                // 等待玩家输入
                while (!decisionMade) {
                    yield return null;
                }

                var playerVoteNumber = 20;
                if (agreeDecision) {
                    agreeVoteNumber += playerVoteNumber;
                    voteViwer.PlayrVote(true, playerVoteNumber);
                }
                else {
                    disagreeVoteNumber += playerVoteNumber;
                    voteViwer.PlayrVote(false, playerVoteNumber);
                }
            }
            yield return new WaitForSeconds(1f);
        }

        if (agreeVoteNumber > disagreeVoteNumber) {
            voteViwer.ViewVoteResult(true);
        }
        else {
            voteViwer.ViewVoteResult(false);
        }

        yield return new WaitForSeconds(3f);
        voteViwer.Hide();

    }
}
