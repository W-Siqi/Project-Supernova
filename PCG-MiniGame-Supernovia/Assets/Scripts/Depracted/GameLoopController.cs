using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    [SerializeField]
    private Kindom playerKindom;

    IEnumerator Start() {
        yield return null;
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop() {
        while (true) {
            yield return StartCoroutine(EventState());
            yield return StartCoroutine(CouncilStage());
            yield return StartCoroutine(AffairProcessState());

            yield return null;
        }
    }

    IEnumerator EventState()
    {
        var events = EventGenerator.GenrateEvents(playerKindom.eventDeck, playerKindom.state);
        foreach (var e in events) {
            playerKindom.ApplyEvent(e);
        }

        var inputHandle = EventViewer.instance.ViewEvents(events);
        // wait till continue
        while (!inputHandle.contiune) {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator CouncilStage() {
        StratagemViewer.instance.StartCouncilView();

        foreach (var character in playerKindom.characterDeck.GetAll()) {
            var stragemsFromCharacter = StratagemCardDealer.Deal(character.deck, playerKindom.state);
            foreach (var stratagem in stragemsFromCharacter) {
                var inputHandle = StratagemViewer.instance.ViewStratagem(stratagem,character);
                // wait till user input
                while (!inputHandle.decideded) {
                    yield return null;
                }
                // apply if select 
                if (inputHandle.acceptStratagem) {
                    playerKindom.ApplyStratagem(stratagem);
                }
            }
            yield return null;
        }

        StratagemViewer.instance.EndCouncilView();
    }

    IEnumerator AffairProcessState() {
        var inputHandle = AffairProcessViewer.instance.StartAffairView();
        while (!inputHandle.end) {
            yield return null;
        }
    }
}
