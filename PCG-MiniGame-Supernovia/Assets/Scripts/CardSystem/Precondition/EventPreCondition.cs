using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventPrecondition : Precondition
{
    public string eventCardName = "";

    public EventPrecondition(EventCard eventCard) {
        eventCardName = eventCard.name;
    }

    /// <summary>
    /// TBD: 当前为空实现
    /// </summary>
    /// <returns></returns>
    public override bool SatisfiedByCurrentContext() {
        return true;
    }
}
