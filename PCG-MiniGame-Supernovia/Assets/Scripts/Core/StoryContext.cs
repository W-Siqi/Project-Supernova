//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PCG;

//// 用于保存一切故事运作信息的单例
//public class StoryContext : MonoBehaviour {
//    static StoryContext _instance = null;

//    public GameState gameState = new GameState();

//    public static StoryContext instance {
//        get {
//            if (_instance == null) {
//                var GO = new GameObject("Story Context");
//                _instance = GO.AddComponent<StoryContext>();
//            }
//            return _instance;
//        }
//    }


//    public void InitForNewStory(int randomSeed) {
//        var varTable = PCGVariableTable.instance;
//        // init character
//        for (int i = 0; i < varTable.characterCount; i++) {
//            var charaPrototype = DeckArchive.instance.characterCards[i];
//            var newCharacter = Card.DeepCopy(charaPrototype);
//            // random properties
//            newCharacter.loyalty = Random.Range(3, 7);
//            foreach (var p in newCharacter.personalities) {
//                p.trait = TraitUtils.GetRandomTrait();
//            }

//            gameState.characterDeck.Add(newCharacter);
//        }

//        // init startgems of character
//        foreach (var chara in gameState.characterDeck) {
//            gameState.stratagemDict[chara] = new List<StratagemCard>();
//            for (int i = 0; i < varTable.roundCount; i++) {
//                var stratagemPrototype = DeckArchive.instance.stratagemCards[Random.Range(0, DeckArchive.instance.stratagemCards.Count)];
//                gameState.stratagemDict[chara].Add(Card.DeepCopy(stratagemPrototype));
//            }
//        }

//        // init event
//        gameState.eventDeck = new List<EventCard>();
//        foreach (var card in DeckArchive.instance.eventCards) {
//            gameState.eventDeck.Add(Card.DeepCopy(card));
//        }

//        // init status
//        gameState.statusVector.army = Random.Range(20, 100);
//        gameState.statusVector.money = Random.Range(20, 100);
//        gameState.statusVector.people = Random.Range(20, 100);
//    }
//}
