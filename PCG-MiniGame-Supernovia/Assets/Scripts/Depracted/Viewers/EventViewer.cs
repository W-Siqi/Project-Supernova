using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventViewer : MonoBehaviour
{
    public class UserInputHandle
    {
        public bool contiune = false;
    }

    public static EventViewer instance = null;

    [SerializeField]
    GameObject UIRoot;
    [SerializeField]
    Text eventDescription;
    [SerializeField]
    Button toNextButton;
    private UserInputHandle currentInputHandle = null;
    private EventCard[] currentEvents = null;
    private int nextViewIndex = 0;

    private void Awake()
    {
        instance = this;
        HideViewerUI();
        toNextButton.onClick.AddListener(() => ToNextEvent());
    }

    // triggered by button
    public void ToNextEvent() {
        if (currentInputHandle != null)
        {
            if (nextViewIndex < currentEvents.Length)
            {
                // to next event
                ShowNextEvent(currentEvents[nextViewIndex++]);
            }
            else
            {
                // last event viewed, quit
                currentInputHandle.contiune = true;
                currentInputHandle = null;
                currentEvents = null;
                HideViewerUI();
            }
        }
    }

    // view event one by one, once done, the input handle will be set
    public UserInputHandle ViewEvents(EventCard[] events) {
        var inputHandle = new UserInputHandle();
        ShowViewerUI(events[0]);
        currentInputHandle = inputHandle;
        currentEvents = events;
        nextViewIndex = 1;
        return inputHandle;
    }

    private void ShowViewerUI(EventCard startEvent) {
        UIRoot.SetActive(true);
        eventDescription.text = startEvent.discruption;
    }

    private void ShowNextEvent(EventCard nextEvent) {
        eventDescription.text = nextEvent.discruption;
    } 

    private void HideViewerUI() {
        UIRoot.SetActive(false);
        eventDescription.text = "";
    } 
}
