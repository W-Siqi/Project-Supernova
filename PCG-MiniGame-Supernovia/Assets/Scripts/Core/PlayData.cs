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
        public IEnumerator InitData(float difficulty) {
            var recipePCG = FindObjectOfType<RecipePCG>();
            yield return StartCoroutine(recipePCG.GenerateRecipe(difficulty));
            var recipe = recipePCG.GetCurrentRecipeCopy();
            gameState = recipe.gameState;
            gameConfig = recipe.gameConfig;
        }

        public void InitDataWithoutPCG() {
            // init character
            gameState.characterDeck.Clear();
            for (int i = 0; i < gameConfig.characterCount; i++) {
                var charaPrototype = DeckArchive.instance.characterCards[i];
                var newCharacter = Card.DeepCopy(charaPrototype);
                // random properties
                newCharacter.loyalty = Random.Range(2, 7);
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
                    gameState.stratagemIndexDict[chara].Add(Random.Range(0, gameState.stratagemDeck.Count));
                }
            }

            // init status
            gameState.statusVector.army = Random.Range(20, 80);
            gameState.statusVector.money = Random.Range(20, 80);
            gameState.statusVector.people = Random.Range(20, 80);
        }
    }
}

