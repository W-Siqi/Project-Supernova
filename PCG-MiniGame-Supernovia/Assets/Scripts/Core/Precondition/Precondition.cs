using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Precondition
{
    public abstract bool SatisfiedAt(GameState givenState);
}
