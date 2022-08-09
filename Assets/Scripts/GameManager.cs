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
    //FIXME Imas dve prazne linije ovde. Jedna je super za citljivost, dve su nepotrebne.

    [Header("UI")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Gameplay")] //FIXME Zasto su ti ove dve stvari exposed u editoru? Za testiranje? Deluju mi prosto za testiranje kroz sam gameplay
    public bool gameOver = false;
    [SerializeField] float score;

    // Start is called before the first frame update
    void Start()
    {
        // get audio source component from player obj //FIXME Nepotreban komentar, ovo je sasvim evidentno iz same linije koda.
        audioSource = player.GetComponent<AudioSource>(); //FIXME Kad resis sve ostale komentare, pozovi me da se cujemo oko ovog :) Pisem ovde tebi jer cu ja da zaboravim.

        score = 0;
        StartCoroutine(UpdateScore());

    }

    // Update is called once per frame
    void Update()
    {
        //Show score on main game screen //FIXME Nepotreban komentar
        scoreText.SetText("Score: " + score);

        // Unpause game with up arrow //FIXME Nepotreban komentar
        if (Input.GetKeyDown(KeyCode.UpArrow)) //FIXME Ovo je vrlo specificno, nije intuitivno. Press any key to continue bi bilo okej, ili isto Esc.
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

    public void RestartGame()
    {
        gameOver = true;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame(); //FIXME Zbog ovog specificnog mesta poziva bih preimenovao funkciju u UnpauseGame. U ovom kontekstu je "resume" zbunjujuce kad ga procitas, posto "restart" podrazumeva pocetak nove igre, a ne nastavak postojece.
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

    public void ExitGame()
    {
        // On exit opet Main Menu //FIXME nepotreban komentar
        gameOver = true;
        SceneManager.LoadScene("MainMenu"); //FIXME Losa je praksa da se referenciras na bilo sta hardkodiranim stringovima. Najbolje imena svih scena izdvoji u konstante u SceneManageru, pa koristi njih. Tako ne mozes da napravis typo kad pozivas LoadScene, a i ako sutra preimenujes neku scenu imas da promenis samo na jednom mestu u kodu (gde ti je definisana konstanta).
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
