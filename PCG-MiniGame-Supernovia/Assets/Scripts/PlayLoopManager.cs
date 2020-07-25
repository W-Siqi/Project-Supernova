using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理 决策 - 人物 - 事件 等游戏游玩主循环
public class PlayLoopManager : MonoBehaviour {
    // 每个character 有对应的专属filer
    private Dictionary<CharacterCard, StratagemProbFilter> straProbFilters = new Dictionary<CharacterCard, StratagemProbFilter>();
    private Dictionary<CharacterCard, StratagemCountFilter> straCountFilers = new Dictionary<CharacterCard, StratagemCountFilter>();

    public void StartPlayLoop() {
        StartCoroutine(PlayLoopCoroutine());
    }

    IEnumerator PlayLoopCoroutine(){
        while (true) {
            yield return StartCoroutine(CouncilStage());
            yield return StartCoroutine(EventState());
            yield return null;
        }
    }

    IEnumerator EventState() {
        var selecedEvents = CardSelector.Select(new EventProbFilter(), new EventCountFilter());
        foreach (var c in selecedEvents) {
            var eventCard = c as EventCard;

            // binding
            var bindedCharacters = eventCard.preconditonSet.BindCharacters();

            // 演出event
            yield return StartCoroutine(ShowManager.instance.ShowEvent(eventCard,bindedCharacters));

            // TBD: 可能会出错，如果consequence不是确定的话，show在没apply的时候不知道确定值
            // apply consequence
            ConsequenceApplier.Apply(eventCard.consequenceSet, bindedCharacters);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CouncilStage() {
        foreach (var character in StoryContext.instance.characterDeck) {
            if (!straProbFilters.ContainsKey(character)) {
                straProbFilters[character] = new StratagemProbFilter(character);
            }
            if (!straCountFilers.ContainsKey(character)) {
                straCountFilers[character] = new StratagemCountFilter(character);
            }

            // show 角色卡动画
            var chracaterCardDisplay = ShowManager.instance.ShowCardFromDeck(
                character,
                ShowManager.DeckTarget.characterDeck,
                AnchorManager.instance.characterCardAnchor);

            // 抽取决策卡，并逐个进行决策流程
            var stratagesOfCharacter = CardSelector.Select(straProbFilters[character],straCountFilers[character]);
            foreach (var s  in stratagesOfCharacter) {
                var straCard = s as StratagemCard;
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
                    ConsequenceApplier.Apply(straCard.consequenceSet,bindedCharacters);
                    break;
                }
            }

            ShowManager.instance.BackCardToDeck(chracaterCardDisplay, ShowManager.DeckTarget.characterDeck);
        }
    }
}
