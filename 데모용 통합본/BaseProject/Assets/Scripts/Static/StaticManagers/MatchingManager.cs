//--------------------------------------------------------------
// 파일명: MatchingManager.cs
// 작성자: 이수민
// 작성일: 2023-05-05
// 설명: 미니게임 매칭 시스템 매니저
// 수정:
// -
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
    // - nowMatchInfo : "현재" 매칭를 시도하는 미니게임의 매칭 정보
    //--------------------------------------------------------------
    ErrorInfo errorInfo;
    public List<MatchInfo> matchInfos = new List<MatchInfo>();
    MatchInfo nowMatchInfo = new MatchInfo();



    //--------------------------------------------------------------
    // 메소드명 : Init()
    // 설명 : 초기화 작업.
    //--------------------------------------------------------------
    public void Init() {
        MatchMakingHandler();
        JoinMatchMakingServer();
        GetMatchList();
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
    // 설명 : 매칭 서버 접속
    //--------------------------------------------------------------
    void JoinMatchMakingServer() {
        Backend.Match.JoinMatchMakingServer(out errorInfo);
    }


    //--------------------------------------------------------------
    // 메소드명 : GetMatchInfo()
    // 설명 : 콘솔에 생성된 매칭 정보 가져오기.
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
    // 메소드명 : MatchingProcess(string sceneName)
    // 입력 : 
    // - sceneName : 매칭을 시도할 미니게임.
    // 설명 : 미니게임 매칭 작업
    //--------------------------------------------------------------
    public void MatchingProcess(string sceneName) {
        // 매칭 공간 생성
        Backend.Match.CreateMatchRoom();

        //매칭 시작
        for (int i = 0; i < matchInfos.Count; i++) {
            if(matchInfos[i].title.Equals(sceneName)){
                nowMatchInfo = matchInfos[i];
                StaticManager.PopUpUI.PopUp(sceneName+"미션의 매칭을 시작합니다.",()=>{Backend.Match.RequestMatchMaking(nowMatchInfo.matchType, nowMatchInfo.matchModeType, nowMatchInfo.inDate);});
            }
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : JoinGame(MatchMakingResponseEventArgs args)
    // 설명 : 매칭 성공 이후, 반환된 값을 이용한 게임 접근 진행 수순
    //--------------------------------------------------------------
    public void JoinGame(MatchMakingResponseEventArgs args) {
        // 인게임 서버 접속
        Backend.Match.JoinGameServer(serverAddress, serverPort, false, out errorInfo);
        
        //게임방 접속하기
        Backend.Match.JoinGameRoom(roomToken);
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
                default : 
                    StaticManager.PopUpUI.PopUp("매칭 실패");
            }
        };
        Backend.Match.OnSessionJoinInServer += (args) => {
            Debug.Log("인게임 서버 접속 성공.");
        };
        Backend.Match.OnSessionListInServer += (args) => {
             // 게임방 유저 정보 수신
             // 리스트 초기화 코드
             // 간단하게 ID만 받아서, 내부 클래스에 정리하고 리스트로 관리
        };
        Backend.Match.OnMatchInGameStart = () => {
            Debug.Log("게임 시작.");
            SceneLoader.LoadScene(nowMatchInfo.title); // 일단은 사격만
        };
        Backend.Match.OnMatchRelay += (args) => {
            // 바이너리 데이터 처리
            // 얘를 미니게임 매니저 안에다 쳐 넣어야 하나??
            // 아마 사격이
            //      1. 씬 전환 끝나자 마자 3초 세고 30초 동안 풍선 사격
            //      2. 30초 이후에는 강제적으로 점수 데이터를 서버에 보내고, 인게임 팝업 띄워서(잠시 ㄱㄷ리셈) 터치 막아내기.
            //      3. 2~3초 오프셋 내외로 리스트에 찍힌 전원이 보낼 테니, 여기서 결과 값을 받고 정렬시킨 뒤, 결과 팝업을 띄움.
            //      4. 결과 팝업에 ok 버튼 누르면 matchEND 콜백 호출 후 인게임 씬으로 이동.
            // 이렇게 할듯?
        };
        Backend.Match.OnMatchResult += (args) => {
            // 매치 마무리. 여기서 할 것은 별로 없음.
        };
    }
}
