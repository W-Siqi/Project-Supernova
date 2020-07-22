using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventCard:Card
{
    public string discruption = "this is a event, somthing happened";
    public PreconditonSet preconditonSet = new PreconditonSet();
    public ConsequenceSet consequenceSet = new ConsequenceSet();
}
