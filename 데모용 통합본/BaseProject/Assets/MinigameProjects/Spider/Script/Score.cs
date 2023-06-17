using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    [SerializeField] Text scoreText;
    public static int score = 0;

    void Start()
    {
        gameObject.GetComponent<Shoot>().enabled = true;
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }
}
