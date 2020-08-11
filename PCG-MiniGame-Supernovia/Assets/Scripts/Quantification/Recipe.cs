using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class Recipe {
        public float difficulty = -1;
        public GameState gameState = new GameState();
        public GameConfig gameConfig = new GameConfig();

        public Recipe() {

        }

        public Recipe(Recipe src) {
            this.difficulty = src.difficulty;
            this.gameState = src.gameState.MakeDeepCopy();
            this.gameConfig = src.gameConfig.MakeDeepCopy();
        }

        /// <summary>
        /// 保证每个character 恰好只有1个trait
        /// </summary>
        /// <param name="trait"></param>
        public void ToDirectionalRandom(QuantifyValueTable quantifyValueTable,Trait trait) {
            ToRandom(quantifyValueTable);
            foreach (var character in gameState.characterDeck) {
                character.personalities[0].trait = trait;
                character.personalities[1].trait = TraitUtils.GetRandomTrait(trait);
                character.personalities[2].trait = TraitUtils.GetRandomTrait(trait);
            }
        }

        public void ToDirectionalRandom(QuantifyValueTable quantifyValueTable, DiscreteQuantifyValue targetValue, int givenValue) {
        }


        public void ToRandom(QuantifyValueTable quantifyValueTable){
            gameConfig.characterCount = (int)Mathf.Lerp(3, 6, difficulty);
            gameConfig.roundCount = (int)Mathf.Lerp(2, 7, difficulty);
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

        public void AdjustDifficulty(QuantifyValueTable quantifyValueTable, float targetDifficult) {
            ToRandom(quantifyValueTable);
        }
    }
}