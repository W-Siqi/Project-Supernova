using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;
using PCGP;

// 管理 决策 - 人物 - 事件 等游戏游玩主循环
public class PlayLoopManager : MonoBehaviour {
    bool userInputDecisionMade = false;
    bool userInputAgreeDecision = false;
    bool userInputNextRound = false;

    public IEnumerator PlayLoop() {
        yield return StartCoroutine(ViewManager.instance.ViewCardsOnScreen(PlayData.instance.gameState.characterDeck.ToArray()));

        for(int round = 0; round < PlayData.instance.gameConfig.roundCount; round++ ) {
            yield return StartCoroutine(CouncilStage(round));

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                break;
            }

            yield return StartCoroutine(EventStream());

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                break;
            }
        }
        Debug.Log("游戏结束");
    }

    IEnumerator EventStream() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        StoryBook.instance.ViewContent(newPageContent);
        yield return new WaitForSeconds(1.5f);

        var desctriptionPlayer = ViewManager.instance.eventDescriptionPlayer;
        foreach (var selectedEvent in GameExecuter.SelectEventCards(PlayData.instance.gameState, PlayData.instance.gameConfig)) {
            var bindingInfos = selectedEvent.preconditonSet.Bind(PlayData.instance.gameState);

            // 必须要在apply结果前面进行演出
            Debug.Log(string.Format("=========================[{0}]-[{1}]========", selectedEvent.name,selectedEvent.description));
            var description = EventDescription.Generate(selectedEvent, bindingInfos);

            yield return StartCoroutine(desctriptionPlayer.PlayEventDescription(bindingInfos, description));

            selectedEvent.consequenceSet.Apply(bindingInfos, PlayData.instance.gameState);

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                break;
            }
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CouncilStage(int curRound) {
        // new page
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex);
        StoryBook.instance.ViewContent(newPageContent);
        yield return new WaitForSeconds(1.5f);

        // UI
        ViewManager.instance.InitViewForCouncialStage();

        // 开局buff
        var modifyEvents = GameExecuter.CalculateBuffBeforeRound(PlayData.instance.gameState, PlayData.instance.gameConfig);
        yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvents(modifyEvents));
        GameStateModifyEvent.ApplyModificationsTo(PlayData.instance.gameState, modifyEvents);

        foreach (var character in PlayData.instance.gameState.characterDeck) {
            if (character.HasTrait(Trait.silence)) {
                //沉默检测
                if (Random.value < PlayData.instance.gameConfig.slicentTraitSlicenceProbility) {
                    continue;
                }
            }

            // 提取建议卡
            var straCard =PlayData.instance.gameState.stratagemDict[character][curRound];

            // 等待用户输入
            ViewManager.instance.ViewCharacterOfDialog(character);
            ViewManager.instance.characterStausPannel.OnSelect(character);
            ViewManager.instance.ViewDialog(straCard, character);
            yield return StartCoroutine(ResetAndWaitStratagemDecisionInput());
            ViewManager.instance.EndViewDialog();
            ViewManager.instance.EndViewCharacterOfDialog();

            // 计算
            var modifications = GameExecuter.CalculteStratagemDecision(
                PlayData.instance.gameState,
                PlayData.instance.gameConfig,
                straCard,
                character,
                userInputAgreeDecision);

            // 演出
            yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvents(modifications));

            // 应用
            GameStateModifyEvent.ApplyModificationsTo(PlayData.instance.gameState, modifications);

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                break;
            }
        }

        ViewManager.instance.characterStausPannel.Hide();

        // next round btn
        ViewManager.instance.ViewNextRoundBtn();
        yield return StartCoroutine(ResetAndWaitNextRoundInput());
        ViewManager.instance.EndNextRoundBtn();
    }

    public void OnUserInputAcceptStragem() {
       userInputDecisionMade = true;
       userInputAgreeDecision = true;
    }

    public void OnUserInputDeclineStragem() {
        userInputDecisionMade = true;
        userInputAgreeDecision = false;
    }

    public void OnUserInputNextRound() {
        userInputNextRound = true;
    }

    private IEnumerator ResetAndWaitStratagemDecisionInput() {
        userInputDecisionMade = false;
        userInputAgreeDecision = false;
        while (!userInputDecisionMade) {
            yield return null;
        }
    }
    private IEnumerator ResetAndWaitNextRoundInput() {
        userInputNextRound = false;
        while (!userInputNextRound) {
            yield return null;
        }
    }
}
