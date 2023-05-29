//--------------------------------------------------------------
// 파일명: MatchingManager.cs
// 작성자: 이수민
// 작성일: 2023-05-05
// 설명: 매칭 시스템 매니저
// 수정:
// - 이수민(2023-05-12) : 세부적인 구조 설계
// - 이수민(2023-05-14) : 세부 구조 구현 중간단계
//--------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
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
    // - matchInfos : 서버에서 생성된 매치 데이터 저장용 리스트.
    // - matchedUserDatas : 현재 매칭된 인원 데이터 저장용 리스트
    //--------------------------------------------------------------
    ErrorInfo errorInfo;
    public List<MatchInfo> matchInfos = new List<MatchInfo>();  
    public List<MatchedUserData> matchedUserDatas;





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

        Backend.Match.GetMatchList(callback => {
            // 요청 실패하는 경우 재요청
            if (callback.IsSuccess() == false) {
                Debug.Log("매칭카드 리스트 불러오기 실패\n" + callback);
                Dispatcher.Current.BeginInvoke(() =>
                {
                    GetMatchList();
                });
                return;
            }
            foreach (LitJson.JsonData row in callback.Rows()) {
                MatchInfo matchInfo = new MatchInfo();
                matchInfo.title = row["matchTitle"]["S"].ToString();
                matchInfo.inDate = row["inDate"]["S"].ToString();
                matchInfo.headCount = row["matchHeadCount"]["N"].ToString();
                matchInfo.isSandBoxEnable = row["enable_sandbox"]["BOOL"].ToString().Equals("True") ? true : false;
                foreach (MatchType type in Enum.GetValues(typeof(MatchType))) {
                    if (type.ToString().ToLower().Equals(row["matchType"]["S"].ToString().ToLower()))
                    {
                        matchInfo.matchType = type;
                    }
                }
                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType))) {
                    if (type.ToString().ToLower().Equals(row["matchModeType"]["S"].ToString().ToLower())) {
                        matchInfo.matchModeType = type;
                    }
                }

                matchInfos.Add(matchInfo);
            }
            Debug.Log("매칭 리스트 불러오기 성공 : " + matchInfos.Count);
        });
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
        StaticManager.PopUpUI.PopUp("매칭을 시작합니다.",()=>{Backend.Match.RequestMatchMaking(MatchInfos[0].matchType, MatchInfos[0].matchModeType, MatchInfos[0].inDate);});
    }


    //--------------------------------------------------------------
    // 메소드명 : JoinGame(MatchMakingResponseEventArgs args)
    // 설명 : 매칭 성공 이후, 반환된 값을 활용해 인게임에 접근하기 위한 작업 진행
    //--------------------------------------------------------------
    public void JoinGame(MatchMakingResponseEventArgs args) {
        var serverAddress = args.RoomInfo.m_inGameServerEndPoint.m_address;
        varserverPort = args.RoomInfo.m_inGameServerEndPoint.m_port;
        var roomToken = args.RoomInfo.m_inGameRoomToken;

        // 인게임 서버 접속
        Backend.Match.JoinGameServer(serverAddress, serverPort, false, out errorInfo);
        
        //게임방 접속하기
        Backend.Match.JoinGameRoom(roomToken);
    }


    //--------------------------------------------------------------
    // 메소드명 : IncreaseScore(string name, int score)
    // 설명 : 미니게임 끝낸 사람의 점수를 업데이트 시키기.
    //--------------------------------------------------------------
    public void IncreaseScore(string name, int score) {
        string msg = name + "|" + score;            // NKYL|200 형태로 보냄.
        data = Encoding.ASCII.GetBytes(msg);        // 바이너리 데이터 화.
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
            switch (args.ErrInfo) {
                case ErrorCode.Success:
                    StaticManager.PopUpUI.PopUp("매칭 성공", ()=>{JoinGame(args);});
                    break;
                default: 
                    StaticManager.PopUpUI.PopUp("매칭 실패");
            }
        };
        Backend.Match.OnSessionJoinInServer += (args) => {
            Debug.Log("인게임 서버 접속 성공.");
            // 매칭 유저 정보 초기화
            matchedUserDatas.clear();
        };
        // 처음 게임방에 접속했을 때 호출되는 이벤트
        // 현재 접속된 유저의 정보 수신하고 리스트에 정리
        Backend.Match.OnSessionListInServer += (args) => {
            // 현재 접속된 유저 정보 리스트로 데꼬옴
            foreach (var member in args.GameRecord)  { 
                MatchedUserData matchedUserData = new MatchedUserData();
                matchedUserData.id = member.m_nickname;
                matchedUserData.score = 0;
                matchedUserDatas.Add(matchedUserData);
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
                matchedUserData.Id = args.GameRecord.m_nickname;
                matchedUserData.Score = 0;
                matchedUserDatas.Add(matchedUserData);
            } else {
                Debug.Log("같은 닉네임이 존재해요!");
            }
        };
        // 매칭된 인원 전원이 인게임 서버에 접속에 성공했을 시, 팀 배정 후 InGameScene으로 전환 수행.
        Backend.Match.OnMatchInGameStart = () => {
            //정렬
            var sortedList = matchedUserDatas.OrderBy(member => member.id).ToList();
            // 원래 리스트 비우고 재정의
            matchedUserDatas.clear();
            matchedUserDatas.AddRange(sortedList);
            // 팀 정의
            for (int i = 0; i < matchedUserDatas.Count; i++) {
                if (i % 2 == 0) {
                    matchedUserDatas[i].team = "RED";
                } else  {
                    matchedUserDatas[i].team = "BLUE";
                }
            }

            StaticManager.PopUpUI.PopUp("매칭 끝! 게임 시작", ()=>{SceneLoader.LoadScene(nowMatchInfo.title);}); 
        };
        // 바이너리 데이터 처리
        Backend.Match.OnMatchRelay += (args) => {
            // 역인코딩
            string str = ByteArrayToString(args.BinaryUserData, Encoding.ASCII);
            
            // 문자열 나누기(아마 NKYL|200 이런 식으로 문자 보낼 예정)
            string[] parts = input.Split('|');

            // 점수 갱신(누가 미니게임 끝내고 서버로 점수 뿌렸을 때, 이를 받아 인게임 씬의 화면에 갱신시키기)
            foreach (var currentMember in matchedUserDatas)  { 
                if (currentMember.id == parts[0]) {
                    currentMember.score += int.Parse(parts[2]);
                    break;
                }
            }
            InGameSceneManager.Instance.UpdateScoreBoard();   // 솔직히 이거, 누가 미니게임 도중이라 InGameSceneManager의 Instance가 비워져 있으면 어떻게될지 모르겠음;;
        };
        // 채팅 메시지 핸들러
        Backend.Match.OnMatchChat = (MatchChatEventArgs args) => {
            InGameSceneManager.Instance.UpdateChat(args.Message);   // 솔직히 이거, 누가 미니게임 도중이라 InGameSceneManager의 Instance가 비워져 있으면 어떻게될지 모르겠음;;
        };
        Backend.Match.OnMatchResult += (args) => {
            // 매치 마무리.
            // 여기서는 뭘 해야 하지??
        };
    }
}
