using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifesText;
    public TextMeshProUGUI levelText;
    
    public void Awake()
    {
        instance = this;
        
    }

    public void UpdateScore(int _amount)
    {
        scoreText.text = string.Format("Score: {0:000000}", _amount);
    }
    public void UpdateLife(int _amount)
    {
        lifesText.text = string.Format("X{0}", _amount);
    }
    public void ShowLevel(int _amount)
    {
        levelText.gameObject.SetActive(true);
        levelText.text = string.Format("LEVEL {0}", _amount);
        levelText.GetComponent<Animator>().SetTrigger("SetStage");
    }
    void DisableLevelTexts()
    {
        levelText.gameObject.SetActive(false);
    }
}
