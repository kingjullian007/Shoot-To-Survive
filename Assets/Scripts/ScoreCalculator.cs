using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private int coinsCollected = 0;
    public int CoinsCollected => coinsCollected;



    private void OnEnable ()
    {
        GameEvents.CoinCollected += OnCoinCollected;
    }

    private void OnDisable ()
    {
        GameEvents.CoinCollected -= OnCoinCollected;
    }

    private void Start ()
    {
        coinsCollected = 0;
    }

    private void OnCoinCollected (int score)
    {
        coinsCollected += score;
        GameEvents.ScoreUpdated?.Invoke(coinsCollected);

        // Update the all-time total coins
    }
}
