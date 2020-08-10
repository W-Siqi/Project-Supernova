using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// TBD: evironment 和 event先废弃
[System.Serializable]
public class PreconditonSet
{
    public bool environmentEnabled = false;
    public bool characterEnabled = false;
    public bool eventEnabled = false;
    public List<CharacterPrecondition> characterPreconditions = new List<CharacterPrecondition>();
    public EnvironmentPrecondition environmentPrecondition = new EnvironmentPrecondition();
    public List<EventPrecondition> eventPreconditions = new List<EventPrecondition>();
}
