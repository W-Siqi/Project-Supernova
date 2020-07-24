using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemCardDealer 
{
    public static StratagemCard[] Deal(Deck<StratagemCard> stratagemDeck, Kindom.State kindomState)
    {
        return stratagemDeck.GetAll();
    }
}
