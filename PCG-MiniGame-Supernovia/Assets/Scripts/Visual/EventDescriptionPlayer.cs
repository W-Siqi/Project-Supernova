using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PCG {
    public class EventDescriptionPlayer : MonoBehaviour {
        [SerializeField]
        private AnchorPoint showCharacterFirstAnchor;
        [SerializeField]
        private AnchorPoint showCharacterSecondAnchor;
        [SerializeField]
        private AnchorPoint showCharacterSpawnAnchor;

        private Dictionary<BindingInfo, CardDisplayBehaviour> displayDict = new Dictionary<BindingInfo, CardDisplayBehaviour>();

        public IEnumerator PlayEventDescription(BindingInfo[] bindingInfos, EventDescription eventDescription, TextMeshProUGUI textToFill) {
            float charInterval = 0.05f;
            float paragraphInterval = 1f;
            float holdTime = 1f;
            textToFill.text = eventDescription.title + "\n\n";
            Debug.Log("tittle- " + eventDescription.title);
            foreach (var para in eventDescription.paragragh) {
                Debug.Log("paragrah- " + para);
                int cur = 0;
                while (cur < para.Length) {
                    if (para[cur] == EventDescription.ANI_START_SIGN) {
                        // 动画字符
                        var end = cur;
                        while (end < para.Length && para[end] != EventDescription.ANI_END_SIGN) {
                            end++;
                        }
                        yield return StartCoroutine( PlayAniamtionContent(bindingInfos, para.Substring(cur+1, end - cur)));
                        cur = end + 1;
                    }
                    else {
                        // 普通字符
                        textToFill.text += para[cur];
                        yield return new WaitForSeconds(charInterval);
                        cur++;
                    }
                }
                textToFill.text +='\n';
                yield return new WaitForSeconds(paragraphInterval);
            }

            yield return new WaitForSeconds(holdTime);
            foreach (var displayKV in displayDict) {
                DestroyImmediate(displayKV.Value.gameObject);
            }
            displayDict.Clear();
        }

        // 传进来的是不带()的
        // 动画(c0) ,0 代表bindinginfo的下标
        // 动画(h12),h是hightlight，1 是bindinginfo，2是personalites的下标
        // 动画(t12怒),t是transfer，1 是bindinginfo，2是personalites的下标，3是tranfer的目标
        private IEnumerator PlayAniamtionContent(BindingInfo[] bindingInfos, string animationIndecator) {
            var typeSign = animationIndecator[0];
            var bindingIndex = animationIndecator[1] - '0';
            Debug.Log(animationIndecator+ "index: " + bindingIndex);
            var bindingTarget = bindingInfos[bindingIndex];
            if (typeSign == EventDescription.CHARACTER_ANIM_SIGN) {
                yield return StartCoroutine(CharacterShowup(bindingTarget));
            }
            else if (typeSign == EventDescription.TRAIT_HIGHTLIGHT_ANIM_SIGN) {
                var personalityIndex = animationIndecator[2] - '0';
                yield return StartCoroutine(HightlightTrait(bindingTarget,personalityIndex));
            }
            else if (typeSign == EventDescription.TRAIT_TRANSFER_ANIM_SIGN) {
                var personalityInedx = animationIndecator[2] - '0';
                Trait transferTarget = (Trait)animationIndecator[3];
                yield return StartCoroutine(TransferTrait(bindingTarget, personalityInedx, transferTarget));
            }

        }

        private IEnumerator CharacterShowup(BindingInfo bindingInfo) {
            if (!displayDict.ContainsKey(bindingInfo)) {
                float showTime = 0.6f;

                var cardDisplay = CardDisplayBehaviour.Create(bindingInfo.bindedCharacter, showCharacterSpawnAnchor);
                displayDict[bindingInfo] = cardDisplay;

                var posDelta = showCharacterSecondAnchor.position - showCharacterFirstAnchor.position;
                var destPos = showCharacterFirstAnchor.position + posDelta * (displayDict.Count - 1);
                var destRotate = showCharacterFirstAnchor.rotation;
                LerpAnimator.instance.LerpPositionAndRotation(cardDisplay.transform, destPos, destRotate, showTime);
                yield return new WaitForSeconds(showTime);
            }  
        }

        private IEnumerator HightlightTrait(BindingInfo bindingInfo,int index) {
            float showTime = 1f;
            displayDict[bindingInfo].HighlightPersonality(bindingInfo.bindedCharacter.personalities[index]);
            yield return new WaitForSeconds(showTime);
        }

        private IEnumerator TransferTrait(BindingInfo bindingInfo, int index, Trait traitToTransfer) {
            float showTime = 2f;
            displayDict[bindingInfo].UpdatePersonality(bindingInfo.bindedCharacter.personalities[index],traitToTransfer);
            yield return new WaitForSeconds(showTime);
        }
    }
}