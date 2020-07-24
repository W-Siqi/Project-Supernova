using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProbabilityFilter
{
    public class Distrubution {
        public Card[] cards;
        public float[] PDF;
    }

    public abstract Distrubution Filt();
}
