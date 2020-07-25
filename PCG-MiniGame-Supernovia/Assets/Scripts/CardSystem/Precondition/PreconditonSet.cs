using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreconditonSet
{
    public bool environmentEnabled = false;
    public bool characterEnabled = false;
    public bool eventEnabled = false;
    public List<CharacterPrecondition> characterPreconditions = new List<CharacterPrecondition>();
    public EnvironmentPrecondition environmentPrecondition = new EnvironmentPrecondition();
    public List<EventPrecondition> eventPreconditions = new List<EventPrecondition>();

    /// <summary>
    /// 如果连人数都不够，绑定失败，会抛错
    /// </summary>
    /// <returns></returns>
    public CharacterCard[] BindCharacters() {
        var bindedCharacters = new List<CharacterCard>();
        var randomSelectPool = new List<CharacterCard>();
        foreach (var card in StoryContext.instance.characterDeck) {
            randomSelectPool.Add(card);
        }
        for (int i = 0; i < characterPreconditions.Count; i++) {
            var randIndex = Random.Range(0, randomSelectPool.Count);
            bindedCharacters.Add(randomSelectPool[randIndex]);
            randomSelectPool.RemoveAt(randIndex);
        }
        return bindedCharacters.ToArray();
    }
}
