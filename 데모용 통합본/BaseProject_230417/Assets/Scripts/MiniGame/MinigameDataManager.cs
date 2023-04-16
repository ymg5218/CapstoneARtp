//--------------------------------------------------------------
// 파일명: MinigameDataManager.cs
// 작성자: 염민규
// 작성일: 2023-04-15
// 설명: 각 미니게임의 점수 관리 및 랭킹 관련 시스템 매니저
// 목표:    1. 미니게임 결과 저장
//          2. 서버에 랭킹 등록
//          3. 서버에서 랭킹 데이터 받아오기 -> GetMyRankData()
//          4. 각 미니게임에서 해당 매니저를 상속받아 점수 관리 및 랭킹 등록이 이루어 질 것.
//--------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.UI;
using System;

// <개요>
// - 각 미니게임의 관리 씬에서 사용하기 위한 미니게임 점수 등록 및 랭킹 관련 시스템.
// <하는 일>
// - 1. 미니게임 결과 저장 (저장 데이터에는 기본적으로 플레이어 이름(Static.PlayerData.userData...로 접근), 미니게임 점수, 랭킹?? 등이 있을 듯) -> 따로 관리용 외부 클래스 파는 것이 좋아보임.
// - 2. 서버에 랭킹 등록. (추가적으로 점수 비교 로직이 필요하면 아마 이쪽에서 수행하거나, 하위 메소드를 파야할 듯?)
// - 3. 서버에서 랭킹 데이터 받아오기. (받아오고나서, 랭킹창을 띄우는걸 이쪽에서 구현할 지, 외부에서 구현할 지는 구현 방식에 따라 다를 듯. (ex. 싱글턴으로 구현하면 아마 전자가 될 것))
// - 4. 본인이 생각하지 못한 기타 기능들... 
// <구현 방식 예시>
// * 이런 방식이 가능하겠다 싶은거 "아주 대충" 생각한 거임. 본인이 판단했을 때 더 좋은 방식이 있으면 알아서 만들기! 
// - 1. 아래의 예시 코드처럼, 각 미니게임 매니저들이 해당 메소드를 상속 받게 만드는 방식.
public class MinigameDataManager : MonoBehaviour
{
    // 미니게임 이름을 담을 변수. 미니게임 별 랭킹 테이블 구분을 위함.
    public string minigameName;
    // 랭킹 테이블 이름
    public string tableName;
    // 랭킹 uuid
    public string rankUUID;
    // 미니게임 수행 점수
    public int score;
    // 이전 미니게임 점수(최고점수)
    public string last_score;

    
    public MinigameDataManager(string _gameName)
    {
        // 진행중인 미니게임의 이름을 받아옴(정확히는 씬 이름을 받아오게 해놨음)
        minigameName = _gameName;
        // 3. 서버에서 랭킹 데이터 받아오기 : 객체 생성하자마자 받아옴
        // GetMyRankData();
    }

    // Json 데이터로 받아온 랭킹 정보를 string 타입으로 변환하기 위한 클래스
    // 받아올 수 있는 데이터 : 플레이어Indate, 플레이어 닉네임, 랭킹에 등록된 점수, 랭킹 순위
    public class RankItem
    {
        public string gamerInDate;
        public string nickname;
        public string score;
        public string index;
        public string rank;
        public string extraData = string.Empty;
        public string extraName = string.Empty;
        public string totalCount;

        public override string ToString()
        {
            string str = $"유저인데이트:{gamerInDate}\n닉네임:{nickname}\n점수:{score}\n정렬:{index}\n순위:{rank}\n총합:{totalCount}\n";
            if (extraName != string.Empty)
            {
                str += $"{extraName}:{extraData}\n";
            }
            return str;
        }
    }

    //--------------------------------------------------------------
    // 마지막 수정 : 염민규, 23/04/15
    // 함수명 : GetMyRankData()
    // 매개변수 : 없음
    // 반환값 : 플레이어의 최고 점수
    // 각 미니게임 스크립트에서 최고점수를 받아놓고, 최고점수는 MyRankUpdate() 함수의 매개변수로 쓰입니다.
    // --------------------------------------------------------------
    // 기능
    // 1. "동기 방식"을 채택. 각 미니게임에서 MinigameDataManager 클래스 상속받으면, 생성자에서 실행될 것. 미니게임을 키자마자 플레이어의 이전 랭킹 데이터를 갖고 시작
    // 2. 각 미니게임에서 MinigameDataManager 객체를 통해 minigameName을 해당 미니게임 이름(정확히는 씬 이름)으로 재정의. 해당 이름에 맞는 랭킹 테이블에 접근하도록 하기 위함.
    // --------------------------------------------------------------
    // 4시간 갈아넣은 소감
    // " 와 진짜 대가리 터지는줄알았네요. 이 자식 비동기로 돌리니까 자꾸 최고점수 담아야하는 변수가 null인 채로 멍때리길래
    //   뭐가 문제인가 했더니 비동기방식이라 이새끼 안꼴리면 작동자체를 안함 ㅋㅋㅋㅋ
    //   최고점수를 담는 작업같은 "무조건 선행되어야 할 작업"은 동기방식으로 해야하는걸 상기시켜주네요."
    //--------------------------------------------------------------
    public string GetMyRankData()
    {
        // --------------------------------------------------------------
        // 마지막 수정 : 염민규, 23/04/15
        // ** 아직 미니게임별 랭킹 테이블 구분 안 함. **
        // 진행중인 미니게임에 따라 작업할 테이블 구분
        // 미니게임이 추가될 때마다 여기도 조건문을 추가해주어야 함.
        // --------------------------------------------------------------

        // case : 슈팅게임
        if (minigameName == "ShootingGameScene")
        {
            tableName = "SCORE_SHOOTING";
            rankUUID = "81f2b860-db39-11ed-9ee7-099cf31f7d52";
            Debug.Log("슈팅게임이네요");
            // tableName = "슈팅게임 랭킹 테이블 이름";
            // rankUUID = "랭킹의 UUID"; 
        }

        // case : 농구게임
        else if(minigameName == "BasketBallGameScene")
        {
            tableName = "SCORE_BASKETBALL";
            rankUUID = "79e12350-db39-11ed-9ee7-099cf31f7d52";
            Debug.Log("농구게임이네요");
            // tableName = "농구게임 랭킹 테이블 이름";
            // rankUUID = "랭킹의 UUID"
        }

        // else에는 exception을 넣어야함
        else 
        {
            Debug.Log("연결 안됐는뎁쇼");
            
        } 

        Debug.Log("호출됨");

        // 동기 방식으로 수행. 최종적으로 플레이어의 랭킹정보를(특히 최고점수) 가져옴
        var bro = Backend.GameData.Get(tableName, new Where(), 1);
        {
            if (bro.IsSuccess() == false)
            {
                Debug.LogError("게임 정보 조회 실패: ");
                return "";
            }

            Debug.Log("게임 정보 조회 성공: " + bro);

            var data = bro.FlattenRows();

            if (data.Count < 0)
            {
                Debug.LogError("게임정보가 비어있습니다.");
                return "";
            }

            var indate = data[0]["inDate"].ToString();

            string userUuid = indate;

            List<RankItem> rankItemList = new List<RankItem>();

            BackendReturnObject bro2 = Backend.URank.User.GetMyRank(rankUUID);

            if (bro2.IsSuccess())
            {
                JsonData rankListJson = bro2.GetFlattenJSON();
                string extraName = string.Empty;
                for (int i = 0; i < rankListJson["rows"].Count; i++)
                {
                    RankItem rankItem = new RankItem();

                    rankItem.gamerInDate = rankListJson["rows"][i]["gamerInDate"].ToString();
                    rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();
                    rankItem.score = rankListJson["rows"][i]["score"].ToString();
                    rankItem.index = rankListJson["rows"][i]["index"].ToString();
                    rankItem.rank = rankListJson["rows"][i]["rank"].ToString();
                    rankItem.totalCount = rankListJson["totalCount"].ToString();

                    if (rankListJson["rows"][i].ContainsKey(rankItem.extraName))
                    {
                        rankItem.extraData = rankListJson["rows"][i][rankItem.extraName].ToString();
                    }

                    rankItemList.Add(rankItem);
                    
                    last_score = rankItem.score;
                }
            }
            else
            {
                Debug.Log("호출 안됨");
            }
        }
        return last_score;
    }


    // --------------------------------------------------------------
    // 마지막 수정 : 염민규, 23/04/15
    // 함수명 : RankInsert()
    // 매개변수 : 랭킹에 등록하려는 점수
    // 반환값 : 없음
    // --------------------------------------------------------------
    // 기능
    // 1. MyRankUpdate() 메소드에 호출됨. 랭킹에 등록 가능한 상황이라, 얻은 점수를 랭킹에 등록할 때 작동할 예정
    // --------------------------------------------------------------
    public void RankInsert(int score)
    {
        
        string rowInDate = string.Empty;

        // 랭킹을 삽입하기 위해서는 게임 데이터에서 사용하는 데이터의 inDate값이 필요합니다.
        // 따라서 데이터를 불러온 후, 해당 데이터의 inDate값을 추출하는 작업을 해야합니다.
        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

        Debug.Log("데이터 조회에 성공했습니다 : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("내 게임 정보의 rowInDate : " + rowInDate); // 추출된 rowIndate의 값은 다음과 같습니다.

        Param param = new Param();
        param.Add("score", score);

        // 추출된 rowIndate를 가진 데이터에 param값으로 수정을 진행하고 랭킹에 데이터를 업데이트합니다.
        Debug.Log("랭킹 삽입을 시도합니다.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID,tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
            return;
        }

        Debug.Log("랭킹 삽입에 성공했습니다. : " + rankBro);
    }


    // --------------------------------------------------------------
    // 마지막 수정 : 염민규, 23/04/15
    // 함수명 : MyRankUpdate()
    // 매개변수 : 플레이어의 최고점수
    // 반환값 : 없음
    // --------------------------------------------------------------
    // 기능
    // 1. 이미 등록된 점수가 없거나(null), 그 점수보다 높은 점수를 얻으면 랭킹 갱신
    // 2. 반대로 이미 등록된 점수보다 낮은 점수를 얻으면 랭킹 갱신 X
    // --------------------------------------------------------------
    public void MyRankUpdate(string _lastScore)
    {
        
        int _score = 1000; // 임시로 만든 score 변수. 각 미니게임에서 받은 최종 점수는 지역변수 score에 저장되게 할 것임.
        Param param = new Param();
        

        param.Add("게임명", "게임 이름 올 자리");
        param.Add("score", _score);
        Backend.GameData.Insert(tableName, param);
        Debug.Log("최고 점수 : " + _lastScore);

        if (_lastScore == null || _score >= Int32.Parse(_lastScore))
        {
            RankInsert(_score);
            Debug.Log("최고 score로 갱신됨");
        }
        else
        {
            Debug.Log("이전 점수보다 낮아 랭킹 등록 X");
        }
    }
}


