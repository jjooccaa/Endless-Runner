using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("UI")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject[] countDownUI;

    //Gameplay
    public float movementSpeed = 10;
    bool gameOver = false;
    bool isGamePaused = false;
    float difficultyIncreaser = 1;
    float increaseDifficultyAfter = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayStart());
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseDifficultyOverTime();

        PauseOrUnpauseGame();
    }

    IEnumerator DelayStart()
    {
        PauseMovement();
        countDownUI[0].SetActive(true);
        for (int i = 1; i < countDownUI.Length; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            countDownUI[i - 1].SetActive(false);
            countDownUI[i].SetActive(true);
        }
        UnpauseMovement();
    }

    void IncreaseDifficultyOverTime()
    {
        // Increase difficulty after 10 score points
        if (ScoreManager.Instance.Score > increaseDifficultyAfter)
        {
            movementSpeed += difficultyIncreaser;
            increaseDifficultyAfter += 10;
        }
    }

    public void RestartGame()
    {
        gameOver = true;
        ScoreManager.Instance.RestartScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnpauseGame();
    }

    void PauseOrUnpauseGame()
    {
        if (!isGamePaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            PauseGame();
        }
        else if (isGamePaused && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            UnpauseGame();
        }
    }
    public void PauseGame()
    {
        isGamePaused = true;
        PauseMovement();
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        UnpauseMovement();
        pauseScreen.SetActive(false);
    }

    void PauseMovement()
    {
        Time.timeScale = 0;
    }

    void UnpauseMovement()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        gameOver = true;
        SceneManager.LoadScene(SceneName.Main_Menu);
    }

    public void GameOver()
    {
        player.GetComponent<PlayerController>().DisableMovement();
        movementSpeed = 0;
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }
    public bool IsGameOver
    {
        get
        {
            return gameOver;
        }
    }
}
