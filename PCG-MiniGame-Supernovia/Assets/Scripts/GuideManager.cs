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
    [SerializeField]
    private string eventCard1Name;
    [SerializeField]
    private string eventCharacter1Name;
    [SerializeField]
    private string eventCard2Name;
    [SerializeField]
    private string eventCharacter2Name;
    [SerializeField]
    private List<StoryBook.PageContent> voteContents;

    public IEnumerator RunGuidence() {
        //yield return StartCoroutine(EventStreamTutorial());

        //yield return StartCoroutine(VoteTutorial());

        // yield return StartCoroutine(EventStreamTutorial());

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
        var showCard = Card.DeepCopy( DeckArchive.instance.FindCardByName(campfireCharacter1Name) as CharacterCard);
        var showCardDisplay = ViewManager.instance.ViewCardOnScreen(showCard);
        yield return new WaitForSeconds(2f);
        showCardDisplay.UpdatePersonality(showCard.personalities[1], Trait.hopeless);
        yield return new WaitForSeconds(5f);
        DestroyImmediate(showCardDisplay.gameObject);

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
        ViewManager.instance.EndViewCharacterOfDialog();
        yield return new WaitForSeconds(2f);
    }

    IEnumerator EventStreamTutorial() {
        yield return StartCoroutine( StoryBook.instance.ViewContent(new StoryBook.PageContent("过了几日")));

        // 事件1 - 偷窃
        var eventCard1 = DeckArchive.instance.FindCardByName(eventCard1Name) as EventCard;
        var eventCharacter1 = DeckArchive.instance.FindCardByName(eventCharacter1Name) as CharacterCard;
        tipViewer.ViewTip("人物的性格会导致不同的事件发生");
        StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent( "事件-"+eventCard1.name)));
        yield return StartCoroutine(ViewManager.instance.ViewCardsOnScreen(new Card[] { eventCard1, eventCharacter1 }));
        StoryContext.instance.statusVector.money -= 40;
        yield return new WaitForSeconds(5f);

        // 事件2 - 魔鬼区服
        var eventCard2 = DeckArchive.instance.FindCardByName(eventCard2Name) as EventCard;
        var eventCharacter2 = DeckArchive.instance.FindCardByName(eventCharacter2Name) as CharacterCard;
        StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent("事件-" + eventCard2.name)));
        yield return StartCoroutine(ViewManager.instance.ViewCardsOnScreen(new Card[] { eventCard2 }));


        var eventCharacter2Display = ViewManager.instance.ViewCardOnScreen(eventCharacter2);
        yield return new WaitForSeconds(2f);
        tipViewer.ViewTip("-事件的发生也会一定程度改变人格");
        eventCharacter2Display.UpdatePersonality(eventCharacter2.personalities[0], Trait.fury);
        eventCharacter2Display.UpdatePersonality(eventCharacter2.personalities[1], Trait.fury);
        yield return new WaitForSeconds(7f);
        DestroyImmediate(eventCharacter2Display.gameObject);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator VoteTutorial() {
        //投票介绍
        yield return StartCoroutine(ViewManager.instance.ViewReachNewStoryStageCoroutine(StoryStage.vote));
        yield return StartCoroutine(StoryBook.instance.ViewContent(voteContents[0]));

        // show up viewer
        var voteViwer = ViewManager.instance.voteViewer;
        voteViwer.ShowUp();
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(StoryBook.instance.ViewContent(voteContents[1]));

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

        yield return StartCoroutine(StoryBook.instance.ViewContent(voteContents[2]));        
    }
}
