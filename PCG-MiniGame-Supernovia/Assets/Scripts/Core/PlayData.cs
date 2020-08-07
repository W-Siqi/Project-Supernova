using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    // 保存真正用来游玩的gamestate 和gameconfig
    public class PlayData : MonoBehaviour {
        static PlayData _instance = null;
        public static PlayData instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<PlayData>();
                }
                return _instance;
            }
        }


        public GameState gameState = new GameState();
        public GameConfig gameConfig = new GameConfig();

        /// <summary>
        /// 生成残局
        /// </summary>
        public void InitData() {
            // init character
            gameState.characterDeck.Clear();
            for (int i = 0; i < gameConfig.characterCount; i++) {
                var charaPrototype = DeckArchive.instance.characterCards[i];
                var newCharacter = Card.DeepCopy(charaPrototype);
                // random properties
                newCharacter.loyalty = Random.Range(3, 7);
                foreach (var p in newCharacter.personalities) {
                    p.trait = TraitUtils.GetRandomTrait();
                }

                gameState.characterDeck.Add(newCharacter);
            }

            // init startgems of character
            gameState.stratagemDict.Clear();
            foreach (var chara in gameState.characterDeck) {
                gameState.stratagemDict[chara] = new List<StratagemCard>();
                for (int i = 0; i < gameConfig.roundCount; i++) {
                    var stratagemPrototype = DeckArchive.instance.stratagemCards[Random.Range(0, DeckArchive.instance.stratagemCards.Count)];
                    gameState.stratagemDict[chara].Add(Card.DeepCopy(stratagemPrototype));
                }
            }

            // init event
            gameState.eventDeck.Clear();
            gameState.eventDeck = new List<EventCard>();
            foreach (var card in DeckArchive.instance.eventCards) {
                gameState.eventDeck.Add(Card.DeepCopy(card));
            }

            // init status
            gameState.statusVector.army = Random.Range(20, 100);
            gameState.statusVector.money = Random.Range(20, 100);
            gameState.statusVector.people = Random.Range(20, 100);
        }
    }
}

