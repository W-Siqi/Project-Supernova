using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCountFilter : CountFilter
{
    public override int Filt() {
        return 3;
    }
}
