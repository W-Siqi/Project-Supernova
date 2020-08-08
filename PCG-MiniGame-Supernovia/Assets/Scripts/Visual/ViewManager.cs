using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class ViewManager : MonoBehaviour {
        [System.Serializable]
        public class ResTable {
            public PositionTween statusPannelShowup;
            public PositionTween characterUITween;
            public PositionTween nextRoundBtnShowup;
            public PositionTween diaglogUITween;
            public PositionTween endGameUIShowup;

            public TextMeshProUGUI stratagemDialogCharacterName;
            public TextMeshProUGUI stratagemDialogTitleName;
            public TextAnimator startagemDialogTextAnimator;

            public RawImage characterUIImage;
            public GameObject startGameMenuRoot;

            public AnchorPoint viewCardSpwanAnchor;
            public AnchorPoint viewCardLeftAnchor;
            public AnchorPoint viewCardRightAnchor;
            public AnchorPoint viewCardLeaveAnchor;
            public AnchorPoint centerBackCardAnchor;

            public Color evilTraitColor;
            public Color noneEvilTraitColor;
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

        public ResTable resTable;
        public ValueViewer armyValue;
        public ValueViewer moneyValue;
        public ValueViewer peopleValue;
        public VoteViewer voteViewer;
        public EventDescriptionPlayer eventDescriptionPlayer;
        public CharacterStausPannel characterStausPannel;

        public void OnStortEnd() {
            StoryBook.instance.ViewContent(new StoryBook.PageContent(ResourceTable.instance.texturepage.eventSceneTex));
            resTable.endGameUIShowup.Play();
            resTable.statusPannelShowup.Play(true);
            resTable.characterUITween.Play(true);
            resTable.diaglogUITween.Play(true);
            characterStausPannel.Hide();
            eventDescriptionPlayer.HideUI();
        }


        public void InitForGameStart() {
            resTable.startGameMenuRoot.SetActive(false);
            resTable.statusPannelShowup.Play();
            characterStausPannel.Init(PlayData.instance.gameState.characterDeck.ToArray());
        }

        public void InitViewForCouncialStage() {
            characterStausPannel.Showup();
            eventDescriptionPlayer.HideUI();
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


        public void ViewCharacterOfDialog(CharacterCard character) {
            resTable.characterUIImage.texture = character.GetAvatarImage();
            resTable.characterUITween.Play();
        }

        public void EndViewCharacterOfDialog() {
            resTable.characterUITween.ResetToStart();
        }


        public void ViewDialog(StratagemCard stratagemCard, CharacterCard stragemProvider) {
            resTable.diaglogUITween.Play();
            resTable.startagemDialogTextAnimator.Play(stratagemCard.description);
            resTable.stratagemDialogTitleName.text = stratagemCard.name;
            resTable.stratagemDialogCharacterName.text = stragemProvider.name;
        }

        public void EndViewDialog() {
            resTable.diaglogUITween.ResetToStart();
        }

        public void ViewNextRoundBtn() {
            resTable.nextRoundBtnShowup.Play();
        }

        public void EndNextRoundBtn() {
            resTable.nextRoundBtnShowup.Play(true);
        }
    }
}
