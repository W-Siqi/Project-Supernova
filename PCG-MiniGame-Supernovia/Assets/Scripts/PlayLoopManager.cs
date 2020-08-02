using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 管理 决策 - 人物 - 事件 等游戏游玩主循环
public class PlayLoopManager : MonoBehaviour {
    [SerializeField]
    private List<StoryStage> storyline = new List<StoryStage>();
    [SerializeField]
    private bool playInQuickMode = true;

    // 每个character 有对应的专属filer
    private Dictionary<CharacterCard, StratagemProbFilter> straProbFilters = new Dictionary<CharacterCard, StratagemProbFilter>();
    private Dictionary<CharacterCard, StratagemCountFilter> straCountFilers = new Dictionary<CharacterCard, StratagemCountFilter>();

    public void StartPlayLoop() {
        StartCoroutine(PlayLoopCoroutine());
    }

    IEnumerator PlayLoopCoroutine(){
        foreach (var stage in storyline) {
            if (!playInQuickMode) {
                yield return StartCoroutine(ViewManager.instance.ViewReachNewStoryStageCoroutine(stage));
            }

            switch (stage) {
                case StoryStage.campfire:
                    yield return StartCoroutine(CampFireStage());
                    break;
                case StoryStage.fight:
                    yield return StartCoroutine(FightStage());
                    break;
                case StoryStage.vote:
                    yield return StartCoroutine(VoteState());
                    break;
            }

            yield return StartCoroutine(EventStream());
            yield return null;
        }
        Debug.Log("游戏结束");
    }


    // TBD - 副本和支线暂时废弃
    //IEnumerator SubstoryCardCheckPoint() {
    //    var subStories = DeckArchive.instance.substoryCards;
    //    if (subStories.Count > 0) {
    //        var selectedSubstory = subStories[Random.Range(0, subStories.Count)];
    //        // 动画演出
    //        yield return StartCoroutine(ShowManager.instance.SubstoryArriveShow(selectedSubstory));
    //        if (selectedSubstory.type == SubstoryCard.Type.dungeon) {
    //            // 副本卡
    //            StoryContext.instance.PushSubstory(selectedSubstory);
    //        }
    //        else {
    //            //支线卡
    //            var newCards = new List<Card>();
    //            newCards.AddRange(selectedSubstory.newCharacters);
    //            newCards.AddRange(selectedSubstory.eventCards);
    //            newCards.AddRange(selectedSubstory.stratagemCards);

    //            StoryContext.instance.characterDeck.AddRange(selectedSubstory.newCharacters);
    //            StoryContext.instance.eventDeck.AddRange(selectedSubstory.eventCards);
    //            StoryContext.instance.stratagemDeck.AddRange(selectedSubstory.stratagemCards);
    //        }
    //    }
    //}

    IEnumerator EventStream() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

        var selecedEvents = CardSelector.Select(new EventProbFilter(), new EventCountFilter());
        var eventViewer = ViewManager.instance.eventViewer;
        foreach (var c in selecedEvents) {
            var eventCard = c as EventCard;

            // binding
            var bindingInfos = eventCard.preconditonSet.Bind();

            // 演出event
            yield return StartCoroutine(eventViewer.ViewEventCoroutine(eventCard,bindingInfos));

            // TBD: 可能会出错，如果consequence不是确定的话，show在没apply的时候不知道确定值
            // apply consequence
            ConsequenceApplier.Apply(eventCard.consequenceSet, bindingInfos);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CampFireStage() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

        foreach (var character in StoryContext.instance.characterDeck) {
            // show 角色
            ViewManager.instance.ViewCharacterOfDialog(character.GetAvatarImage());

            if (!straProbFilters.ContainsKey(character)) {
                straProbFilters[character] = new StratagemProbFilter(character);
            }
            if (!straCountFilers.ContainsKey(character)) {
                straCountFilers[character] = new StratagemCountFilter(character);
            }

            // 抽取决策卡，并逐个进行决策流程
            var stratagesOfCharacter = CardSelector.Select(straProbFilters[character],straCountFilers[character]);
            foreach (var s  in stratagesOfCharacter) {
                var straCard = s as StratagemCard;
                // show Dialog
                ViewManager.instance.ViewDialog(straCard.description);

                yield return new WaitForSeconds(1f);

                // 生成交互
                bool decisionMade = false;
                bool agreeDecision = false;
                DecisionInteraction.Create(
                    straCard,
                    (bool agree) => { decisionMade = true; agreeDecision = agree; });

                // 等待玩家输入
                while (!decisionMade) {
                    yield return null;
                }

                // 应用后果
                var bindingInfos = straCard.preconditonSet.Bind();
                if (agreeDecision) {
                    // 如果采纳，则不进行后续的策略建议
                    ConsequenceApplier.Apply(straCard.consequenceSet,bindingInfos);
                    break;
                }

                // end show Dialog
                ViewManager.instance.EndViewDialog();
                yield return new WaitForSeconds(2f);
            }

            // end character
            ViewManager.instance.EndViewCharacterOfDialog();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator VoteState() {
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
                    "投票",
                    "是否放逐",
                    (ag) => { decisionMade = true; agreeDecision = ag; }); ;

                // 等待玩家输入
                while (!decisionMade) {
                    yield return null;
                }

                var playerVoteNumber = 50;
                if (agreeDecision) {
                    agreeVoteNumber += playerVoteNumber;
                    voteViwer.PlayrVote(true,playerVoteNumber);
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

    IEnumerator FightStage() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.fightSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

        var enemies = FightManager.instance.InstanticteRandomEnemies();
        yield return StartCoroutine(FightManager.instance.ExecuteFight(StoryContext.instance.characterDeck.ToArray(),enemies));
    }
}
