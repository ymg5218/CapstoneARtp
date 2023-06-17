using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text pointText;
    public static int point = 0;
    public int Point 
    {
        get { return point; }
        set { point = value; }
    }

    void Update()
    {
        pointText.text = Point.ToString();
    }
}
