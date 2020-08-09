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
            var bindingInfos = selectedEvent.preconditonSet.Bind(PlayData.instance.gameState);
            // 必须要在apply结果前面进行演出
            Debug.Log(string.Format("=========================[{0}]-[{1}]========", selectedEvent.name, selectedEvent.description));
            var description = EventDescription.Generate(selectedEvent, bindingInfos);

            yield return StartCoroutine(desctriptionPlayer.PlayEventDescription(bindingInfos, description));

            selectedEvent.consequenceSet.Apply(bindingInfos, PlayData.instance.gameState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
