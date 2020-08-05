﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 管理 决策 - 人物 - 事件 等游戏游玩主循环
public class PlayLoopManager : MonoBehaviour {
    [System.Serializable]
    private class Config {
        public bool playInQuickMode = true;
        public int eventCountPerRound = 2;
    }

    [SerializeField]
    private List<StoryStage> storyline = new List<StoryStage>();
    [SerializeField]
    private Config config;

    public void StartPlayLoop() {
        StartCoroutine(PlayLoopCoroutine());
    }

    IEnumerator PlayLoopCoroutine() {
        int round = 0;
        foreach (var stage in storyline) {
            if (round >= PCGVariableTable.instance.roundCount) {
                Debug.Log("游戏胜利");
                break;
            }

            if (!config.playInQuickMode) {
                yield return StartCoroutine(ViewManager.instance.ViewReachNewStoryStageCoroutine(stage));
            }

            yield return StartCoroutine(CouncilStage(round));

            yield return StartCoroutine(EventStream());

            BuffApplyBeforeRoundEnd();

            if (CheckDeathEnding()) {
                Debug.Log("游戏失败");
                break;
            }

            round++;
        }
        Debug.Log("游戏结束");
    }

    private bool CheckDeathEnding() {
        var status = StoryContext.instance.statusVector;
        if (status.army < 0 || status.money < 0 || status.people < 0) {
            return true;
        }
        foreach (var character in StoryContext.instance.characterDeck) {
            if (character.loyalty <= 0) {
                return true;
            }
        }
        return false;
    }

    IEnumerator EventStream() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));
        var eventViewer = ViewManager.instance.eventViewer;
        var qualifeiedEvents = EventCardFilter.Filt();
        for (int i = 0; i < config.eventCountPerRound; i++) {
            var selectedEvent = qualifeiedEvents[Random.Range(0, qualifeiedEvents.Length)];
            var bindingInfos = selectedEvent.preconditonSet.Bind();

            // 演出event
            // TBD: 可能会出错，如果consequence如果是运行时随机的话不是确定的话，show在没apply的时候不知道确定值
            yield return StartCoroutine(eventViewer.ViewEventCoroutine(selectedEvent, bindingInfos));

            // apply consequence
            selectedEvent.consequenceSet.Apply(bindingInfos);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CouncilStage(int curRound) {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

        foreach (var character in StoryContext.instance.characterDeck) {
            if (character.HasTrait(Trait.silence)) {
                //沉默检测
                if (Random.value < PCGVariableTable.instance.slicentTraitSlicenceProbility) {
                    continue;
                }
            }

            // show 角色
            ViewManager.instance.ViewCharacterOfDialog(character.GetAvatarImage());

            // 提取建议卡
            var straCard = StoryContext.instance.stratagemDict[character][curRound];
            // show Dialog
            ViewManager.instance.ViewDialog(straCard.description);

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

            if (agreeDecision) {
                // 应用后果
                straCard.consequenceSet.Apply(character, true);

                // 残暴词缀应用
                if (character.HasTrait(Trait.cruel)) {
                    StoryContext.instance.statusVector.people -= PCGVariableTable.instance.cruelTraitPeopleValuePerDecision;
                }
            }
            else {
                straCard.consequenceSet.Apply(character, false);
            }

            // end show Dialog
            ViewManager.instance.EndViewDialog();
            // end character
            ViewManager.instance.EndViewCharacterOfDialog();
        }
    }

    private void BuffApplyBeforeRoundEnd() {
        foreach (var character in StoryContext.instance.characterDeck) {
            if (character.HasTrait(Trait.corrupt)) {
                var corrputValue = PCGVariableTable.instance.corrputTraitMoneyPerRound;
                StoryContext.instance.statusVector.money -= corrputValue;
            }
        }
    }

    //IEnumerator VoteState() {
    //    // show up viewer
    //    var voteViwer = ViewManager.instance.voteViewer;
    //    voteViwer.ShowUp();
    //    yield return new WaitForSeconds(2.5f);

    //    var voters = StoryContext.instance.characterDeck.ToArray();
    //    int playerVoteIndex = Random.Range(0, voters.Length);
    //    int agreeVoteNumber = 0;
    //    int disagreeVoteNumber = 0;
    //    for (int i = 0; i < voters.Length; i++) {
    //        // NPC Vote
    //        var NPCVoter = voters[i];
    //        var NPCVoteNumber = Random.Range(1, 7);
    //        var agree = Random.value < 0.5f;
    //        if (agree) {
    //            agreeVoteNumber += NPCVoteNumber;
    //        }
    //        else {
    //            disagreeVoteNumber += NPCVoteNumber;
    //        }
    //        voteViwer.NPCVote(agree, NPCVoteNumber, NPCVoter);
    //        yield return new WaitForSeconds(1.2f);

    //        // Player Vote
    //        if (i == playerVoteIndex) {
    //            // viewer 准备
    //            voteViwer.ViewBeforePlayerVote();
    //            yield return new WaitForSeconds(2f);

    //            // 玩家投票,生成交互
    //            bool decisionMade = false;
    //            bool agreeDecision = false;
    //            DecisionInteraction.Create(
    //                ResourceTable.instance.texturepage.aynominousCharacter,
    //                "投票",
    //                "是否放逐",
    //                (ag) => { decisionMade = true; agreeDecision = ag; }); ;

    //            // 等待玩家输入
    //            while (!decisionMade) {
    //                yield return null;
    //            }

    //            var playerVoteNumber = 50;
    //            if (agreeDecision) {
    //                agreeVoteNumber += playerVoteNumber;
    //                voteViwer.PlayrVote(true,playerVoteNumber);
    //            }
    //            else {
    //                disagreeVoteNumber += playerVoteNumber;
    //                voteViwer.PlayrVote(false, playerVoteNumber);
    //            }
    //        }
    //        yield return new WaitForSeconds(1f);
    //    }

    //    if (agreeVoteNumber > disagreeVoteNumber) {
    //        voteViwer.ViewVoteResult(true);
    //    }
    //    else {
    //        voteViwer.ViewVoteResult(false);
    //    }

    //    yield return new WaitForSeconds(3f);
    //    voteViwer.Hide();
    //}

    //IEnumerator FightStage() {
    //    var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.fightSceneRT);
    //    yield return StartCoroutine(StoryBook.instance.ViewContent(newPageContent));

    //    var enemies = FightManager.instance.InstanticteRandomEnemies();
    //    yield return StartCoroutine(FightManager.instance.ExecuteFight(StoryContext.instance.characterDeck.ToArray(),enemies));
    //}
}
