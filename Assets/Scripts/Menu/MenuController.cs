using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] TextMeshProUGUI volumeTextValue; //FIXME Ovo je okej, jasno je sta je. Ako zelis konciznije, moze "volumeLabel"
    [SerializeField] Slider volumeSlider;

    public string newGame;

    // When yes has been pressed in new game dialogue. Load new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(newGame);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0"); //FIXME Cisto sa UX strane mozda nije lose da korisnik vidi volume kao broj od 0 do 10, a ne 0.0 do 0.1. Pogotovu jer si vec odvojio prikaz od vrednosti koju zapravo cuvas.
    }

    public void ApplyVolume() //FIXME Deluje mi da ova funkcija samo cuva volume, a da ga ova iznad zapravo primenjuje (posto menja AudioListener). Mozda ova treba da se zove SaveVolume a ona iznad ApplyVolume?
    {
        // Apply and save volume //FIXME Nepotreban komentar - ime funkcije treba vec da nam da ovu informaciju
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
