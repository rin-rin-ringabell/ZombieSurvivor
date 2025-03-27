using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ZombieSpawner spawner;
    public GameObject gameoverUI;
    public Button reStartButton;

    public TMP_Text bulletText;
    public TMP_Text WaveText;
    public TMP_Text ScoreText;


    public int currentWave = 1;
    public int zombieAmount = 1;

    public int score = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        reStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        gameoverUI.SetActive(false);
    }

    private void Update()
    {
        if (zombieAmount <= 0)
        {
            currentWave++;
            zombieAmount = currentWave / 2;
            for (int i = zombieAmount; i > 0; i--)
            {
                spawner.Spawn();
            }
        }
        WaveText.text = "Wave : " + currentWave.ToString();
        ScoreText.text = "Score : " + (score * 20).ToString();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        gameoverUI.SetActive(true);

    }
}
