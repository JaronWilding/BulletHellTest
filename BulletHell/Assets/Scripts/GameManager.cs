using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    static public int level;
    static public int score;
    static public int lifes = 3;

    static public float health;

    int enemyAmount;
    int bonusLifeScore = 10000;
    public Image healthBar = null;

    static int bonusLifes;
    static bool hasLost;

    public void Awake()
    {
        instance = this;
        

        if (hasLost)
        {
            level = 1;
            score = 0;
            lifes = 3;
            health = 1;
            bonusLifes = 0;
            hasLost = false;
        }
    }

    public void Start()
    {
        UIManager.instance.UpdateScore(score);
        UIManager.instance.UpdateLife(lifes);
        UIManager.instance.ShowLevel(level);
    }

    public void AddScore(int _score)
    {
        score += _score;
        UIManager.instance.UpdateScore(score);
        bonusLifes += _score;
        if(bonusLifes >= bonusLifeScore)
        {
            lifes++;
            bonusLifes %= bonusLifeScore;
        }
    }

    public void HealthUpdate(float _health)
    {
        health = _health;
        healthBar.fillAmount = health;
    }

    public void LifeUpdate()
    {
        lifes--;
        UIManager.instance.UpdateLife(lifes);
        if (lifes < 0)
        {
            // Game Over.
        }
    }

    public void AddEnemy()
    {
        enemyAmount++;
    }

    public void WinGame()
    {
        level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
