using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class FightManager : MonoBehaviour {
        public static FightManager instance = null;

        public Texture2D enemyImageForTest;

        private void Awake() {
            instance = this;
        }

        public CharacterCard[] InstanticteRandomEnemies() {
            int count = 6;
            var enemies = new List<CharacterCard>();
            for (int i = 0; i < count; i++) {
                var enemy = new CharacterCard();
                enemy.attributes.atkVal = Random.Range(1, 6);
                enemy.attributes.maxHP = Random.Range(5, 20);
                enemy.attributes.HP = enemy.attributes.maxHP;
                enemy.name = "测试怪";
                enemy.SetAvatarImage(enemyImageForTest);
                enemies.Add(enemy);
            }
            return enemies.ToArray();
        }

        public IEnumerator ExecuteFight(CharacterCard[] allies, CharacterCard[] enemies) {
            List<CharacterCard> aliveAllies = new List<CharacterCard>(allies);
            // aliveAllies.Add(playerCard);
            List<CharacterCard> aliveEnemies = new List<CharacterCard>(enemies);

            // init viewer
            var viewer = FightViewer.instance;
            yield return StartCoroutine(viewer.InitFight(aliveAllies.ToArray(), aliveEnemies.ToArray()));

            while (aliveAllies.Count > 0 && aliveEnemies.Count > 0) {
                // interacton, fight or run

                // one round
                var attakedBefore = new HashSet<CharacterCard>();
                while (aliveAllies.Count > 0 && aliveEnemies.Count > 0) {
                    // ally attack
                    var allyRoundRemain = false;
                    if (aliveAllies.Count > 0 && aliveEnemies.Count > 0) {
                        var attackerAlly = aliveAllies[0];
                        aliveAllies.RemoveAt(0);
                        aliveAllies.Add(attackerAlly);
                        attakedBefore.Add(attackerAlly);

                        var defenderEnemy = aliveEnemies[Random.Range(0, aliveEnemies.Count)];

                        // fight happen
                        yield return StartCoroutine(ExecuteAttack(attackerAlly, defenderEnemy));
                        if (defenderEnemy.attributes.HP <0) {
                            aliveEnemies.Remove(defenderEnemy);
                        }

                        if (!attakedBefore.Contains(aliveAllies[0])) {
                            allyRoundRemain = true;
                        }
                    }

                    // enemy attack
                    var enemyRoundRemain = false;
                    if (aliveAllies.Count > 0 && aliveEnemies.Count > 0) {
                        var attackerEnemy = aliveEnemies[0];
                        aliveEnemies.RemoveAt(0);
                        aliveEnemies.Add(attackerEnemy);
                        attakedBefore.Add(attackerEnemy);

                        var defenderAlly = aliveAllies[Random.Range(0, aliveAllies.Count)];

                        // fight happen
                        bool allyDead;
                        yield return StartCoroutine(ExecuteAttack(attackerEnemy, defenderAlly));
                        if (defenderAlly.attributes.HP < 0) {
                            aliveEnemies.Remove(defenderAlly);
                        }

                        if (!attakedBefore.Contains(aliveEnemies[0])) {
                            enemyRoundRemain = true;
                        }
                    }

                    // round end
                    if (!allyRoundRemain && !enemyRoundRemain) {
                        break;
                    }
                }
            }
        }

        private IEnumerator ExecuteAttack(CharacterCard attacker, CharacterCard defender) {
            defender.attributes.HP -= attacker.attributes.atkVal;

            // wait for animation and visual
            yield return StartCoroutine(FightViewer.instance.ViewAttackEvent(attacker, defender));
            if (defender.attributes.HP < 0) {
                // death visual
                yield return StartCoroutine(FightViewer.instance.ViewDeathEvent(defender));
            }
        }
    }
}