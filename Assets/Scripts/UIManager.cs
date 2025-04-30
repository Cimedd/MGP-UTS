using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text AmmoText;
    public List<GameObject> healthBars = new List<GameObject>();
    public TMP_Text ScoreText;
    public GameObject panelOver;
    private int score = 0;
    private int healthIndex;

    void Start()
    {
        healthIndex = healthBars.Count - 1;
        Time.timeScale = 1f; // Make sure time is unpaused when scene starts
    }

    void Update()
    {

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

    public void GameOver()
    {
        panelOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
