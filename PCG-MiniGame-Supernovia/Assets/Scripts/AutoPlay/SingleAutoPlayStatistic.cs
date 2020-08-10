using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAutoPlayStatistic
{
    /// <summary>
    /// 这个可以为空
    /// </summary>
    public GameLog gameLog = null;

    public bool win;
    public int roundsSurvive;
    public bool lostInEventStream;
    public bool lostInCouncil;
    public bool lostForMoney;
    public bool lostForArmy;
    public bool lostForPeople;
    public bool lostForloyalty;
}
