using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDescriptionTest : MonoBehaviour
{
    public EventDescriptionPlayer desctriptionPlayer;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        PlayData.instance.InitData();
        foreach (var selectedEvent in DeckArchive.instance.eventCards) {
            var bindingInfos = GameExecuter.BindEventCharacters(PlayData.instance.gameState, selectedEvent);
            if (bindingInfos.Length == selectedEvent.preconditonSet.characterPreconditions.Count) {
                // 绑定成功了才放
                var modification = GameExecuter.CalculteEventConsequence(
                  PlayData.instance.gameState,
                  PlayData.instance.gameConfig,
                  selectedEvent,
                  bindingInfos);

                // 演出
                yield return StartCoroutine(ViewManager.instance.gameStateModifyEventPlayer.PlayEvent(PlayData.instance.gameState, modification));
            }       
        }
    }
}
