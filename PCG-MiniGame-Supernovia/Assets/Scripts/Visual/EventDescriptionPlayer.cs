﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PCG {
    public class EventDescriptionPlayer : MonoBehaviour {
        [SerializeField]
        private Text eventTtileText;
        [SerializeField]
        private PositionTween eventTitleShowup;
        [SerializeField]
        private Text eventDescriptionText;
        [SerializeField]
        private PositionTween eventDescriptionShowup;
        [SerializeField]
        private Transform showCharacterFirstAnchor;
        [SerializeField]
        private Transform showCharacterSecondAnchor;
        [SerializeField]
        private Transform showCharacterSpawnAnchor;

        private Dictionary<CharacterCard, CharacterCardViewer> viewerDict = new Dictionary<CharacterCard, CharacterCardViewer>();

        public void HideUI() {
            eventDescriptionShowup.Play(true);
            eventTitleShowup.Play(true);
        }

        public IEnumerator PlayEventDescription(GameState gameState, EventDescription eventDescription) {
            float charInterval = 0.05f;
            float paragraphInterval = 1f;
            float holdTime = 1f;
            eventDescriptionShowup.Play();
            eventTitleShowup.Play();
            eventTtileText.text = eventDescription.title;
            eventDescriptionText.text = "";
            foreach (var para in eventDescription.paragragh) {
                Debug.Log("[paragrah]- " + para);
                int cur = 0;
                while (cur < para.Length) {
                    if (para[cur] == EventDescription.ANI_START_SIGN) {
                        // 动画字符
                        var end = cur;
                        while (end < para.Length && para[end] != EventDescription.ANI_END_SIGN) {
                            end++;
                        }
                        yield return StartCoroutine( PlayAniamtionContent(gameState, para.Substring(cur+1, end - cur)));
                        cur = end + 1;
                    }
                    else {
                        // 普通字符
                        eventDescriptionText.text += para[cur];
                        yield return new WaitForSeconds(charInterval);
                        cur++;
                    }
                }
                eventDescriptionText.text += '\n';
                yield return new WaitForSeconds(paragraphInterval);
            }

            yield return new WaitForSeconds(holdTime);
            foreach (var viewer in viewerDict) {
                DestroyImmediate(viewer.Value.gameObject);
            }
            viewerDict.Clear();
            Debug.Log("[结束]------------- " );
        }

        // 传进来的是不带()的
        // 动画(c[characterIndex]) ,c代表character Showup
        // 动画(h[characterIndex][trait-转ASCII]),h是hightlight
        // 动画(r[characterIndex][trait-转ASCII]),r是remove trait
        // 动画(a[characterIndex][trait-转ASCII]),a是add trait
        public const char TRAIT_ADD_ANIM_SIGN = 'a';
        private IEnumerator PlayAniamtionContent(GameState gameState, string animationIndecator) {
            var typeSign = animationIndecator[0];
            var characterIndex = animationIndecator[1] - '0';
            Debug.Log(animationIndecator+ "index: " + characterIndex);
            var character = gameState.characterDeck[characterIndex];
            if (typeSign == EventDescription.CHARACTER_ANIM_SIGN) {
                yield return StartCoroutine(CharacterShowup(character));
            }
            else if (typeSign == EventDescription.TRAIT_HIGHTLIGHT_ANIM_SIGN) {
                Trait transferTarget = (Trait)animationIndecator[2];
                yield return StartCoroutine(PlayHightlightTrait(character, transferTarget));
            }
            else if (typeSign == EventDescription.TRAIT_ADD_ANIM_SIGN) {
                Trait transferTarget = (Trait)animationIndecator[2];
                yield return StartCoroutine(PlayAddTrait(character, transferTarget));
            }
            else if (typeSign == EventDescription.TRAIT_REMOVE_ANIM_SIGN) {
                Trait transferTarget = (Trait)animationIndecator[2];
                yield return StartCoroutine(PlayRemoveTrait(character, transferTarget));
            }
        }

        private IEnumerator CharacterShowup(CharacterCard characterCard) {
            if (!viewerDict.ContainsKey(characterCard)) {

                var posDelta = showCharacterSecondAnchor.position - showCharacterFirstAnchor.position;
                var destPos = showCharacterFirstAnchor.position + posDelta * (viewerDict.Count);
                var GO = new GameObject("temp anchor");
                GO.transform.SetParent(showCharacterSpawnAnchor.parent);
                GO.transform.position = destPos;
                var viewer = CharacterCardViewer.Create(characterCard, showCharacterSpawnAnchor, GO.transform);
                viewerDict[characterCard] = viewer;

                yield return new WaitForSeconds(0.5f);
            }  
        }

        private IEnumerator PlayHightlightTrait(CharacterCard characterCard, Trait trait) {
            float showTime = 1f;
            viewerDict[characterCard].HighlightTrait(trait);
            yield return new WaitForSeconds(showTime);
        }

        private IEnumerator PlayRemoveTrait(CharacterCard characterCard, Trait trait) {
            float showTime = 2f;
            viewerDict[characterCard].OnTraitRemove(trait);
            yield return new WaitForSeconds(showTime);
        }
        private IEnumerator PlayAddTrait(CharacterCard characterCard, Trait trait) {
            float showTime = 2f;
            viewerDict[characterCard].OnTraitAdd(trait);
            yield return new WaitForSeconds(showTime);
        }
    }
}