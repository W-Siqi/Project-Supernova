using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiAutoPlayStatistic 
{
    public float winRate = 0;
    public int playCount;
    public int winCount;
    public double averageRoundsCountPerPlay;
    public int lostInEventStream;
    public int lostInCouncil;
    public int lostForMoney;
    public int lostForArmy;
    public int lostForPeople;
    public int lostForloyalty;
    private double totalRoundsPlay;
    public void Merge(SingleAutoPlayStatistic singleAutoPlayStatistic) {
        playCount++;
        winCount += singleAutoPlayStatistic.win ? 1 : 0;
        totalRoundsPlay += singleAutoPlayStatistic.roundsSurvive;
        averageRoundsCountPerPlay = totalRoundsPlay / (double)playCount;
        winRate = (float)winCount / (float)playCount;
        if (singleAutoPlayStatistic.lostInCouncil) {
            lostInCouncil++;
        }
        if (singleAutoPlayStatistic.lostInEventStream) {
            lostInEventStream++;
        }
        if (singleAutoPlayStatistic.lostForMoney) {
            lostForMoney++;
        }
        if (singleAutoPlayStatistic.lostForArmy) {
            lostForArmy++;
        }
        if (singleAutoPlayStatistic.lostForPeople) {
            lostForPeople++;
        }
        if (singleAutoPlayStatistic.lostForloyalty) {
            lostForloyalty++;
        }
    }
}
