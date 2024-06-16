using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    private PlayerController playerController;
    public PlayerController PlayerControllerInstance => playerController;

    private PoolManager poolManager;
    public PoolManager PoolManagerInstance => poolManager;

    private ScoreCalculator scoreCalculator;
    public ScoreCalculator ScoreCalculatorInstance => scoreCalculator;

    private GameManager gameManager;
    public GameManager GameManagerInstance => gameManager;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
          
            InitializeInstance();
        }
    }

    private void InitializeInstance ()
    {

        if (PlayerControllerInstance == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (PoolManagerInstance == null)
        {
            poolManager = FindObjectOfType<PoolManager>();
        }

        if(ScoreCalculatorInstance == null)
        {
            scoreCalculator = GetComponent<ScoreCalculator>();
        }

        if(GameManagerInstance == null)
        {
            gameManager = GetComponent<GameManager>();
        }
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        if (PlayerControllerInstance == null || PoolManagerInstance == null)
        {
            InitializeInstance();
        }
    }

    private void OnEnable ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable ()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
