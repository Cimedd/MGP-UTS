using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text AmmoText;
    public List<GameObject> healthBars = new List<GameObject>();
    public TMP_Text ScoreText, ScoreOverText, gameStatusText;
    public GameObject panelOver,panelStart;
    private int score = 0;
    private int healthIndex;

    void Start()
    {
        healthIndex = healthBars.Count - 1;
        Time.timeScale = 0f; // Make sure time is unpaused when scene starts
    }

    void Update()
    {
        if(score == 10)
        {
            GameOver("YOU WIN");
            Time.timeScale = 0f;
        }
    }

    public void updateAmmo(int ammo)
    {
        AmmoText.text = $"Ammo : {ammo}";
    }

    public void updateHealth(int health)
    {
        for (int i = 0; i < healthBars.Count; i++)
        {
            healthBars[i].SetActive(i < health);
        }
    }

    public void addScore(int value)
    {
        score += value;
        ScoreText.text = $"Score : {score}";
    }

    public void GameOver(string status)
    {
        gameStatusText.text = status;
        ScoreOverText.text = score.ToString();
        panelOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void StartGame()
    {
        panelStart.SetActive(false);
        Time.timeScale+= 1f;
    }
}
