using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int point = 10;
    private void Update()
    {

    }
    public int Point //미션 하고 얻는 포인트
    {
        get { return point; }
        set { point = value; }
    }
    
    void Start()
    {
    }

    public void addPoint(int point) //미션을 끝내면 포인트 추가됨
    {
        this.point += Mathf.Max(0, point);
    }

    

}
