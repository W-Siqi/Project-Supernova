using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 管理 决策 - 人物 - 事件 等游戏游玩主循环
public class PlayLoopManager : MonoBehaviour {
    bool userInputDecisionMade = false;
    bool userInputAgreeDecision = false;
    bool userInputNextRound = false;

    public IEnumerator PlayLoop() {
        for(int round = 0; round < PlayData.instance.gameConfig.roundCount; round++ ) {
            GameExecuter.ApplyBuffBeforeRound(PlayData.instance.gameState, PlayData.instance.gameConfig, true);

            yield return StartCoroutine(CouncilStage(round));

            yield return StartCoroutine(EventStream());

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                Debug.Log("游戏失败");
                break;
            }
        }
        Debug.Log("游戏结束");
    }

    IEnumerator EventStream() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex);
        StoryBook.instance.ViewContent(newPageContent);

        var desctriptionPlayer = ViewManager.instance.eventDescriptionPlayer;
        foreach (var selectedEvent in GameExecuter.SelectEventCards(PlayData.instance.gameState, PlayData.instance.gameConfig)) {
            var bindingInfos = selectedEvent.preconditonSet.Bind(PlayData.instance.gameState);

            // 必须要在apply结果前面进行演出
            var description = EventDescription.Generate(selectedEvent, bindingInfos);
            yield return StartCoroutine(desctriptionPlayer.PlayEventDescription(bindingInfos, description));

            selectedEvent.consequenceSet.Apply(bindingInfos, PlayData.instance.gameState);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CouncilStage(int curRound) {
        // new page
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        StoryBook.instance.ViewContent(newPageContent);

        // UI
        ViewManager.instance.InitViewForCouncialStage();

        foreach (var character in PlayData.instance.gameState.characterDeck) {
            if (character.HasTrait(Trait.silence)) {
                //沉默检测
                if (Random.value < PlayData.instance.gameConfig.slicentTraitSlicenceProbility) {
                    continue;
                }
            }

            // show 角色
            ViewManager.instance.ViewCharacterOfDialog(character);
            ViewManager.instance.characterStausPannel.OnSelect(character);

            // 提取建议卡
            var straCard =PlayData.instance.gameState.stratagemDict[character][curRound];
            // show Dialog
            ViewManager.instance.ViewDialog(straCard,character);

            // 等待用户输入
            yield return StartCoroutine(ResetAndWaitStratagemDecisionInput());
            // end show Dialog
            ViewManager.instance.EndViewDialog();

            // apply 
            straCard.consequenceSet.Apply(PlayData.instance.gameState, PlayData.instance.gameConfig, character, userInputAgreeDecision);
            // 稍等一会再进入下一个角色，因为当前动画演出
            yield return new WaitForSeconds(1f);

            // end character
            ViewManager.instance.EndViewCharacterOfDialog();
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
