using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] float maxTime =5f;
    float timeLeft;
    Image timerBar;

    void Start()
    {
        gameOverText.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            gameOverText.SetActive(true);
           // Time.timeScale = 0;
        }
    }
}
