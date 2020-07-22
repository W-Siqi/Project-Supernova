using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// 提供最初的卡片排版和罗列
// 子类可替:
//  1)换具体渲染卡片的delegate
//  2)指定卡组类型
public abstract class DeckEditor : EditorWindow
{    
    private static Texture2D defaultImg;

    protected delegate void DrawCardBlock(Vector2 leftUpAnchor, float width, float height, Card characterCard, out bool isDeleted);
    protected DrawCardBlock drawCardlock = DefaultDrawCardBlock;
    protected Type cardTypeInDeck = typeof(CharacterCard);

    private DeckArchive archive = null;
    private Vector2 showCaseScrollView = Vector2.zero;

    private void OnEnable() {
        archive = DeckArchive.instance;
        if (defaultImg == null) {
            defaultImg = new Texture2D(100, 100);
        }
    }

    private void OnDisable() {
        EditorUtility.SetDirty(archive);
        AssetDatabase.Refresh();
    }

    protected virtual void OnGUI() {
        DrawCharacterShowcaseCol(new Vector2(0, 0), 600);
    }

    private void DrawCharacterShowcaseCol(Vector2 leftUpAnchor, int colWidth) {
        float CHARACTER_BLOCK_HEIGHT = 200;
        int CHARACTER_NUMER_PER_LINE = 3;

        Card[] targetCards = archive.GetCards(cardTypeInDeck);
        // decide scroll view
        var cardNum = targetCards.Length;
        showCaseScrollView = GUI.BeginScrollView(
            new Rect(leftUpAnchor.x, leftUpAnchor.y, colWidth, 600),
            showCaseScrollView,
            new Rect(0, 0, colWidth, CHARACTER_BLOCK_HEIGHT * (cardNum / CHARACTER_NUMER_PER_LINE + 1f)));

        int characterIndex = 0;
        Vector2 anchor = Vector3.zero;
        var cardsToDelete = new List<Card>();
        float blockWidth = (float)colWidth / (float)CHARACTER_NUMER_PER_LINE;
        // block for add new..
        var addNewCharacterBtnPos = new Rect(anchor.x, anchor.y, blockWidth, CHARACTER_BLOCK_HEIGHT);
        if (GUI.Button(addNewCharacterBtnPos, "创建")) {
            archive.AddCard((Card)Activator.CreateInstance(cardTypeInDeck));
        }
        anchor += new Vector2(blockWidth, 0);
        // draw character one by one to fill the grid
        while (characterIndex < cardNum) {
            while (anchor.x < colWidth - blockWidth * 0.5f && characterIndex < cardNum) {
                Card targetCard = targetCards[characterIndex];

                // draw actual character block in the grid
                bool isDeleted;
                drawCardlock.Invoke(anchor, blockWidth, CHARACTER_BLOCK_HEIGHT, targetCard, out isDeleted);
                if (isDeleted) {
                    cardsToDelete.Add(targetCard);
                }

                characterIndex++;
                anchor += new Vector2(blockWidth, 0);
            }

            anchor += new Vector2(0, CHARACTER_BLOCK_HEIGHT);
            anchor.x = 0;
        }

        // process delete request
        foreach (var card in cardsToDelete) {
            archive.RemoveCard(card);
        }

        GUI.EndScrollView();
    }


    // 画格子
    static void DefaultDrawCardBlock(Vector2 leftUpAnchor, float width, float height, Card card, out bool isDeleted) {
        float TEX_HEIGHT_PERSENT = 0.8f;

        // draw avatar image
        Rect texPos = new Rect(leftUpAnchor, new Vector2(width, height * TEX_HEIGHT_PERSENT));
        var avatarImg = card.GetAvatarImage();
        if (avatarImg != null) {
            GUI.DrawTexture(texPos, avatarImg);
        }
        else {
            GUI.DrawTexture(texPos, defaultImg);
        }

        // draw edit operation 
        Rect editBtnPos = new Rect(
            leftUpAnchor + new Vector2(0, height * TEX_HEIGHT_PERSENT),
            new Vector2(width * 0.5f, height * (1 - TEX_HEIGHT_PERSENT)));
        if (GUI.Button(editBtnPos, "编辑")) {
            CardEditor.ShowWindow(card);
        }

        // draw delete operation
        Rect deleteButtonPos = new Rect(
            leftUpAnchor + new Vector2(width * 0.5f, height * TEX_HEIGHT_PERSENT),
            new Vector2(width * 0.5f, height * (1 - TEX_HEIGHT_PERSENT)));
        GUI.color = Color.red;
        if (GUI.Button(deleteButtonPos, "删除")) {
            isDeleted = true;
        }
        else {
            isDeleted = false;
        }
        GUI.color = Color.white;
    }
}
