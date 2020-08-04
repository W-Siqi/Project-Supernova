using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class FightViewer : MonoBehaviour {
        public static FightViewer instance = null;

        private Dictionary<CharacterCard, CardDisplayBehaviour> cardDisplayDict = new Dictionary<CharacterCard, CardDisplayBehaviour>();

        private void Awake() {
            instance = this;
        }

        public IEnumerator InitFight(CharacterCard[] group1, CharacterCard[] group2) {
            float intervalTime = 0.2f;
            float flyTime = 1.5f;
            // init group1
            var spawnAnchor1 = AnchorManager.instance.fightLeftSpawn.transform;
            var bottomAnchor1 = AnchorManager.instance.fightLeftBottom.transform;
            var upAnchor1 = AnchorManager.instance.fightLeftUp.transform;
            for (int i = 0; i < group1.Length; i++) {
                var cardDisplay = CardDisplayBehaviour.Create(group1[i], spawnAnchor1.position, spawnAnchor1.rotation);
                cardDisplayDict[group1[i]] = cardDisplay;

                var t = (float)(i + 1) / (float)(group1.Length + 1);
                var destPos = Vector3.Lerp(bottomAnchor1.position, upAnchor1.position, t);
                var destRotate = Quaternion.Lerp(bottomAnchor1.rotation, upAnchor1.rotation, t);

                LerpAnimator.instance.LerpPositionAndRotation(cardDisplay.transform, destPos, destRotate, flyTime);
                yield return new WaitForSeconds(intervalTime);
            }

            // init group2
            var spawnAnchor2 = AnchorManager.instance.fightRightSpawn.transform;
            var bottomAnchor2 = AnchorManager.instance.fightRightBottom.transform;
            var upAnchor2 = AnchorManager.instance.fightRightUp.transform;
            for (int i = 0; i < group2.Length; i++) {
                var cardDisplay = CardDisplayBehaviour.Create(group2[i], spawnAnchor2.position, spawnAnchor2.rotation);
                cardDisplayDict[group2[i]] = cardDisplay;

                var t = (float)(i + 1) / (float)(group2.Length + 1);
                var destPos = Vector3.Lerp(bottomAnchor2.position, upAnchor2.position, t);
                var destRotate = Quaternion.Lerp(bottomAnchor2.rotation, upAnchor2.rotation, t);

                LerpAnimator.instance.LerpPositionAndRotation(cardDisplay.transform, destPos, destRotate, flyTime);
                yield return new WaitForSeconds(intervalTime);
            }
        }

        public IEnumerator ViewAttackEvent(CharacterCard attacker, CharacterCard defender) {
            // config
            var attackDistance = 0.2f;
            var attackerMoveTime = 0.3f;
            
            var attackerDisplay = cardDisplayDict[attacker];
            var defenderDisplay = cardDisplayDict[defender];

            // move to attack pos
            var defenderToAttacker = attackerDisplay.transform.position - defenderDisplay.transform.position;
            var attackPos = defenderDisplay.transform.position + attackDistance * defenderToAttacker.normalized;
            var attackerPrePos = attackerDisplay.transform.position;
            var attackerPreRotate = attackerDisplay.transform.rotation;
            LerpAnimator.instance.LerpPositionAndRotation(attackerDisplay.transform, attackPos, attackerPreRotate,attackerMoveTime);
            yield return new WaitForSeconds(attackerMoveTime);

            // play effect
            var attackGO =Instantiate(ResourceTable.instance.prefabPage.attackEffect, attackPos, Quaternion.identity);
            Destroy(attackGO, 3f);
            HitEffect.Create(attacker.attributes.atkVal, attackPos);
            //defenderDisplay.UpdateHPValue(defender.attributes.HP);
            yield return new WaitForSeconds(1f);

            // attacker move back
            LerpAnimator.instance.LerpPositionAndRotation(attackerDisplay.transform, attackerPrePos, attackerPreRotate, attackerMoveTime);
            //yield return new WaitForSeconds(attackerMoveTime);
        }

        public IEnumerator ViewDeathEvent(CharacterCard deadCard) {
            Debug.Log(deadCard.name + " dead");
            yield return null;
        }
    }

}
