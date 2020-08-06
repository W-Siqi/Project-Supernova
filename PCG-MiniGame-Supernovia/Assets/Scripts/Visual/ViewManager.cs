using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class ViewManager : MonoBehaviour {
        [System.Serializable]
        private class ResTable {
            public RawImage characterUIImage;
            public PositionTween characterUITween;
            public PositionTween diaglogUITween;
            public TextAnimator dialogTextAnimator;
            public AnchorPoint viewCardSpwanAnchor;
            public AnchorPoint viewCardLeftAnchor;
            public AnchorPoint viewCardRightAnchor;
            public AnchorPoint viewCardLeaveAnchor;
            public AnchorPoint centerBackCardAnchor;
            public VerticalLayoutGroup charcterStatusViewerLayout;
        }

        private static ViewManager _instance = null;
        public static ViewManager instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<ViewManager>();
                }
                return _instance;
            }
        }

        public ValueViewer armyValue;
        public ValueViewer moneyValue;
        public ValueViewer peopleValue;
        public VoteViewer voteViewer;
        public EventViewer eventViewer;

        [SerializeField]
        private ResTable resTable;
        private Dictionary<CharacterCard, CharacterStatusViewer> characterStatusViewerDict = new Dictionary<CharacterCard, CharacterStatusViewer>();
        public void Init() {
            foreach (var charcater in PlayData.instance.gameState.characterDeck) {
                var GO = Instantiate(ResourceTable.instance.prefabPage.characterStatusViewer);
                var viewer = GO.GetComponent<CharacterStatusViewer>();
                viewer.HookTo(charcater);
                viewer.transform.SetParent(resTable.charcterStatusViewerLayout.transform);
                characterStatusViewerDict[charcater] = viewer;
            }
        }

        public void HightLightCharacterTrait(CharacterCard characterCard, Trait trait) {
            characterStatusViewerDict[characterCard].HightlightTrait(trait);
        }

        public IEnumerator ViewReachNewStoryStageCoroutine(StoryStage storyStage) {
            yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent("营火 战斗 表决")));

            float intervalTime = 0.2f;
            float flyTime = 1.5f;
            // 按照campfire， fight， vote 顺序
            GameObject[] stagePrefabs = new GameObject[] {
                ResourceTable.instance.prefabPage.campfireStageCard,
                ResourceTable.instance.prefabPage.fightStageCard,
                ResourceTable.instance.prefabPage.voteStageCard};
            var cardGOs = new GameObject[3];
            // instantiate to screen
            for (int i = 0; i < 3; i++) {
                var spawnAnchor = resTable.viewCardSpwanAnchor.transform;
                var leftAnchor = resTable.viewCardLeftAnchor.transform;
                var rightAnchor = resTable.viewCardRightAnchor.transform;

                var GO = Instantiate(stagePrefabs[i], spawnAnchor.position, spawnAnchor.rotation);
                cardGOs[i] = GO;

                var t = (float)(i + 1) / 4f;
                var destPos = Vector3.Lerp(leftAnchor.position, rightAnchor.position, t);
                var destRotate = Quaternion.Lerp(leftAnchor.rotation, rightAnchor.rotation, t);

                LerpAnimator.instance.LerpPositionAndRotation(GO.transform, destPos, destRotate, flyTime);
                yield return new WaitForSeconds(intervalTime);
            }
            yield return new WaitForSeconds(2f);

            // turn back
            var centarBackAnchor = resTable.centerBackCardAnchor.transform;
            float turnBackTime = 1f;
            foreach (var cardGO in cardGOs) {
                var startRotation = cardGO.transform.rotation;
                LerpAnimator.instance.LerpValues(0, 1, turnBackTime,
                    (v) => {
                        cardGO.transform.rotation = Quaternion.Lerp(startRotation, centarBackAnchor.rotation, v);
                    });
            }
            yield return new WaitForSeconds(turnBackTime);

            // go to center Back Anchor
            float gotoCenterTime = 1f;
            foreach (var cardGO in cardGOs) {
                var startRotation = cardGO.transform.rotation;
                LerpAnimator.instance.LerpPositionAndRotation(cardGO.transform, centarBackAnchor.position, centarBackAnchor.rotation, gotoCenterTime);
            }
            yield return new WaitForSeconds(gotoCenterTime);

            // destroy 
            // 按照campfire， fight， vote 顺序的mask
            int dontDerstroyIndex = 0;
            switch (storyStage) {
                case StoryStage.campfire:
                    dontDerstroyIndex = 0;
                    break;
                case StoryStage.fight:
                    dontDerstroyIndex = 1;
                    break;
                case StoryStage.vote:
                    dontDerstroyIndex = 2;
                    break;
            }

            for (int i = 0; i < 3; i++) {
                if (i != dontDerstroyIndex) {
                    DestroyImmediate(cardGOs[i]);
                }
            }

            // turnFaceTo
            var faceToPlayer = Quaternion.Lerp(resTable.viewCardLeftAnchor.transform.rotation, resTable.viewCardRightAnchor.transform.rotation,0.5f);
            LerpAnimator.instance.LerpValues(0, 1, turnBackTime,
                (v)=> {
                    cardGOs[dontDerstroyIndex].transform.rotation = Quaternion.Lerp(resTable.centerBackCardAnchor.transform.rotation, faceToPlayer, v);
                });
            yield return new WaitForSeconds(turnBackTime);

            // leave
            yield return new WaitForSeconds(2.5f);
            var leaveAnchor = resTable.viewCardLeaveAnchor.transform;
            LerpAnimator.instance.LerpPositionAndRotation(cardGOs[dontDerstroyIndex].transform, leaveAnchor.position, leaveAnchor.rotation, 1f);
            yield return new WaitForSeconds(1);
        }

        public IEnumerator ViewCardsOnScreen(Card[] cards,float holdTime = 2f) {
            var cardDisplays = new List<CardDisplayBehaviour>();
            foreach (var card in cards) {
                cardDisplays.Add(CardDisplayBehaviour.Create(card, resTable.viewCardSpwanAnchor));
            }

            // show card
            float showupInterval = 0.2f;
            float showupAnimationDuration = 1.5f;
            for (int i = 0; i < cardDisplays.Count; i++) {
                var cardDisplay = cardDisplays[i];
                var t = (float)(i + 1) / (float)(cardDisplays.Count + 1);
                var destPos = Vector3.Lerp(resTable.viewCardRightAnchor.position, resTable.viewCardLeftAnchor.position, t);
                var destRotate = Quaternion.Lerp(resTable.viewCardRightAnchor.rotation, resTable.viewCardLeftAnchor.rotation, t);
                LerpAnimator.instance.LerpPositionAndRotation(cardDisplay.transform, destPos, destRotate, showupAnimationDuration);
                yield return new WaitForSeconds(showupInterval);
            }

            // stay 
            yield return new WaitForSeconds(holdTime + showupAnimationDuration - showupInterval);

            // card Leave
            foreach (var cardDisplay in cardDisplays) {
                LerpAnimator.instance.LerpPositionAndRotation(
                    cardDisplay.transform,
                    resTable.viewCardLeaveAnchor.position,
                    resTable.viewCardLeaveAnchor.rotation,
                    2f);
            }
            yield return new WaitForSeconds(2f);
        }

        public void ViewCharacterOfDialog(Texture characterImage) {
            resTable.characterUIImage.texture = characterImage;
            resTable.characterUITween.Play();
        }

        public void EndViewCharacterOfDialog() {
            resTable.characterUITween.ResetToStart();
        }

        public void ViewDialog(string content) {
            resTable.diaglogUITween.Play();
            resTable.dialogTextAnimator.Play(content);
        }

        public void EndViewDialog() {
            resTable.diaglogUITween.ResetToStart();
        }
    }
}
