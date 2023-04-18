//--------------------------------------------------------------
// 파일명: ShootingGameSceneManager.cs
// 작성자: 이수민
// 작성일: 2023-04-18
// 설명: 
// - 풍선 사격 게임을 관리하는 미니게임 관리 클래스.
// - 게임 점수 관리, 랭킹 등록등의 작업을 수행함.
// <사용 약어 정리>
// - MDM : MinigameDataManager
// 수정 :
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class ShootingGameSceneManager : MonoBehaviour {
    // 자체 인스턴스
    private static ShootingGameSceneManager _instance;
    public static ShootingGameSceneManager Instance {
        get {
            return _instance;
        }
    }

    //--------------------------------------------------------------
    // 변수 리스트 :
    // <유니티 설정>
    //
    // <내부 변수>
    // - score : 해당 세션에서 얻은 점수 보관용 변수.
    //--------------------------------------------------------------
    [SerializeField] Text result;
    int score = 0;


    //--------------------------------------------------------------
    // 메소드명 : Awake()
    // 설명 : instance 세팅
    //--------------------------------------------------------------
    void Awake() {
        if (_instance == null) {
            _instance = this;
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : GameInit()
    // 설명 : 
    // - 게임 시작을 위한 기본적인 밑작업을 수행하는 구간
    // - MDM과 통신하여 기본적인 랭킹 정보를 세팅하는 등의 일을 함.
    //--------------------------------------------------------------
    void GameInit() {        
        /*
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
        */
    }


    //--------------------------------------------------------------
    // 메소드명 : GameInit()
    // 설명 : 
    // - 실질적인 게임 제어 작업이 들어가는 공간.
    //--------------------------------------------------------------
    void StartGame() {        
    }


    //--------------------------------------------------------------
    // 메소드명 : RegisterGameScore()
    // 설명 : 
    // - 게임 완료시 호출되어, 해당 세션에서의 미니게임 결과를 MDM에.
    // - MinigameDataManager와 통신하여 기본적인 랭킹 정보를 세팅하고, 게임 제어 변수들을 초기화함.
    //--------------------------------------------------------------
    void RegisterGameScore() {
    }


    //--------------------------------------------------------------
    // 메소드명 : RegisterGameScore()
    // 설명 : 
    // - 게임 완료시 호출되어, 해당 세션에서의 미니게임 결과를 MDM에.
    // - MinigameDataManager와 통신하여 기본적인 랭킹 정보를 세팅하고, 게임 제어 변수들을 초기화함.
    //--------------------------------------------------------------
    public void IncreaseScore() {
        
    }
}
