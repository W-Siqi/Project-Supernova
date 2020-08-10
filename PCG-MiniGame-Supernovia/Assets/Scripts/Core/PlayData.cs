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
        public void InitData(float difficulty = 0.5f) {
            gameConfig.characterCount = (int)Mathf.Lerp(3, 6, difficulty);
            gameConfig.roundCount = (int)Mathf.Lerp(2, 7, difficulty);
            gameConfig.minInitLoyalty = (int)Mathf.Lerp(4, 2, difficulty); 
            gameConfig.maxInitLoyalty = gameConfig.minInitLoyalty+4;
        
            // init character
            gameState.characterDeck.Clear();
            for (int i = 0; i < gameConfig.characterCount; i++) {
                var charaPrototype = DeckArchive.instance.characterCards[i];
                var newCharacter = Card.DeepCopy(charaPrototype);
                // random properties
                newCharacter.loyalty = Random.Range(gameConfig.minInitLoyalty, gameConfig.maxInitLoyalty);
                foreach (var p in newCharacter.personalities) {
                    p.trait = TraitUtils.GetRandomTrait();
                }

                gameState.characterDeck.Add(newCharacter);
            }

            // copy stratagem pool
            gameState.stratagemDeck.Clear();
            foreach (var stragemCard in DeckArchive.instance.stratagemCards) {
                gameState.stratagemDeck.Add(Card.DeepCopy(stragemCard));
            }

            // copy event pool
            gameState.eventDeck.Clear();
            foreach (var card in DeckArchive.instance.eventCards) {
                gameState.eventDeck.Add(Card.DeepCopy(card));
            }

            // init startgems of character
            gameState.stratagemIndexDict.Clear();
            foreach (var chara in gameState.characterDeck) {
                gameState.stratagemIndexDict[chara] = new List<int>();
                for (int i = 0; i < gameConfig.roundCount; i++) {
                    gameState.stratagemIndexDict[chara].Add(Random.Range(0,gameState.stratagemDeck.Count));
                }
            }

            // init status
            gameState.statusVector.army = Random.Range(20, 80);
            gameState.statusVector.money = Random.Range(20, 80);
            gameState.statusVector.people = Random.Range(20, 80);
        }
    }
}

