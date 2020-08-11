using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [System.Serializable]
    public abstract class Card {
        private static Texture2D defaultImage = null;

        public string name = "name";
        public string description = "";

        public int avatarImageIndex = 0;
        private Texture2D loadedAvatarImage = null;
        private Sprite loadedAvatarSprite = null;

        public static T DeepCopy<T>(T src) {
            var serilized = JsonUtility.ToJson(src);
            return JsonUtility.FromJson<T>(serilized);
        }


        public Texture2D GetAvatarImage() {
            return  ResourcePool.instance.avatarTextures[avatarImageIndex];
        }

        public Sprite GetAvatarSprite() {
            return ResourcePool.instance.avatarSprites[avatarImageIndex];    
        }
    }
}


