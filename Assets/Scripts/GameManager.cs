using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("Sound")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crashSound;
    AudioSource audioSource;


    [Header("UI")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Gameplay")]
    public bool gameOver = false;
    [SerializeField] float score;

    // Start is called before the first frame update
    void Start()
    {
        // get audio source from player
        audioSource = player.GetComponent<AudioSource>();

        score = 0;
        StartCoroutine(UpdateScore());

    }

    // Update is called once per frame
    void Update()
    {
        //Show score on main game screen
        scoreText.SetText("Score: " + score);

        // Unpause game with up arrow
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResumeGame();
        }
    }

    IEnumerator UpdateScore()
    {
        while(!gameOver)
        {
            score += 1;
            yield return new WaitForSeconds(1);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void RestartGame()
    {
        gameOver = true;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame();
    }

    public void ExitGame()
    {
        // On exit opet Main Menu
        gameOver = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayCrashSound()
    {
        audioSource.PlayOneShot(crashSound);
    }
}
