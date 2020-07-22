using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreconditonSet
{
    public List<CharacterPrecondition> characterPreconditions = new List<CharacterPrecondition>();
    public EnvironmentPrecondition environmentPrecondition = new EnvironmentPrecondition();
    public List<EventPrecondition> eventPreconditions = new List<EventPrecondition>();
}
