//--------------------------------------------------------------
// 수정 작업자 : 염민규
// 수정 작업일 : 23/04/15
// 수정 목적
// 1. 호출되면 MinigameManager 객체 만들고, 객체 생성자 작동시키면서 최종적으로 해당 미니게임에 대한 랭킹 기록 "먼저" 가져옴
// 2. 일반 풍선 쏘는 로직 
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

//--------------------------------------------------------------
// 마지막 수정 : 염민규, 23/04/15
// 클래스명 : Shoot
// --------------------------------------------------------------
// 기능
// 1. 호출되면 MinigameManager 객체 만들고, 객체 생성자 작동시키면서 최종적으로 해당 미니게임에 대한 랭킹 기록 "먼저" 가져옴
// 2. 일반 풍선 쏘는 로직
// --------------------------------------------------------------
public class Shoot : MonoBehaviour
{   
    public int goal_point = 0;
    public Text result;
    public GameObject arCamera;
    public GameObject smoke;

    MinigameDataManager minigameData;
    public string _lastScore;
    void Start()
    {
        // MinigameDataManager 클래스의 객체를 통해 현재 진행중인 미니게임(씬) 이름을 ShootingGameScene으로 수정
        minigameData = new MinigameDataManager("ShootingGameScene");
        // ShootingGame 랭킹의 최고점수 받아놓음
        _lastScore = minigameData.GetMyRankData();
        
    }

    //--------------------------------------------------------------
    // 임시로 버튼 누르면 점수 잘 작동하나 보려고 만듬.
    // --------------------------------------------------------------
    public void ButtonClick()
    {
        minigameData.MyRankUpdate(_lastScore);
        
    }

    //--------------------------------------------------------------
    // 마지막 수정 : 염민규, 23/04/15
    // 함수명 : Shoot_Balloon()
    // 매개변수 : 없음
    // 반환값 : 없음
    // 제가 수정한 부분은, 풍선을 맞출 때마다 점수가 MinigameDataManager 객체의 score 변수에 쌓이도록만 조작했습니다.
    // --------------------------------------------------------------
    // 기능
    // 1. 걍 풍선 터뜨리는 기존 방식 + 터뜨릴 때마다 점수 추가
    // --------------------------------------------------------------
    public void Shoot_Balloon()
    {
        RaycastHit hit;

        if(Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
        {
            if(hit.transform.tag == "Balloon")
            {
                Destroy(hit.transform.gameObject);

                Instantiate(smoke, hit.point, Quaternion.LookRotation(hit.normal));
                goal_point += 10;
                // 풍선 맞출 때마다 점수 쌓이도록 로직 추가
                minigameData.score = goal_point;
                result.text = goal_point.ToString();
            }
        }
    }
    
    
    
}
