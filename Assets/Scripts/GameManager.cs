using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject countdownUI;
    [SerializeField] private TextMeshProUGUI countdownText;
    
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private int maxLaps = 3;
    
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button restartButton;
    
    [SerializeField] private InputActionAsset inputActions;
    private InputAction clearHighScoreAction;
    
    private float playerTimer = 0.0f;
    public static bool isGameOver = false;
    private PlayerController player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnPlayerCompletedLap.AddListener(UpdateLapText);
        player = FindAnyObjectByType<PlayerController>();
        
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);

        StartCoroutine(Countdown());
        
        if (!inputActions) return;

        clearHighScoreAction = inputActions.FindAction("Player/ClearHighScore");
        clearHighScoreAction?.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (clearHighScoreAction != null && clearHighScoreAction.triggered)
        {
            ClearHighScore();
        }
        
        if (isGameOver && !gameOverUI.activeSelf)
        {
            GameOver();
            return;
        }

        playerTimer += Time.deltaTime;
        
        UpdateTimerText();
    }

    private IEnumerator Countdown()
    {
        Time.timeScale = 0.0f;
        countdownUI.SetActive(true);

        int time = 3;
        
        while (time > 0)
        {
            countdownText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            time--;
        }
        
        countdownText.text = "GO!";
        
        yield return new WaitForSecondsRealtime(0.5f);
        
        countdownUI.SetActive(false);
        gameplayUI.SetActive(true);
        
        Time.timeScale = 1.0f;
    }

    private void UpdateTimerText()
    {
        timerText.text = playerTimer.ToString("F2");
    }

    private void UpdateLapText()
    {
        if (player.GetCurrentLap() >= maxLaps + 1)
        {
            isGameOver = true;
            return;
        }
        
        lapText.text = "Lap: " + player.GetCurrentLap() + " / " + maxLaps;
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        gameplayUI.SetActive(false);
        
        finalTimeText.text = "Current Time: " + playerTimer.ToString("F2");
        
        bool newHighScore = TrySetHighScore();
        
        highScoreText.text = "Best Time: " + PlayerPrefs.GetFloat("BestTime", float.MaxValue).ToString("F2");
        
        if (newHighScore)
        {
            finalTimeText.color = Color.green;
            highScoreText.color = Color.green;
        }
        
        Time.timeScale = 0.0f;
    }

    private bool TrySetHighScore()
    {
        float currentBestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
       
        if (playerTimer < currentBestTime)
        {
            PlayerPrefs.SetFloat("BestTime", playerTimer);
            return true;
        }
        
        return false;
    }

    private void RestartGame()
    {
        Time.timeScale = 1.0f;
        restartButton.onClick.RemoveAllListeners();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("BestTime");
    }
}
