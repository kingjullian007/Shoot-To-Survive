using System;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private int coinsCollected = 0;
    public int CoinsCollected => coinsCollected;

    private int enemyKilled = 0;
    public int EnemyKilled => enemyKilled;
    private void OnEnable ()
    {
        GameEvents.CoinCollected += OnCoinCollected;
        GameEvents.EnemyDied += OnEnemyDied;
    }
    private void OnDisable ()
    {
        GameEvents.CoinCollected -= OnCoinCollected;
        GameEvents.EnemyDied -= OnEnemyDied;
    }
    private void OnEnemyDied ()
    {
        enemyKilled += 1;
    }
    private void Start ()
    {
        coinsCollected = 0;
        enemyKilled = 0;
    }
    private void OnCoinCollected (int score)
    {
        coinsCollected += score;
        GameEvents.ScoreUpdated?.Invoke(coinsCollected);
    }
}
