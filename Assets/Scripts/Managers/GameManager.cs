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
    [SerializeField] GameObject numberOfLivesText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject countDownText;
    
    [SerializeField] int startDelayer = 3;

    //Gameplay
    float movementSpeed = 10;
    public float MovementSpeed 
    { 
        get { return movementSpeed; } 
    }

    bool gameOver = false;
    public bool IsGameOver 
    { 
        get { return gameOver; } 
    }

    bool isGamePaused = false;
    int numberOfLives = 1;

    float difficultyIncreaser = 1;
    float increaseDifficultyAfter = 10;
    float scoreCounter = 0;

    private void OnEnable()
    {
        EventManager.Instance.onPlayerCrash += TakeLife;
        EventManager.Instance.onGameOver += GameOver;
        EventManager.Instance.onPowerUpPickUp += ActivatePowerUp;
    }

    void Start()
    {
        DisplayNumberOfLives();
        StartCoroutine(DelayStart());
    }

    void Update()
    {
        IncreaseDifficultyOverTime(increaseDifficultyAfter);

        PauseOrUnpauseGame();
    }

    IEnumerator DelayStart()
    {
        PauseMovement();

        countDownText.SetActive(true);
        for (int i = startDelayer; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);

            if (i == 0)
            {
                countDownText.GetComponent<TextMeshProUGUI>().text = "GO!!!";
            }
            else
            {
                countDownText.GetComponent<TextMeshProUGUI>().text = i.ToString();
            }
        }

        UnpauseMovement();
    }

    void IncreaseDifficultyOverTime(float increaseDifficultyAfter)
    {
        // Increase difficulty for every value of increaseDifficultyAter
        if (ScoreManager.Instance.Score > scoreCounter)
        {
            movementSpeed += difficultyIncreaser;
            scoreCounter += increaseDifficultyAfter;
            EventManager.Instance.onMovementSpeedChange?.Invoke(movementSpeed);
            EventManager.Instance.onIncreasedDifficulty?.Invoke();
        }
    }

    public void RestartGame()
    {
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
        SceneManager.LoadScene(SceneName.MAIN_MENU);
    }

    void GameOver()
    {
        movementSpeed = 0;
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    void TakeLife()
    {
        numberOfLives --;
        DisplayNumberOfLives();

        if(numberOfLives < 1)
        {
            Death();
        } 
        else 
        {
            ResetPositionOfPlayer();
            ActivateInvisibility(3);
        }
    }

    void Death()
    {
        EventManager.Instance.onGameOver?.Invoke();
    }

    void ResetPositionOfPlayer()
    {
        player.transform.position = Vector3.zero;
    }

    void DisplayNumberOfLives()
    {
        numberOfLivesText.GetComponent<TextMeshProUGUI>().text = "Lives: " + numberOfLives;
    }

    //Power Ups
    public void ActivatePowerUp(GameObject powerUp, int powerUpID)
    {
        switch (powerUpID)
        {
            case 1:
                ActivateInvisibility(5);
                break;
            case 2:
                ActivateExtraLife();
                break;
            case 3:
                break;
        }

        powerUp.SetActive(false);
    }

    void ActivateExtraLife()
    {
        Debug.Log("Extra life power up activated");
        numberOfLives++;
        DisplayNumberOfLives();
    }

    void ActivateFlyPowerUp()
    {
        // Third power up. In progress
    }

    void ActivateInvisibility(int duration)
    {
        Debug.Log("Invisibility activated");
        StartCoroutine(InvisibilityTimer(duration));
    }

    IEnumerator InvisibilityTimer(float time)
    {
        //Ignore collision between player and obstacles and between player and enemies
        Physics.IgnoreLayerCollision(6, 7, true);
        Physics.IgnoreLayerCollision(6, 8, true);
        player.GetComponent<PlayerController>().PlaySmokeParticle();

        yield return new WaitForSeconds(time);

        Physics.IgnoreLayerCollision(6, 7, false);
        Physics.IgnoreLayerCollision(6, 8, false);
        player.GetComponent<PlayerController>().StopSmokeParticle();
    }

}
