
using System;
public static class GameEvents
{
    public static Action<int> CoinCollected;
    public static Action<int> ScoreUpdated;
    public static Action GameOver;
    public static Action EnemyDied;
}
