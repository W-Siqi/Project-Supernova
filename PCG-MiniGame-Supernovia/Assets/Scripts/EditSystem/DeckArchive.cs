using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;
using PCG;

[System.Serializable]
public class DeckArchive : ScriptableObject {
    private const string ARCHIVE_SAVE_DIR = "Assets/Resources";
    private const string ARCHIVE_SAVE_NAME = "DeckArchive.asset";

    public static DeckArchive _instance = null;

    //public List<Card> cards = new List<Card>();
    public List<CharacterCard> characterCards = new List<CharacterCard>();
    public List<EventCard> eventCards = new List<EventCard>();
    public List<StratagemCard> stratagemCards = new List<StratagemCard>();
    public List<SubstoryCard> substoryCards = new List<SubstoryCard>();
    public QualifierLibrary characterQualifierLib= new QualifierLibrary();
    public QualifierLibrary environmentQualifierLib = new QualifierLibrary();

    private Dictionary<Type, List<Card>> directory = null;

    public static DeckArchive instance {
        get {
            if (_instance == null) {
                _instance = LoadOrCreate();
            }
            return _instance;
        }
    }

    //public Card[] GetCards(Type cardType){
    //    var selectedCards = new List<Card>();
    //    foreach (var c in cards) {
    //        if (c.GetType() == cardType) {
    //            selectedCards.Add(c);
    //        }
    //    }
    //    return selectedCards.ToArray();
    //}

    public void AddCard(Card card){
        if (card is CharacterCard) {
            characterCards.Add(card as CharacterCard);
        }
        else if (card is EventCard) {
            eventCards.Add(card as EventCard);
        }
        else if (card is StratagemCard) {
            stratagemCards.Add(card as StratagemCard);
        }
        else if (card is SubstoryCard) {
            substoryCards.Add(card as SubstoryCard);
        }
        else {
            throw new System.Exception("UDF type");
        }
    }

    public void RemoveCard(Card card) {
        if (card is CharacterCard) {
            characterCards.Remove(card as CharacterCard);
        }
        else if (card is EventCard) {
            eventCards.Remove(card as EventCard);
        }
        else if (card is StratagemCard) {
            stratagemCards.Remove(card as StratagemCard);
        }
        else if (card is SubstoryCard) {
            substoryCards.Remove(card as SubstoryCard);
        }
        else {
            throw new System.Exception("UDF type");
        }
    }
    public Card[] GetCards(Type cardType) {
        if (cardType == typeof(CharacterCard)) {
            return characterCards.ToArray();
        }
        else if (cardType == typeof(EventCard)) {
            return eventCards.ToArray();
        }
        else if (cardType == typeof(StratagemCard)) {
            return stratagemCards.ToArray();
        }
        else if (cardType == typeof(SubstoryCard)) {
            return substoryCards.ToArray();
        }
        throw new System.Exception("UDF type");
    }

    public Card FindCardByName(string name) {
        foreach(var card in characterCards){
            if (card.name == name) {
                return card;
            }
        }

        foreach (var card in stratagemCards) {
            if (card.name == name) {
                return card;
            }
        }

        foreach (var card in eventCards) {
            Debug.Log(card.name);
            if (card.name == name) {
                return card;
            }
        }
        return null;
    }

    //public Card[] GetCards<T>() where T:Card{
    //    if (typeof(CharacterCard).IsAssignableFrom(typeof(T))) {
    //        return characterCards.ToArray();
    //    }
    //    else if (typeof(EventCard).IsAssignableFrom(typeof(T))) {
    //        return eventCards.ToArray();
    //    }
    //    else if (typeof(StratagemCard).IsAssignableFrom(typeof(T))) {
    //        return stratagemCards.ToArray();
    //    }
    //    throw new System.Exception("UDF type");
    //}

    private static DeckArchive LoadOrCreate(){
        var savePath = string.Format("{0}/{1}", ARCHIVE_SAVE_DIR, ARCHIVE_SAVE_NAME);
        var archive = AssetDatabase.LoadAssetAtPath<DeckArchive>(savePath);
        if (archive == null)
        {
            if (!Directory.Exists(ARCHIVE_SAVE_DIR))
            {
                Directory.CreateDirectory(ARCHIVE_SAVE_DIR);
            }

            archive = ScriptableObject.CreateInstance<DeckArchive>();
            AssetDatabase.CreateAsset(archive, savePath);
        }
        return archive;
    }
}
