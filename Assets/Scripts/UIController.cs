using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject PanelGamePlay;
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private Button buttonRestartGame;
    [SerializeField] private Button buttonQuitGame;
    [SerializeField] private TextMeshProUGUI textGamePlayCoinHUD;
    [SerializeField] private TextMeshProUGUI textGameOverCoinHUD;
    [SerializeField] private TextMeshProUGUI textEnemiesKilledHUD;

    private GameManager gameManager;

    private void OnEnable ()
    {
        GameEvents.ScoreUpdated += OnScoreUpdated;
        GameEvents.GameOver += OnGameOver;

    }

    private void OnDisable ()
    {
        GameEvents.ScoreUpdated -= OnScoreUpdated;
        GameEvents.GameOver -= OnGameOver;

    }

    private void OnGameOver ()
    {
        UpdateUIForGameState(GameState.GameOver);
    }

    private void OnScoreUpdated (int score)
    {
        UpdateCoinHUD(score);
    }

    private void Start ()
    {
        gameManager = Singleton.Instance.GameManagerInstance;
        InitButtons();
        UpdateUIForGameState(gameManager.CurrentState);
    }

    private void InitButtons ()
    {
        buttonRestartGame.onClick.AddListener(RestartGame);
        buttonQuitGame.onClick.AddListener(QuitGame);
    }

    private void UpdateUIForGameState (GameState gameState)
    {
        PanelGamePlay.SetActive(gameState == GameState.GamePlay);
        PanelGameOver.SetActive(gameState == GameState.GameOver);
        if (gameState == GameState.GameOver)
        {
            textGameOverCoinHUD.text = "Coins Collected x " + Singleton.Instance.ScoreCalculatorInstance.CoinsCollected.ToString();
            textEnemiesKilledHUD.text = "Enemy Defeated x " + Singleton.Instance.ScoreCalculatorInstance.EnemyKilled.ToString();

        }
    }

    private void RestartGame ()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        gameManager.SetGameState(GameState.GamePlay);
        UpdateUIForGameState( GameState.GamePlay );
    }

    private void QuitGame ()
    {
        Application.Quit();
    }

    private void UpdateCoinHUD (int coins)
    {
        textGamePlayCoinHUD.text = "Coins: " + coins.ToString();
    }
}
