using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class ResourcePool : MonoBehaviour {
        public static ResourcePool _instance = null;
        public static ResourcePool instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<ResourcePool>();
                }
                return _instance;
            }
        }

        public List<Texture2D> avatarTextures;
        public List<Sprite> avatarSprites;
        public List<string> namePool = new List<string>();
        public List<EventCard> eventCards = new List<EventCard>();
        public List<StratagemCard> stratagemCards = new List<StratagemCard>();

# if UNITY_EDITOR
        [ContextMenu("导入存档")]
        private void ImportFromArchive() {
            stratagemCards.Clear();
            foreach (var stragemCard in DeckArchive.instance.stratagemCards) {
                stratagemCards.Add(Card.DeepCopy(stragemCard));
            }

            // copy event pool
            eventCards.Clear();
            foreach (var card in DeckArchive.instance.eventCards) {
                eventCards.Add(Card.DeepCopy(card));
            }
        }
#endif
    }
}