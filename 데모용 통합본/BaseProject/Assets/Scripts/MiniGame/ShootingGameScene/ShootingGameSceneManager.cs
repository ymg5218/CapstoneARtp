//--------------------------------------------------------------
// 파일명: ShootingGameSceneManager.cs
// 작성자: 이수민
// 작성일: 2023-04-18
// 설명: 
// - 풍선 사격 게임을 관리하는 미니게임 관리 클래스.
// - 화면 구성, 게임 점수 관리, 랭킹 등록, 매칭 제어등의 작업을 수행함.
// <사용 약어 정리>
// - MDM : MinigameDataManager
// - MM : MatchingManager
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;

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
    // <UI 설정>
    // - UI : 점수창/조준선/사격/결과 등록버튼(임시)등 인게임 UI 묶음.
    // - scoreText : 점수창
    // - GameStartButton : 게임 시작 버튼, 임시
    // <하위 컴포넌트>
    // - BalloonShooter/BalloonSpawner : ㅇㅇ
    // <내부 변수>
    // - score : 해당 세션에서 얻은 점수 보관용 변수.
    // - id : 플레이어 닉네임. 서버와의 통신에서 사용할 예정.
    //--------------------------------------------------------------
    [SerializeField] GameObject UI;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject GameStartButton;

    [SerializeField] GameObject BalloonShooter;
    [SerializeField] GameObject BalloonSpawner;

    int score = 0;
    string id;


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
    // 메소드명 : OnEnable()
    // 설명 : 
    // - 미니게임 접근 시, 동작시킬 메소드 순서 정의
    // - 우선은 다음과 같이 구성됨.
    //      1. GameInit(기본적인 핸들러, UI 세팅)
    //      2. StartGame(본격적인 게임 시작)
    //--------------------------------------------------------------
    public void OnEnable() {        
        GameInit();
        GameStart();
    }



    //--------------------------------------------------------------
    // 메소드명 : GameInit()
    // 설명 : 
    // - 게임 시작을 위한 기본적인 밑작업을 수행하는 구간
    //      1. MDM과 통신하여 기본적인 랭킹 정보를 세팅
    //      2. 매칭 관련 핸들러 세팅.
    //      3. 내부 변수(점수, ID) 초기화
    //      4. 게임의 UI를 세팅.
    //--------------------------------------------------------------
    public void GameInit() {        
        StaticManager.MinigameData.SetRankData("ShootingGameScene");
        MatchMakingHandler();

        //내부 변수 초기화.
        score = 0;
        id = StaticManager.PlayerData.userData.nickname;
        // 현재 MM의 리스트 데이터를 가져와, 랭킹창 업데이트.

        //여기서 부터는 GameStart로 옮겨야 함.
        GameStartButton.SetActive(false); // 이새끼 삭제해야 함.
        UI.SetActive(true);

        BalloonShooter.SetActive(true);
        BalloonSpawner.SetActive(true);
    }


    //--------------------------------------------------------------
    // 메소드명 : GameStart()
    // 설명 : 
    // - 실질적인 게임 제어 작업이 들어가는 공간.
    //      1. 5초 카운팅
    //      2. 게임UI/벌룬스포너/벌룬슈터 활성화
    //      3. 
    //--------------------------------------------------------------
    void GameStart() {
    }


    //--------------------------------------------------------------
    // 메소드명 : GameEnd()
    // 설명 : 
    // - 30초 만료시 강제로 호출되는 메소드.
    //      1. 일단 화면에 멈춰! UI 삽입
    //      2. 서버에게 ID+END 메시지 전달
    //      3. 브로드캐스팅 된 데이터를 활용하여 랭킹을 표시함
    //      4. 서버로 게임 종료 메시지 전달 후 인게임 씬으로 탈출
    //--------------------------------------------------------------
    public void GameEnd() {
        // 멈추기 UI 띄우기
        
        // 서버에 END 메시지 전달 + 결과 등록
        StaticManager.MinigameData.SetGameResult(score);
    }



    //--------------------------------------------------------------
    // 메소드명 : IncreaseScore()
    // 설명 : 
    // - 점수를 올리는 메소드
    // - 터트리는 대로 서버에 닉네임+현재 점수를 전달하여, 각 클라이언트에 브로드캐스팅하여 실시간으로 점수판 갱신
    //--------------------------------------------------------------
    public void IncreaseScore() {
        score += 100;
        scoreText.text = "점수 : " + score.ToString();
        // 여기에 서버로 닉네임+현재 점수를 통신하는 코드를 삽입할 것.
    }



    //--------------------------------------------------------------
    // 메소드명 : UpdateScoreboard()
    // 설명 : 스코어보드 갱신 메소드
    //--------------------------------------------------------------
    public void UpdateScoreboard() {
        // 여기에 점수판을 업데이트 하는 코드를 집어넣을 것.
        // 아마 위->아래로 가는 통짜 Textbox주고, \n 구분자로 계속 집어넣으면 될 듯??
    }
}
