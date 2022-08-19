using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("Sound Setting")]
    [SerializeField] TextMeshProUGUI volumeTextValue = null;
    [SerializeField] Slider volumeSlider;

    [Header("Fullscreen Setting")]
    [SerializeField] Toggle fullScreenToggle;
}
