
using System;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private int totalCoins;
    public int TotalCoins => totalCoins;
    private void OnEnable ()
    {
        GameEvents.CoinCollected += OnCoinCollected;
    }
    private void OnDisable ()
    {
        GameEvents.CoinCollected -= OnCoinCollected;

    }
    private void OnCoinCollected (int score)
    {
        totalCoins += score;
    }
    private void Start ()
    {
        
    }
    public void CoinCollected()
    {
        totalCoins += 1;
    }
}
