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
        yield return StartCoroutine(ViewManager.instance.ViewWinConditon(PlayData.instance.gameConfig.roundCount));

        for(int round = 0; round < PlayData.instance.gameConfig.roundCount; round++ ) {
            PlayData.instance.gameState.RefreshForNewRound();

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
        var straDelta = stratagem.consequenceSet.statusConsequenceWhenAccept.delta;
        var traitsPannel = ViewManager.instance.characterStausPannel;

        ViewManager.instance.statusVectorPannel.ActivateRelatedValues(stratagem.consequenceSet.statusConsequenceWhenAccept.delta);
        foreach (var character in PlayData.instance.gameState.characterDeck) {
            if (straDelta.people > 0) {
                traitsPannel.ActivateTrait(character, Trait.silence,"这是提升民心的建议，陛下若不采纳会忠诚度-1");
            }
            if (straDelta.army > 0) {
                traitsPannel.ActivateTrait(character, Trait.warlike,"这是提升军队的建议，陛下若不采纳会忠诚度-1");
            }
            if (straDelta.money > 0) {
                traitsPannel.ActivateTrait(character, Trait.corrupt,"虽然次计增加收入，但腐败之人会从中获取提成");
            }

            if (provider.HasTrait(Trait.honest)) {
                traitsPannel.ActivateTrait(character, Trait.corrupt,string.Format("采用{0}的建议, 一定几率会消除{1}的腐败性格",provider.name,character.name));
            }

            if (provider.HasTrait(Trait.cruel)) {
                traitsPannel.ActivateTrait(character, Trait.tolerant, character.name+ "宽容的性格，会降低你采纳“残暴”所带来的副作用");
            }

            if (provider.HasTrait(Trait.arrogent)) {
                traitsPannel.ActivateTrait(character, Trait.arrogent,string.Format("{0}和{1}同为傲慢之人，无论采纳谁的建议，另一个人都会忠诚度-1",provider.name, character.name));
            }

            traitsPannel.ActivateTrait(character, Trait.jealous, string.Format("{0}为嫉妒之人，一回合内采纳3个以上他人的建议，忠诚度-1",character.name));
        }
        Trait[] traitsToActivate = { Trait.arrogent, Trait.warlike, Trait.wise, Trait.cruel, Trait.tolerant, Trait.tricky , Trait.honest};
        foreach (var t in traitsToActivate) {
            traitsPannel.ActivateTrait(provider, t, TraitUtils.GetTooltip(t));
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
