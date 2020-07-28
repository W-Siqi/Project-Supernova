using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 播放story的时候，无视玩家输入的结果。
// 也就是eventcard和stratagemcard完全是按照记录播放，而不是根据上下文播放
public class SerializedStoryPlayer : MonoBehaviour
{
    public SerializedStory serializedStory;


    private bool isPlaying = false;

    [ContextMenu("Create Empty serialized story")]
    private void CreateEmaptySerializedStory() {
        serializedStory = SerializedStory.CreateAndSave();
    }

    public void Play() {
        if (!isPlaying) {
            isPlaying = true;
            StartCoroutine(PlayStory());
        }
    }

    IEnumerator PlayStory() {
        // 初始化故事状态
        int seed = Random.Range(-10000, 100000);
        StoryContext.instance.InitForNewStory(seed);

        // 洗牌动画
        var cardsInDeck = new List<Card>();
        cardsInDeck.AddRange(StoryContext.instance.characterDeck);
        cardsInDeck.AddRange(StoryContext.instance.stratagemDeck);
        cardsInDeck.AddRange(StoryContext.instance.eventDeck);
        yield return StartCoroutine(ShowManager.instance.PlayCardsShuffleIn(cardsInDeck.ToArray()));

        foreach (var section in serializedStory.sections) {
            yield return StartCoroutine(ExeCouncilStage(section));
            yield return StartCoroutine(ExeEventState(section));
   
            yield return null;
        }
    }

    IEnumerator ExeEventState(SerializedStory.Section section) {
        yield return StartCoroutine(StoryBook.instance.TurnPage(StoryBook.instance.eventPage));

        foreach (var eventCard in section.eventCards) {
            // binding
            var bindedCharacters = eventCard.preconditonSet.BindCharacters();

            // 演出event
            yield return StartCoroutine(ShowManager.instance.ShowEvent(eventCard, bindedCharacters));

            // TBD: 可能会出错，如果consequence不是确定的话，show在没apply的时候不知道确定值
            // apply consequence
            ConsequenceApplier.Apply(eventCard.consequenceSet, bindedCharacters);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator ExeCouncilStage(SerializedStory.Section section) {
        yield return StartCoroutine(StoryBook.instance.TurnPage(StoryBook.instance.councilPage));

        foreach (var councilInfo in section.councilStageInfos) {
            // show 角色卡动画
            var chracaterCardDisplay = ShowManager.instance.ShowCardFromDeck(
                councilInfo.characterCard,
                ShowManager.DeckTarget.characterDeck,
                AnchorManager.instance.characterCardAnchor);

            // 抽取决策卡，并逐个进行决策流程
            foreach (var straCard in councilInfo.stratagemCards) {
                // 生成交互
                bool decisionMade = false;
                bool agreeDecision = false;
                StratagemCardInteraction.Create(straCard, (bool agree) => { decisionMade = true; agreeDecision = agree; });

                // 等待玩家输入
                while (!decisionMade) {
                    yield return null;
                }

                // 应用后果
                var bindedCharacters = straCard.preconditonSet.BindCharacters();
                if (agreeDecision) {
                    // 如果采纳，则不进行后续的策略建议
                    ConsequenceApplier.Apply(straCard.consequenceSet, bindedCharacters);
                    break;
                }
            }

            ShowManager.instance.BackCardToDeck(chracaterCardDisplay, ShowManager.DeckTarget.characterDeck);
        }
    }
}
