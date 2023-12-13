using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static int score { get; private set; }

    static ScoreManager()
    {
        score = 0;
    }

    public static void IncrementScoreFromMerge(FruitType type)
    {
        score += GetScoreForMerge(type);
    }
    
    public static void ResetScore()
    {
        score = 0;
    }

    private static int GetScoreForMerge(FruitType type)
    {
        switch (type)
        {
            case FruitType.Cherry:
                return 1;
            case FruitType.Strawberry:
                return 3;
            case FruitType.Grape:
                return 6;
            case FruitType.Dekopon:
                return 10;
            case FruitType.Persimmon:
                return 15;
            case FruitType.Apple:
                return 21;
            case FruitType.Pear:
                return 28;
            case FruitType.Peach:
                return 36;
            case FruitType.Pineapple:
                return 45;
            case FruitType.Melon:
                return 55;
            case FruitType.Watermelon:
                return 66;
            default:
                return 0;
        }
    }
}
