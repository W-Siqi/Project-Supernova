﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

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
            yield return StartCoroutine(FightStage());
            yield return StartCoroutine(CouncilStage());
            yield return StartCoroutine(EventState());
            yield return StartCoroutine(SubstoryCardCheckPoint());
            yield return null;
        }
    }


    // TBD - 当前算法是必然会出一个副本卡
    IEnumerator SubstoryCardCheckPoint() {
        var subStories = DeckArchive.instance.substoryCards;
        if (subStories.Count > 0) {
            var selectedSubstory = subStories[Random.Range(0, subStories.Count)];
            // 动画演出
            yield return StartCoroutine(ShowManager.instance.SubstoryArriveShow(selectedSubstory));
            if (selectedSubstory.type == SubstoryCard.Type.dungeon) {
                // 副本卡
                StoryContext.instance.PushSubstory(selectedSubstory);
            }
            else {
                //支线卡
                var newCards = new List<Card>();
                newCards.AddRange(selectedSubstory.newCharacters);
                newCards.AddRange(selectedSubstory.eventCards);
                newCards.AddRange(selectedSubstory.stratagemCards);

                StoryContext.instance.characterDeck.AddRange(selectedSubstory.newCharacters);
                StoryContext.instance.eventDeck.AddRange(selectedSubstory.eventCards);
                StoryContext.instance.stratagemDeck.AddRange(selectedSubstory.stratagemCards);
            }
        }
    }

    

    IEnumerator EventState() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneRT);
        yield return StartCoroutine(StoryBook.instance.TurnPage(newPageContent));

        var selecedEvents = CardSelector.Select(new EventProbFilter(), new EventCountFilter());
        foreach (var c in selecedEvents) {
            var eventCard = c as EventCard;

            // binding
            var bindingInfos = eventCard.preconditonSet.Bind();

            // 演出event
            var bindedCharacters = new CharacterCard[bindingInfos.Length];
            for (int i = 0; i < bindingInfos.Length; i++) {
                bindedCharacters[i] = bindingInfos[i].bindedCharacter;
            }
            yield return StartCoroutine(ShowManager.instance.ShowEvent(eventCard,bindedCharacters));

            // TBD: 可能会出错，如果consequence不是确定的话，show在没apply的时候不知道确定值
            // apply consequence
            ConsequenceApplier.Apply(eventCard.consequenceSet, bindingInfos);
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator CouncilStage() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.councilSceneRT);
        yield return StartCoroutine(StoryBook.instance.TurnPage(newPageContent));

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
                var bindingInfos = straCard.preconditonSet.Bind();
                if (agreeDecision) {
                    // 如果采纳，则不进行后续的策略建议
                    ConsequenceApplier.Apply(straCard.consequenceSet,bindingInfos);
                    break;
                }
            }

            ShowManager.instance.BackCardToDeck(chracaterCardDisplay, ShowManager.DeckTarget.characterDeck);
        }
    }

    IEnumerator FightStage() {
        var newPageContent = new StoryBook.PageContent(ResourceTable.instance.texturepage.fightSceneRT);
        yield return StartCoroutine(StoryBook.instance.TurnPage(newPageContent));

        var enemies = FightManager.instance.InstanticteRandomEnemies();
        yield return StartCoroutine(FightManager.instance.ExecuteFight(StoryContext.instance.characterDeck.ToArray(),enemies));
    }
}
