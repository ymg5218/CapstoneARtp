//--------------------------------------------------------------
// 파일명: IngameTimer.cs
// 작성자: 염민규
// 작성일: 2023-05-21
// 설명: 
// - 싱글톤에서 돌아가는 인게임 타이머 수치를 그대로 가져와 UI에 표시
//--------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameTimer : MonoBehaviour
{
    string time;
    public Text text_Timer;

    // 따로 분리한 이유 : 아마 싱글톤의 IngameTimerManager에서 Update로 갱신되는 시간이랑, 여기서 Update로 갱신되는 시간 사이에
    // 조금의 갭이 있는건지, UI에 60, . . , 57, . , 55 , . .  이렇게 중간중간 연산이 누락됨.
    void FixedUpdate()
    {
        time = Mathf.Round(StaticManager.Timer.LimitTime).ToString();
    }
    void Update()
    {
        text_Timer.text = time;
    }
}
