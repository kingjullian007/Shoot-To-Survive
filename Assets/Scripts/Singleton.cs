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
            DontDestroyOnLoad(gameObject);

            InitializeInstance();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInstance ()
    {

        if (PlayerControllerInstance == null)
        {
            //Debug.Log("PlayerController component not found on Singleton GameObject.");
            playerController = FindObjectOfType<PlayerController>();
        }

        if (PoolManagerInstance == null)
        {
            //Debug.Log("PoolManager component not found on Singleton GameObject.");
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
        // Reinitialize components if necessary
        if (PlayerControllerInstance == null || PoolManagerInstance == null)
        {
            InitializeInstance();
        }
    }

    private void OnEnable ()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable ()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
