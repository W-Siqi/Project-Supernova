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
        //yield return StartCoroutine(ViewManager.instance.ViewCardsOnScreen(PlayData.instance.gameState.characterDeck.ToArray()));

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

    IEnumerator CouncilStage(int curRound) {
        // new page
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex);
        StoryBook.instance.ViewContent(newPageContent);
        yield return new WaitForSeconds(1.5f);

        // UI
        ViewManager.instance.InitViewForCouncialStage();

        // 开局buff
        var modifyEvents = GameExecuter.CalculateBuffBeforeRound(PlayData.instance.gameState, PlayData.instance.gameConfig);
        yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvents(PlayData.instance.gameState,modifyEvents));
        GameStateModifyEvent.ApplyModificationsTo(PlayData.instance.gameState, modifyEvents);

        foreach (var character in PlayData.instance.gameState.characterDeck) {
            // [没走GameExecuter！]沉默检测 - 当场播动画 
            if (character.HasTrait(Trait.silence)) {
                if (Random.value < PlayData.instance.gameConfig.slicentTraitSlicenceProbility) {
                    var silenceEvent = new GameStateModifyEvent(PlayData.instance.gameState, character, Trait.silence);
                    yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvent(PlayData.instance.gameState,silenceEvent));
                    continue;
                }
            }

            // 提取建议卡
            var straCard = PlayData.instance.gameState.GetStratagem(character,curRound);

            // 等待用户输入
            ActivateDecisionElements(straCard, character);
            ViewManager.instance.ViewCharacterOfDialog(character);
            ViewManager.instance.characterStausPannel.OnSelect(character);
            ViewManager.instance.ViewDialog(straCard, character);
            yield return StartCoroutine(ResetAndWaitStratagemDecisionInput());
            ViewManager.instance.EndViewDialog();
            ViewManager.instance.EndViewCharacterOfDialog();
            DisactivateDecisionElements();

            // 计算
            var modifications = GameExecuter.CalculteStratagemDecision(
                PlayData.instance.gameState,
                PlayData.instance.gameConfig,
                straCard,
                character,
                userInputAgreeDecision);

            // 演出
            yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvents(PlayData.instance.gameState,modifications));

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

    IEnumerator EventStream() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        StoryBook.instance.ViewContent(newPageContent);
        yield return new WaitForSeconds(1.5f);

        foreach (var selectedEvent in GameExecuter.SelectEventCards(PlayData.instance.gameState, PlayData.instance.gameConfig)) {
            // 绑定
            var bindingInfos = GameExecuter.BindEventCharacters(PlayData.instance.gameState, selectedEvent);
            // 计算
            var modification = GameExecuter.CalculteEventConsequence(PlayData.instance.gameState, PlayData.instance.gameConfig, selectedEvent, bindingInfos);
            // 演出
            yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvent(PlayData.instance.gameState,modification));
            // 应用
            GameStateModifyEvent.ApplyModificationsTo(PlayData.instance.gameState, new GameStateModifyEvent[] { modification});

            if (GameExecuter.HasReachDeath(PlayData.instance.gameState)) {
                break;
            }
        }
        yield return new WaitForSeconds(2f);

        // 最后记得force sync
        ViewManager.instance.characterStausPannel.ForceSync();
        ViewManager.instance.statusVectorPannel.ForceSync(PlayData.instance.gameState.statusVector);
    }

    private void ActivateDecisionElements(StratagemCard stratagem, CharacterCard provider) {
        ViewManager.instance.statusVectorPannel.ActivateRelatedValues(stratagem.consequenceSet.statusConsequenceWhenAccept.delta);
        foreach (var character in PlayData.instance.gameState.characterDeck) {
            ViewManager.instance.characterStausPannel.ActivateTrait(character, Trait.jealous);
        }
        Trait[] traitsToActivate = { Trait.arrogent, Trait.warlike, Trait.wise, Trait.cruel, Trait.tolerant, Trait.tricky };
        foreach (var t in traitsToActivate) {
            ViewManager.instance.characterStausPannel.ActivateTrait(provider, t);
        }
    }

    private void DisactivateDecisionElements() {
        ViewManager.instance.statusVectorPannel.DisactivateAllValues();
        ViewManager.instance.characterStausPannel.DisactivateAllTraits();
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
