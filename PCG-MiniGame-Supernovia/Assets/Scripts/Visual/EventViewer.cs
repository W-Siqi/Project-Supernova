using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class EventViewer : MonoBehaviour {
        [SerializeField]
        private AnchorPoint cardSpawnAnchor;
        [SerializeField]
        private AnchorPoint leftCardShowAnchor;
        [SerializeField]
        private AnchorPoint rightCardShowAnchor;
        [SerializeField]
        private AnchorPoint cardLeaveAnchor;
        public IEnumerator ViewEventCoroutine(EventCard eventCard, BindingInfo[] bindingInfos) {

            // show related character card
            var cardDisplays = new List<CardDisplayBehaviour>();
            foreach (var bindingInfo in bindingInfos) {
                cardDisplays.Add(CardDisplayBehaviour.Create(bindingInfo.bindedCharacter, cardSpawnAnchor));
            }

            float showupInterval = 0.3f;
            for(int i = 0; i < cardDisplays.Count; i++) {
                var cardDisplay = cardDisplays[i];
                var t = (float)(i + 1) /(float) (cardDisplays.Count + 1);
                var destPos = Vector3.Lerp(rightCardShowAnchor.position, leftCardShowAnchor.position, t);
                var destRotate = Quaternion.Lerp(rightCardShowAnchor.rotation, leftCardShowAnchor.rotation, t);
                LerpAnimator.instance.LerpPositionAndRotation(cardDisplay.transform,destPos,destRotate,1f);
                yield return new WaitForSeconds(showupInterval);
            }

            // 生成文字表述
            var description = EventDescription.Generate(eventCard,bindingInfos);
            var descriptionStr = description.title+"\n\n";
            foreach (var p in description.paragragh) {
                descriptionStr += p + "\n";
            }
            StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent(descriptionStr)));
            yield return new WaitForSeconds(3f);

            // card Leave
            foreach (var cardDisplay in cardDisplays) {
                LerpAnimator.instance.LerpPositionAndRotation(
                    cardDisplay.transform, 
                    cardLeaveAnchor.transform.position, 
                    cardLeaveAnchor.transform.rotation, 
                    1f);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}