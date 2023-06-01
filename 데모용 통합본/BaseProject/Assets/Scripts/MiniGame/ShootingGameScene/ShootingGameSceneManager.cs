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
    // - scoreText : 현재 얻은 점수 표시. 필요하다면 적당한 UI로 대체 가능.
    // - GameStartButton : 게임 시작 버튼, 임시()
    // <하위 컴포넌트>
    // - BalloonShooter/BalloonSpawner : 풍선 생성 및 사격을 위한 하위 컴포넌트.
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
    // - 미니게임 씬 접근 시, 동작시킬 메소드 순서 정의
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
    //      1. MDM과 통신하여 기본적인 랭킹 정보를 세팅(완료)
    //      2. 내부 변수(점수, ID) 초기화
    //      3. 게임의 UI를 세팅.
    //--------------------------------------------------------------
    public void GameInit() {        
        StaticManager.MinigameData.SetRankData("ShootingGameScene");

        //내부 변수 초기화.
        score = 0;
        id = StaticManager.PlayerData.userData.nickname;
        // 현재 MM의 리스트 데이터를 가져와, 랭킹창 업데이트.

        //여기서 부터는 GameStart로 옮겨야할 수도?
        GameStartButton.SetActive(false); // 최종적으로는 얘 삭제하고, 씬 진입하자마자 바로 5초 카운트내고 30초 타임어택 내기
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
    //      3. 30초 게임 이후 GameEnd 메소드 호출하기
    //--------------------------------------------------------------
    void GameStart() {
    }


    //--------------------------------------------------------------
    // 메소드명 : GameEnd()
    // 설명 : 
    // - 30초 만료시 강제로 호출되는 메소드.
    //      1. 결과창 띄우기.
    //      2. 인게임 씬으로 사출되면서, MM의 내부 리스트의 score 변수에 값 집어넣기.
    //--------------------------------------------------------------
    public void GameEnd() {
        // 서버에 점수 등록
        StaticManager.MinigameData.SetGameResult(score);

        // MM 리스트에 점수 증가++
        StaticManager.Matching.IncreaseTeamScore(StaticManager.PlayerData.userData.nickname, score);

        // 결과 UI 띄우는 메소드를 여기에 넣을 것.
        // (여기!)


        // 메인 씬으로 돌아가는 함수. 나중에 결과 UI에 트리거로서 넣을 것.
        // SceneLoader.LoadScene("InGameScene");
    }



    //--------------------------------------------------------------
    // 메소드명 : IncreaseScore()
    // 설명 : 
    // - 점수를 올리는 메소드
    //--------------------------------------------------------------
    public void IncreaseScore() {
        score += 100;
        scoreText.text = "점수 : " + score.ToString();
    }
}
