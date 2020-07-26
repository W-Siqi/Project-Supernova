using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayLoopManager playLoopManager;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        StartGame();
    }

    // [ContextMenu("start game")]
    public void StartGame() {
        StartCoroutine(StartGameProcedule());
    }

    IEnumerator StartGameProcedule() {
        // 初始化故事状态
        int seed = Random.Range(-10000, 100000);
        StoryContext.instance.InitForNewStory(seed);

        // 洗牌动画
        var cardsInDeck = new List<Card>();
        cardsInDeck.AddRange(StoryContext.instance.characterDeck);
        cardsInDeck.AddRange(StoryContext.instance.stratagemDeck);
        cardsInDeck.AddRange(StoryContext.instance.eventDeck);
        yield return StartCoroutine(ShowManager.instance.PlayCardsShuffleIn(cardsInDeck.ToArray()));

        // 开始游戏主循环
        playLoopManager.StartPlayLoop();
    }
}
