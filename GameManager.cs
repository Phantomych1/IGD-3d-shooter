using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Игры")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killsText;

    [Header("UI Окна конца игры")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI loseReasonText;

    [Header("Настройки игры")]
    public float timeLimit = 120f;
    public int targetKills = 30;

    private float currentTime;
    private bool isGameOver = false;
    private int killCount = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        currentTime = timeLimit;
        UpdateKillsUI();
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            LoseGame("Time Left!"); 
        }

        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = (currentTime % 60).ToString("00");

        if (timerText != null) timerText.text = minutes + ":" + seconds;
    }

    public void AddKill()
    {
        if (isGameOver) return;

        killCount++;
        UpdateKillsUI();

        if (killCount >= targetKills)
        {
            WinGame();
        }
    }

    void UpdateKillsUI()
    {
        if (killsText != null) killsText.text = killCount + " / " + targetKills;
    }

    public void LoseGame(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (loseReasonText != null) loseReasonText.text = reason; 
        if (losePanel != null) losePanel.SetActive(true);

        EndGameSetup();
    }

    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (winPanel != null) winPanel.SetActive(true);

        EndGameSetup();
    }

    private void EndGameSetup()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}