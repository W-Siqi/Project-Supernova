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
            gameConfig.characterCount = Random.Range(quantifyValueTable.characterCount.from, quantifyValueTable.characterCount.to + 1);
            gameConfig.roundCount = Random.Range(quantifyValueTable.winRound.from, quantifyValueTable.winRound.to + 1);

            // init character
            gameState.characterDeck.Clear();
            for (int i = 0; i < gameConfig.characterCount; i++) {
                gameState.characterDeck.Add(BuildRandomCharacter());
            }

            // copy stratagem pool
            gameState.stratagemDeck.Clear();
            foreach (var stragemCard in ResourcePool.instance.stratagemCards) {
                gameState.stratagemDeck.Add(Card.DeepCopy(stragemCard));
            }

            // copy event pool
            gameState.eventDeck.Clear();
            foreach (var card in ResourcePool.instance.eventCards) {
                gameState.eventDeck.Add(Card.DeepCopy(card));
            }

            // init startgems of character
            gameState.stratagemIndexDict.Clear();
            foreach (var chara in gameState.characterDeck) {
                gameState.stratagemIndexDict[chara] = new List<int>();
                for (int i = 0; i < quantifyValueTable.winRound.to; i++) {
                    gameState.stratagemIndexDict[chara].Add(Random.Range(0, gameState.stratagemDeck.Count));
                }
            }

            // init status
            gameState.statusVector.army = Random.Range(20, 80);
            gameState.statusVector.money = Random.Range(20, 80);
            gameState.statusVector.people = Random.Range(20, 80);
        }

        public void ToRandom(QuantifyValueTable quantifyValueTable, float estimatedDifficulty) {
            gameConfig.characterCount = (int)Mathf.Lerp(quantifyValueTable.characterCount.from, quantifyValueTable.characterCount.to + 1, estimatedDifficulty);
            gameConfig.roundCount = Random.Range(quantifyValueTable.winRound.from, quantifyValueTable.winRound.to + 1);

            // init character
            gameState.characterDeck.Clear();
            for (int i = 0; i < gameConfig.characterCount; i++) {
                gameState.characterDeck.Add(BuildRandomCharacter());
            }

            // copy stratagem pool
            gameState.stratagemDeck.Clear();
            foreach (var stragemCard in ResourcePool.instance.stratagemCards) {
                gameState.stratagemDeck.Add(Card.DeepCopy(stragemCard));
            }

            // copy event pool
            gameState.eventDeck.Clear();
            foreach (var card in ResourcePool.instance.eventCards) {
                gameState.eventDeck.Add(Card.DeepCopy(card));
            }

            // init startgems of character
            gameState.stratagemIndexDict.Clear();
            foreach (var chara in gameState.characterDeck) {
                gameState.stratagemIndexDict[chara] = new List<int>();
                for (int i = 0; i < quantifyValueTable.winRound.to; i++) {
                    gameState.stratagemIndexDict[chara].Add(Random.Range(0, gameState.stratagemDeck.Count));
                }
            }

            // init status
            gameState.statusVector.army = Random.Range(20, 80);
            gameState.statusVector.money = Random.Range(20, 80);
            gameState.statusVector.people = Random.Range(20, 80);

            Debug.Log("人数： " + gameState.characterDeck.Count + "  拿督" + estimatedDifficulty);
        }

        private CharacterCard BuildRandomCharacter() {
            var newCharacter = new CharacterCard();
            // random properties
            newCharacter.loyalty = Random.Range(2, 7);
            var existedTrait = new List<Trait>();
            foreach (var p in newCharacter.personalities) {
                p.trait = TraitUtils.GetRandomTrait(existedTrait.ToArray());
                existedTrait.Add(p.trait);
            }

            return newCharacter;
        }


        // 这个核心函数...
        const float STATUS_VALUE_THRESHOLD = 0.01f;
        const float STATUS_VALUE_PROB = 0.8f;

        const float PERSONALITY_THRESHOLD = 0.03f;
        const float PERSONALITY_PROB = 0.3f;

        const float LOYALTY_THRESHOLD = 0.04f;
        const float LOYALTY_PROB = 0.3f;

        const float ROUND_THRESHOLD = 0.10f;
        const float ROUND_PROB = 0.1f;

        public void AdjustDifficulty(QuantifyValueTable quantifyValueTable, float targetDifficulty) {
            var absDelta = Mathf.Abs(difficulty - targetDifficulty);
            if (difficulty < targetDifficulty) {
                Debug.Log("增加难度 [abs delta] " + absDelta);
                // 增加难度
                // 门槛0 - 调整初始值
                if (absDelta > STATUS_VALUE_THRESHOLD && Random.value < STATUS_VALUE_PROB) {
                    if (gameState.statusVector.army > 20) {
                        gameState.statusVector.army -= Random.Range(0, 3);
                    }
                    if (gameState.statusVector.money > 20) {
                        gameState.statusVector.money -= Random.Range(0, 3);
                    }
                    if (gameState.statusVector.people > 20) {
                        gameState.statusVector.people -= Random.Range(0, 3);
                    }
                }

                // 门槛1 - 调整人格分配
                if (absDelta > PERSONALITY_THRESHOLD && Random.value < PERSONALITY_PROB) {
                    var randChar = gameState.characterDeck[Random.Range(0, gameState.characterDeck.Count)];
                    AdjustPersonality(randChar, quantifyValueTable, true);
                }

                // 门槛2 - 调整忠诚度
                if (absDelta > LOYALTY_THRESHOLD && Random.value < LOYALTY_PROB) {
                    var randChar = gameState.characterDeck[Random.Range(0, gameState.characterDeck.Count)];
                    if (randChar.loyalty > quantifyValueTable.loyalty.from) {
                        randChar.loyalty--;
                    }
                }

                // 门槛3- 调整回合
                if (absDelta > ROUND_THRESHOLD && Random.value < ROUND_PROB) {
                    if (gameConfig.roundCount < quantifyValueTable.winRound.to) {
                        gameConfig.roundCount++;
                    }
                }
            }
            else {
                Debug.Log("降低难度 [abs delta] " + absDelta);
                // 减小难度
                // 门槛0 - 调整初始值
                if (absDelta > STATUS_VALUE_THRESHOLD && Random.value < STATUS_VALUE_PROB) {
                    if (gameState.statusVector.army < 90) {
                        gameState.statusVector.army += Random.Range(0, 3);
                    }
                    if (gameState.statusVector.money < 90) {
                        gameState.statusVector.money += Random.Range(0, 3);
                    }
                    if (gameState.statusVector.people < 90) {
                        gameState.statusVector.people += Random.Range(0, 3);
                    }
                }

                // 门槛1 - 调整人格分配
                if (absDelta > PERSONALITY_THRESHOLD && Random.value < PERSONALITY_PROB) {
                    var randChar = gameState.characterDeck[Random.Range(0, gameState.characterDeck.Count)];
                    AdjustPersonality(randChar, quantifyValueTable, false);
                }

                // 门槛2 - 调整忠诚度
                if (absDelta > LOYALTY_THRESHOLD && absDelta > LOYALTY_THRESHOLD && Random.value < LOYALTY_PROB) {
                    var randChar = gameState.characterDeck[Random.Range(0, gameState.characterDeck.Count)];
                    if (randChar.loyalty < quantifyValueTable.loyalty.to) {
                        randChar.loyalty++;
                    }
                }

                // 门槛3- 调整回合
                if (absDelta > ROUND_THRESHOLD && Random.value < ROUND_PROB) {
                    if (gameConfig.roundCount > quantifyValueTable.winRound.from) {
                        gameConfig.roundCount--;
                    }
                }
            }
        }

        private void AdjustPersonality(CharacterCard characterCard, QuantifyValueTable quantifyValueTable, bool addDifficulty) {
            int selectedIndex = Random.Range(0, CharacterCard.PERSONALITY_COUNT);
            var curTrait = characterCard.personalities[selectedIndex].trait;
            characterCard.personalities[selectedIndex].trait = quantifyValueTable.GetRandomTraitUsingQuanficiton(curTrait, addDifficulty);
        }
    }
}