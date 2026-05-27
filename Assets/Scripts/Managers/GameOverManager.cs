using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    Stats Stats => ServiceProvider.Instance.GetService<Stats>();


    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestartButtonClick);
        scoreText.text = "Score: " + Stats.Score;
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
