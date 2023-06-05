//--------------------------------------------------------------
// 파일명: InGameSceneManager.cs
// 작성자: 박영훈(GameManager,m_Contents,m_SceneManager,Mission,Mission_b,UI_m,worldscenemanager)
// 작성일: 2023-04-08
// 설명: 인게임 씬 화면 구성 클래스
// 수정:
// - 이수민(2023-04-08) : 대규모 리팩토링(기능 수정, 통합, 재배치), 문서화 작업
// - 이수민(2023-04-11) : 간단한 사용자 정보 UI 표시기능(최종 버전 아님), 간단한 미션 선택 UI 기능(최종 버전 아님)
// - 이수민(2023-04-12) : 미션 데이터 위치 변경(Resource/Settings/InGame안에 있음.)
// - 이수민(2023-04-19) : Start() 제거
// - 이수민(2023-04-19) : JSON 파일 불러오는 방식 변경(안드로이드의 경우 추가)
// - 염민규(2023-05-21) : StartTimer() 추가
// - 이수민(2023-05-24) : StartTimer() 마이너 변경
// - 이수민(2023-05-24) : SendChat()/UpdateScoreBoard()/UpdateChat() 추가
// - 이수민(2023-05-30) : 채팅창/인게임 점수판 임시 UI 작업
//--------------------------------------------------------------



using UnityEngine;
using UnityEngine.UI;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using System.IO;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine.Networking;

public class InGameSceneManager : MonoBehaviour
{
    // 자체 인스턴스
    private static InGameSceneManager _instance;
    public static InGameSceneManager Instance {
        get {
            return _instance;
        }
    }

    //--------------------------------------------------------------
    // 변수 리스트 :
    // <유니티에서 세팅할 거>
    // - _InGameSceneUI_User : 인게임 씬에서 사용할 사용자 정보 표시 UI
    // - _map : Mapbox 맵, 얘가 메인 배경이 될거임.
    // <내부 변수>
    // - _missions : JSON에서 받아온 미션 데이터가 저장되는 리스트
    // - _spawnedObjects : 인게임에 스폰된 미션 포인트 리스트
    //--------------------------------------------------------------
    [SerializeField] GameObject _InGameSceneUI_User;
    [SerializeField] AbstractMap _map;
    [SerializeField] InputField msgbox;
    [SerializeField] Text chatbox;
    [SerializeField] Text redTeamBox;
    [SerializeField] Text blueTeamBox;

    [System.Serializable]
    public class MissionDataWrapper {
        public List<MissionData> missions;
    }

    List<MissionData> _missions;
    List<GameObject> _spawnedObjects;


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
    // 설명 : 화면 세팅
    //--------------------------------------------------------------
    void OnEnable() {
        LoadMissionData();      // MissionDataSet에서 미션 정보 가져오기
        SetUIUser();            // 유저 데이터 표시 UI 세팅 메소드.
        SetUIComponent();       // 인게임 UI 세팅 메소드.
        SpawnMissionObjects();  // 미션 오브젝트 생성
        StartTimer();           // 타이머 시작 23/05/21 민규 추가
        UpdateScoreBoard();     // 유저 정보 갱신.
    }



    //--------------------------------------------------------------
    // 메소드명 : Update()
    // 설명 : 지속적으로 미션 포인트 위치 정보 수정
    //--------------------------------------------------------------
    void Update() {
        for (int i = 0; i < _spawnedObjects.Count; i++)  {
            var spawnedObject = _spawnedObjects[i];
            var mission = _missions[i];
            if (spawnedObject != null) {
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(mission.latitude, mission.longitude), true);
                spawnedObject.transform.localScale = new Vector3(mission.scale, mission.scale, mission.scale);
            }
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : LoadMissionData()
    // 설명 : MissionDataSet에서 미션 정보 가져와서 _missions에 초기화 시킴.
    //--------------------------------------------------------------
    void LoadMissionData() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "MissionDataSet.json");
        string jsonString;
        if (Application.platform == RuntimePlatform.Android) {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone) { }
            jsonString = www.downloadHandler.text;
        }else {
            jsonString = File.ReadAllText(filePath);
        }   

        MissionDataWrapper missionDataWrapper = JsonUtility.FromJson<MissionDataWrapper>(jsonString); // JSON 데이터 파싱
        _missions = missionDataWrapper.missions;
    }


    //--------------------------------------------------------------
    // 메소드명 : SetUIUser()
    // 설명 :
    // - 화면 내 플레이어 정보 UI 세팅, 메인 씬 코드 그대로 들고 옴.
    //--------------------------------------------------------------
    private void SetUIUser() {
        if (_InGameSceneUI_User.activeSelf) {
            return;
        }
        _InGameSceneUI_User.SetActive(true);

        Text[] Texts = _InGameSceneUI_User.GetComponentsInChildren<Text>();

        Texts[0].text = StaticManager.PlayerData.userData.nickname;
        Texts[1].text = StaticManager.PlayerData.userData.title;
        Texts[2].text = "경험치 량 : " + StaticManager.PlayerData.userData.exp.ToString();
        Texts[3].text = "돈 량 : " +StaticManager.PlayerData.userData.money.ToString();
    }


    //--------------------------------------------------------------
    // 메소드명 : SetUIComponent()
    // 설명 : 인게임 UI 세팅용 메소드.
    //--------------------------------------------------------------
    void SetUIComponent() {
        // 지금은 공란, 나중에 뭐 추가할 지 생각해서 추가해보세요!
    }


    //--------------------------------------------------------------
    // 메소드명 : SpawnMissionObjects()
    // 설명 : 에 초기화된 미션 데이터를 활용하여, 인게임 상에서 미션 포인트를 생성함.
    //--------------------------------------------------------------
    void SpawnMissionObjects() {
        _spawnedObjects = new List<GameObject>();
        for (int i = 0; i < _missions.Count; i++) {
            var mission = _missions[i];
            var instance = Instantiate(Resources.Load<GameObject>(mission.prefabPath));
            instance.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(mission.latitude, mission.longitude), true);
            instance.transform.localScale = new Vector3(mission.scale, mission.scale, mission.scale);
            _spawnedObjects.Add(instance);

            int index = i;
            var collider = instance.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            instance.AddComponent<MissionObjectTouchHandler>().OnTouch += () => OnMissionObjectTouched(index);
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : OnMissionObjectTouched(int index)
    // 입력 : 
    // - index - 터치된 애가 어떤 미션 포인트인지 구분해줌.
    // 설명 : 
    // - 터치된 미션 포인트에 해당하는 _mission 정보를 읽은 뒤, 해당하는 미니게임 씬으로 이동.
    // - 가능하면 카운터가 1분 미만일 때, StaticManager.PopUpUI.PopUp("남은 시간이 1분 미만입니다! 중간에 게임이 종료될 수 있습니다") 정도로 알람을 줄것. 
    //--------------------------------------------------------------
    void OnMissionObjectTouched(int index) {
        if (index >= 0 && index < _missions.Count) {
            var mission = _missions[index];
            StaticManager.PopUpUI.YesOrNoPopUp("선택된 미션은 비매칭 미션으로\n"+mission.sceneName+"에요. 미션을 플레이 하실건가요?",()=>{SceneLoader.LoadScene(mission.sceneName);}, ()=>{});
        } else {
            StaticManager.PopUpUI.PopUp("미션 인덱스 관련해서 오류가 터졌어요.\n어디가 잘못된건지 얼른 찾아보세요!");
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : StartTimer()
    // 설명 : 인 게임 씬 최초 접근 시점부터 카운트 시작하도록 bool 값 조정
    // 수정 :
    // - 이수민(2023-05-24) : Timer가 false일 때만 동작하도록 변경. 혹시 나중에 기능 추가할 수 있으니
    //--------------------------------------------------------------
    public void StartTimer() {
        if(StaticManager.Timer.isTimerStart != true) {
            StaticManager.Timer.isTimerStart = true;
        }
    }

    //--------------------------------------------------------------
    // 메소드명 : UpdateScoreBoard()
    // 설명 :
    // - 점수판 업데이트
    //--------------------------------------------------------------
    public void UpdateScoreBoard() {
        redTeamBox.text = "[RED 팀]\n";
        blueTeamBox.text = "[BLUE 팀]\n";

        foreach (var teamMember in StaticManager.Matching.matchedUserDatas)  { 
            if (teamMember.team == "RED") {
                string str = teamMember.id + " : " + teamMember.score + "\n";
                redTeamBox.text += str;
            } else if (teamMember.team == "BLUE") {
                string str = teamMember.id + " : " + teamMember.score + "\n";
                blueTeamBox.text += str;
            }
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : SendChat()
    // 설명 : 
    // - 채팅 보내기 버튼 누르면 발송.
    //--------------------------------------------------------------
    public void SendChat() {
        string str;
        str = msgbox.GetComponent<InputField>().text;   // 화면 내 메시지 박스에서 값 긁어오기.
        string msg = StaticManager.PlayerData.userData.nickname + " : " + str + "\n"; // 대충 "NKYL : 안녕하세요\n" 이런 느낌이 될 것.
        Backend.Match.ChatToGameRoom(MatchChatModeType.All, msg);
    }


    //--------------------------------------------------------------
    // 메소드명 : UpdateChat(string msg)
    // 설명 : MM에서 받아온 msg를 채팅창에 띄우는 역할.
    //--------------------------------------------------------------
    public void UpdateChat(string msg) {
        chatbox.text += msg;
    }
}
