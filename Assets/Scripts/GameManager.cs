using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;

    [Header("Game Settings")]
    public GameObject gameOverPanel;
    public GameObject tipPanel;
    public Text mainScore;
    public Text finalScore;
    public Text bestScore;
    public float secondsTillRestart = 1f;

    [HideInInspector]
    public bool gameIsOver;
    [HideInInspector]
    public bool gameStarted;

    private int _scoreCoins = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SetGameLoadedState();
    }

    private void Update()
    {
        if (Time.timeScale == 0 && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Time.timeScale = 1;
                gameStarted = true;
            }
        }

        if (gameStarted)
        {
            tipPanel.SetActive(false);
        }

        if (gameIsOver)
        {
            StartCoroutine(SetGameOverState());
        }
    }

    public void SetGameLoadedState()
    {
        mainScore.text = _scoreCoins.ToString();

        Time.timeScale = 0;
        gameStarted = false;
        gameIsOver = false;
        gameOverPanel.SetActive(false);
        tipPanel.SetActive(true);
    }

    public IEnumerator SetGameOverState()
    {
        yield return new WaitForSecondsRealtime(secondsTillRestart);
        Time.timeScale = 0;

        LoadGameOverPanel();

        if (gameIsOver && Input.touchCount > 0)
        {
            RestartLevel();
        }
    }

    private void LoadGameOverPanel()
    {
        gameOverPanel.SetActive(true);

        var maxScore = PlayerPrefs.GetInt("MaxScore");

        if (_scoreCoins > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", _scoreCoins);
        }

        finalScore.text = "Your Score: " + _scoreCoins.ToString();
        bestScore.text = "Best Score: " + PlayerPrefs.GetInt("MaxScore").ToString();
    }

    public void RestartLevel()
    {
        var currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevel);
    }

    public void Collect(int amount)
    {
        _scoreCoins += amount;
        mainScore.text = _scoreCoins.ToString();
    }
}
