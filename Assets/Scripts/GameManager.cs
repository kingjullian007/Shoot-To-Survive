using UnityEngine;

public enum GameState
{
    GamePlay,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private GameState currentState;

    public GameState CurrentState => currentState;

    private void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SetGameState(GameState.GamePlay); // Start the game in the GamePlay state
    }

    private void OnEnable ()
    {
        GameEvents.GameOver += OnGameOver;
        GameEvents.GameStart += OnGameStart;

    }

    private void OnDisable ()
    {
        GameEvents.GameOver -= OnGameOver;
        GameEvents.GameStart += OnGameStart;
    }

    private void OnGameStart ()
    {
        //SetGameState(GameState.GamePlay);
    }

    private void OnGameOver ()
    {
        SetGameState(GameState.GameOver);
    }

    //private void Start ()
    //{
    //    SetGameState(GameState.GamePlay); // Start the game in the GamePlay state
    //}

    public void SetGameState (GameState state)
    {
        currentState = state;
        Debug.Log("Game state changed to: " + state);
        HandleGameStateChange(state);
    }

    private void HandleGameStateChange (GameState newState)
    {
        switch (newState)
        {
            case GameState.GamePlay:
                Time.timeScale = 1f; // Resume game
                break;
            case GameState.GameOver:
                Time.timeScale = 0f; // Pause game
                ShowGameOverScreen();
                break;
        }
    }

    private void ShowGameOverScreen ()
    {
        // Implement your Game Over logic here, such as displaying a game over screen, stopping gameplay elements, etc.
        Debug.Log("Game Over!");
    }

    public void PlayerDied ()
    {
        SetGameState(GameState.GameOver);
    }
}
