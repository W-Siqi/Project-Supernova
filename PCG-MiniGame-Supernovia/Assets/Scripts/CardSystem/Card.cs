using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class Card
{
    public string name = "name";

    [SerializeField]
    private string avatarImagePath = "";
    private Texture2D loadedAvatarImage = null;

    public Texture2D GetAvatarImage() {
        if (loadedAvatarImage == null) {
            loadedAvatarImage = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImagePath);
        }
        return loadedAvatarImage;
    }

    public void SetAvatarImage(Texture2D newAvatarImage) {
        avatarImagePath = AssetDatabase.GetAssetPath(newAvatarImage);
        loadedAvatarImage = AssetDatabase.LoadAssetAtPath<Texture2D>(avatarImagePath);
    }
}
