//--------------------------------------------------------------
// 파일명: IngameTimerManager.cs
// 작성자: 염민규
// 작성일: 2023-05-21
// 설명: 인 게임 접근 시점부터, 지속적으로 카운트다운되는 제한시간 관리
//--------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTimerManager : MonoBehaviour
{
    public float LimitTime;
    public bool isTimerStart;

    public void Init()
    {
        isTimerStart = false;
    }
    private void Start()
    {
        LimitTime = 60;
    }
    private void Update()
    {
        // 인 게임 접근시, IngameSceneManager를 통해 isTimerStart를 true로 바꾸어주어, 해당 시점부터 카운트다운 시작
        if (isTimerStart == true)
        {
            if(LimitTime <= 0){
                isTimerStart = false;
                InGameSceneManager.Instance.EndGame();
            }
            else{
                LimitTime -= Time.deltaTime;
            }
        }

    }
}
