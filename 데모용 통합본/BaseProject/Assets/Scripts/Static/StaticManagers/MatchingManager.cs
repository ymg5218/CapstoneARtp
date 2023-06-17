//--------------------------------------------------------------
// 파일명: MatchingManager.cs
// 작성자: 이수민
// 작성일: 2023-05-05
// 설명: 매칭 시스템 매니저
// 수정:
// - 이수민(2023-05-12) : 세부적인 구조 설계
// - 이수민(2023-05-14) : 세부 구조 구현 중간단계
// - 이수민(2023-06-16) : 타이밍 맞추기용 코루틴 함수 추가
//--------------------------------------------------------------


using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Battlehub.Dispatcher;
using System.Linq;

public class MatchingManager : MonoBehaviour {
    //--------------------------------------------------------------
    // 변수 리스트 :
    // <내부 변수> :
    // - errorInfo : 이벤트 처리 결과값 저장용 변수
    // - isSceneActive : 현재 인게임 씬이 동작중인가???
    // - matchInfos : 서버에서 생성된 매치 데이터 저장용 리스트.
    // - matchedUserDatas : 현재 매칭된 인원 데이터 저장용 리스트
    //--------------------------------------------------------------
    ErrorInfo errorInfo;
    public bool isSceneActive = false;
    public List<MatchInfo> matchInfos = new List<MatchInfo>();  
    public List<MatchedUserData> matchedUserDatas = new List<MatchedUserData>();
    public MatchedServerData matchedServerData = new MatchedServerData();
    public class MatchedServerData {
        public string serverAddress;
        public ushort serverPort;
        public string roomToken;
    }





    //--------------------------------------------------------------
    // 메소드명 : Init()
    // 설명 : 초기화 작업.
    //--------------------------------------------------------------
    public void Init() {
        MatchMakingHandler();       // 매칭 관련 핸들러
        JoinMatchMakingServer();    // 매치매이킹 서버 연결 확인
        GetMatchList();             // 서버에 저장된 매칭 정보 획득
    }
    

    //--------------------------------------------------------------
    // 메소드명 : Update()
    // 설명 : Poll함수를 지속적으로 콜하며, 스택된 메시지를 서버로 전송
    //--------------------------------------------------------------
    void Update() {
        Backend.Match.Poll();
    }

    //--------------------------------------------------------------
    // 메소드명 : JoinMatchMakingServer()
    // 설명 : 
    // - 매칭 서버와 소켓 연결이 되어 있는지 확인
    // - 앱 실행시(StaticManager 호출시), 최초로 한번 수행되어 연결이 되어있는지 확인.
    //--------------------------------------------------------------
    void JoinMatchMakingServer() {
        Backend.Match.JoinMatchMakingServer(out errorInfo);
    }


    //--------------------------------------------------------------
    // 메소드명 : GetMatchInfo()
    // 설명 : 서버에 생성된 매칭 정보 가져오기.
    //--------------------------------------------------------------
    void GetMatchList() {
        // 매칭 정보 초기화
        matchInfos.Clear();

        var callback = Backend.Match.GetMatchList();
    
        if (callback.IsSuccess() == false) {
            Debug.Log("매칭카드 리스트 불러오기 실패\n" + callback);
            return;
        }

        LitJson.JsonData matchCardListJson = callback.FlattenRows();

        Debug.Log("서버에서 매칭 정보 불러오기 성공 : " + matchCardListJson.Count);

        for (int i = 0; i < matchCardListJson.Count; i++) {
            MatchInfo matchInfo = new MatchInfo();
            matchInfo.title = matchCardListJson[i]["matchTitle"].ToString();
            matchInfo.inDate = matchCardListJson[i]["inDate"].ToString();
            matchInfo.headCount = matchCardListJson[i]["matchHeadCount"].ToString();
            matchInfo.isSandBoxEnable = matchCardListJson[i]["enable_sandbox"].ToString().Equals("True") ? true : false;
            foreach (MatchType type in Enum.GetValues(typeof(MatchType))) {
                if (type.ToString().ToLower().Equals(matchCardListJson[i]["matchType"].ToString().ToLower()))
                {
                    matchInfo.matchType = type;
                }
            }
            foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType))) {
                if (type.ToString().ToLower().Equals(matchCardListJson[i]["matchModeType"].ToString().ToLower())) {
                    matchInfo.matchModeType = type;
                }
            }

            matchInfos.Add(matchInfo);
        }
        Debug.Log("매칭 리스트 세팅 성공 : " + matchInfos.Count);

        foreach (var matchInfo in matchInfos)
        {
            Debug.Log(matchInfo.title);
        }
    }



    //--------------------------------------------------------------
    // 메소드명 : MatchingProcess()
    // 설명 : 
    // - 메인화면에서 게임 시작 버튼을 누를 경우 호출되는 메소드.
    // - Init 과정에서 받아와 저장한 매칭 정보(MatchInfos)에 기반하여, 뒤끝 내부 함수로 매칭 수행.
    //--------------------------------------------------------------
    public void MatchingProcess() {
        // 매칭 공간 생성
        Backend.Match.CreateMatchRoom();

        //매칭 시작
        Backend.Match.RequestMatchMaking(matchInfos[0].matchType, matchInfos[0].matchModeType, matchInfos[0].inDate);
    }


    //--------------------------------------------------------------
    // 메소드명 : JoinGame(MatchMakingResponseEventArgs args)
    // 설명 : 매칭 성공 이후, 반환된 값을 활용해 인게임에 접근하기 위한 작업 진행
    //--------------------------------------------------------------
    public void JoinGame(MatchMakingResponseEventArgs args) {
        matchedServerData.serverAddress = args.RoomInfo.m_inGameServerEndPoint.m_address;
        matchedServerData.serverPort = args.RoomInfo.m_inGameServerEndPoint.m_port;
        matchedServerData.roomToken = args.RoomInfo.m_inGameRoomToken;

        // 인게임 서버 접속
        Backend.Match.JoinGameServer(matchedServerData.serverAddress, matchedServerData.serverPort, false, out errorInfo);
    }


    //--------------------------------------------------------------
    // 메소드명 : IncreaseTeamScore(string name, int score)
    // 설명 : 미니게임 끝낸 사람의 점수를 업데이트 시키기.
    //--------------------------------------------------------------
    public void IncreaseTeamScore(string name, int score) {
        Debug.Log("전송 시도 :" + name + "|" + score);
        string msg = name + "|" + score;            // NKYL|200 형태로 보냄.
        var data = Encoding.UTF8.GetBytes(msg);        // 바이너리 데이터 화.
        Debug.Log("인코딩 처리 :" + data);
        Backend.Match.SendDataToInGameRoom(data);        
    }


    //--------------------------------------------------------------
    // 메소드명 : MatchMakingHandler()
    // 설명 : 매칭 관련 이벤트 핸들러
    //--------------------------------------------------------------
    private void MatchMakingHandler() {
        Backend.Match.OnJoinMatchMakingServer += (args) => {
            Debug.Log("매칭 서버 접속 성공.");
        };
        Backend.Match.OnMatchMakingRoomCreate += (args) => {
            Debug.Log("대기방 생성 성공.");
        };
        Backend.Match.OnMatchMakingResponse += (args) => {
            Debug.Log(args.ErrInfo);
            switch (args.ErrInfo) {
                case ErrorCode.Success:
                    JoinGame(args);
                    break;
            }
        };
        Backend.Match.OnSessionJoinInServer += (args) => {
            Debug.Log(args.ErrInfo);
            Debug.Log("인게임 서버 접속 성공.");
            // 매칭 유저 정보 초기화
            matchedUserDatas.Clear();
            Debug.Log("유저 정보 초기화 성공.");
            Backend.Match.JoinGameRoom(matchedServerData.roomToken);
        };
        // 처음 게임방에 접속했을 때 호출되는 이벤트
        // 현재 접속된 유저의 정보 수신하고 리스트에 정리
        Backend.Match.OnSessionListInServer += (args) => {
            Debug.Log("게임방 접속 성공.");
            Debug.Log(args.GameRecords);
            // 현재 접속된 유저 정보 리스트로 데꼬옴
            foreach (var member in args.GameRecords)  { 
                MatchedUserData matchedUserData = new MatchedUserData();
                matchedUserData.id = member.m_nickname;
                matchedUserData.score = 0;
                matchedUserDatas.Add(matchedUserData);
            }

            Debug.Log(matchedUserDatas.Count());
            foreach(var UserData in matchedUserDatas){
                Debug.Log(UserData.ToString());
            }
        };
        // 누군가 접속하는 대로 호출되는 이벤트
        // 리스트에 해당 인원이 존재하는지 판단하고, 없으면 리스트에 추가
        Backend.Match.OnMatchInGameAccess += (args) => {
            // 이 새끼 데이터가 내부 리스트에 저장되어 있는지 확인
            bool foundItem = false;

            foreach (var currentMember in matchedUserDatas)  { 
                if (currentMember.id == args.GameRecord.m_nickname) {
                    foundItem = true;
                    break;
                }
            } 
            if(!foundItem){
                MatchedUserData matchedUserData = new MatchedUserData();
                matchedUserData.id = args.GameRecord.m_nickname;
                matchedUserData.score = 0;
                matchedUserDatas.Add(matchedUserData);
            }
            Debug.Log(matchedUserDatas.Count());
            foreach(var UserData in matchedUserDatas){
                Debug.Log(UserData.ToString());
            }
        };

        // 매칭된 인원 전원이 인게임 서버에 접속에 성공했을 시, 팀 배정 후 InGameScene으로 전환 수행.
        Backend.Match.OnMatchInGameStart = () => {
            Debug.Log("게임 시작 트리거 발동");
            //정렬
            var sortedList = matchedUserDatas.OrderBy(member => member.id).ToList();
            Debug.Log("여긴가?");
            // 원래 리스트 비우고 재정의
            matchedUserDatas.Clear();
            matchedUserDatas.AddRange(sortedList);
            Debug.Log("여기?");
            // 팀 정의
            for (int i = 0; i < matchedUserDatas.Count; i++) {
                if (i % 2 == 0) {
                    matchedUserDatas[i].team = "RED";
                } else  {
                    matchedUserDatas[i].team = "BLUE";
                }
            }
            Debug.Log("아니면 여기?");
            isSceneActive = true;
            SceneLoader.LoadScene("InGameScene"); 
        };
        // 바이너리 데이터 처리
        Backend.Match.OnMatchRelay += (args) => {
            Debug.Log("바이너리 데이터 획득");

            string str;
            str = Encoding.UTF8.GetString(args.BinaryUserData);
            
            Debug.Log("얻어낸 정보");

            // 문자열 나누기(아마 NKYL|200 이런 식으로 문자 보낼 예정)
            string[] parts = str.Split('|');

            // 점수 갱신(누가 미니게임 끝내고 서버로 점수 뿌렸을 때, 이를 받아 인게임 씬의 화면에 갱신시키기)
            foreach (var currentMember in matchedUserDatas)  { 
                if (currentMember.id == parts[0]) {
                    currentMember.score += int.Parse(parts[1]);
                    break;
                }
            }
            
            StartCoroutine(WaitForSceneActivation("InGameScene"));
        };
        // 채팅 메시지 핸들러
        Backend.Match.OnMatchChat = (MatchChatEventArgs args) => {
            if (SceneManager.GetActiveScene().name == "InGameScene") {
                InGameSceneManager.Instance.UpdateChat(args.Message);  
            }
        };
        Backend.Match.OnMatchResult += (args) => {
            // 매치 마무리.
            // 여기서는 뭘 해야 하지??
        };
    }


    //--------------------------------------------------------------
    // 메소드명 : WaitForSceneActivation()
    // 설명 : 인게임 씬이 동작할 때까지 기다리게 만드는 함수.
    //--------------------------------------------------------------
    private IEnumerator WaitForSceneActivation(String sceneName)
    {
        while (!isSceneActive)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                isSceneActive = true;
                InGameSceneManager.Instance.UpdateScoreBoard();
            }

            yield return null;
        }
    }
}
