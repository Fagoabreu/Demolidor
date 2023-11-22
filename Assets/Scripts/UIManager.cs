using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider healthUI;
    public Image playerImage;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI livesText;

    public GameObject enemyUI;
    public Slider enemySlider;
    public Image enemyImage;
    public TextMeshProUGUI enemyName;
    public GameObject gameoverobject;

    public float enemyUITime = 4f;

    private float enemyTimer;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        healthUI.maxValue = player.maxHealth;
        healthUI.value = healthUI.maxValue;
        playerName.text = player.playerName;
        playerImage.sprite = player.playerImage;
    }

    // Update is called once per frame
    void Update()
    {
        enemyTimer += Time.deltaTime;
        if(enemyTimer >= enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
            
        }
    }

    public void UpdateHealth(int amount)
    {
        healthUI.value = amount;
    }

    public void UpdateEnemyUI(int maxHeath, int currentHeath, string name, Sprite image)
    {
        enemySlider.maxValue = maxHeath;
        enemySlider.value = currentHeath;
        enemyName.text = name;
        enemyImage.sprite = image;
        enemyTimer = 0;
        enemyUI.SetActive(true);
    }

    public void GameOver() {
        gameoverobject.SetActive(true );
        Time.timeScale = 0f;
    }
}
