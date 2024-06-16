using UnityEngine;

public enum GameState
{
    GamePlay,
    GameOver
}

public class GameManager : MonoBehaviour
{

    private GameState currentState;

    public GameState CurrentState => currentState;

    private void Awake ()
    {
        SetGameState(GameState.GamePlay); // Start the game in the GamePlay state
    }

    private void OnEnable ()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable ()
    {
        GameEvents.GameOver -= OnGameOver;
    }
   
    private void OnGameOver ()
    {
        SetGameState(GameState.GameOver);
    }

    public void SetGameState (GameState state)
    {
        currentState = state;
        HandleGameStateChange(state);
    }

    private void HandleGameStateChange (GameState newState)
    {
        switch (newState)
        {
            case GameState.GamePlay:
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    public void PlayerDied ()
    {
        SetGameState(GameState.GameOver);
    }
}
