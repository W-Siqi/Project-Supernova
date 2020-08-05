using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusVector {
    public int people = 80;
    public int money = 76;
    public int army = 20;

    public static StatusVector operator +(StatusVector a, StatusVector b) {
        var res = new StatusVector();
        res.people = a.people + b.people;
        res.money = a.money + b.money;
        res.army = a.army + b.army;
        return res;
    }

    public StatusVector(int people, int money, int army) {
        this.people = people;
        this.money = money;
        this.army = army;
    }

    public StatusVector(StatusVector src) {
        this.people = src.people;
        this.money = src.money;
        this.army = src.army;
    }

    public StatusVector() { }

    public void AmplifyValueIfPositive(float amplifyRate) {
        var factor = (1 + amplifyRate);
        if (army > 0) {
            army = (int)(army * factor);
        }

        if (money > 0) {
            money = (int)(money * factor);
        }

        if (people > 0) {
            people = (int)(people * factor);
        }
    }
}