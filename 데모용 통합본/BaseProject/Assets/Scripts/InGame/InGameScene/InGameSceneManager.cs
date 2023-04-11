//--------------------------------------------------------------
// 파일명: InGameSceneManager.cs
// 작성자: 박영훈(GameManager,m_Contents,m_SceneManager,Mission,Mission_b,UI_m,worldscenemanager)
// 작성일: 2023-04-08
// 설명: 인게임 씬 화면 구성 클래스
// 수정:
// - 이수민(2023-04-08) : 대규모 리팩토링(기능 수정, 통합, 재배치), 문서화 작업
// - 이수민(2023-04-11) : 간단한 사용자 정보 UI 표시기능(최종 버전 아님), 간단한 미션 선택 UI 기능(최종 버전 아님)
//--------------------------------------------------------------



using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using System.IO;

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
    // - _missionDataPath : 미션 데이터 저장 장소. Data 폴더안에 JSON 파일로 넣어두었음.
    // <내부 변수>
    // - _missions : 미션 데이터가 저장되는 리스트
    // - _spawnObjects : 인게임에 스폰된 미션 포인트 리스트
    //--------------------------------------------------------------
    [SerializeField] GameObject _InGameSceneUI_User;
    [SerializeField] AbstractMap _map;
    [SerializeField] string _missionDataPath;

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
    // 메소드명 : Start()
    // 설명 : 화면 세팅
    //--------------------------------------------------------------
    void Start() {
        LoadMissionData();      // MissionDataSet에서 미션 정보 가져오기
        SetUIUser();            // 유저 데이터 표시 UI 세팅 메소드.
        SetUIComponent();       // 인게임 UI 세팅 메소드.
        SpawnMissionObjects();  // 미션 오브젝트 생성
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
        string jsonString = File.ReadAllText(_missionDataPath); // JSON 파일 읽기
        _missions = JsonUtility.FromJson<List<MissionData>>(jsonString); // JSON 데이터 파싱
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
    // 설명 : _missions에 초기화된 미션 데이터를 활용하여, 인게임 상에서 미션 포인트를 생성함.
    //--------------------------------------------------------------
    void SpawnMissionObjects() {
        _spawnedObjects = new List<GameObject>();
        for (int i = 0; i < _missions.Count; i++) {
            var mission = _missions[i];
            var instance = Instantiate(Resources.Load<GameObject>(mission.prefabPath));
            instance.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(mission.latitude, mission.longitude), true);
            instance.transform.localScale = new Vector3(mission.scale, mission.scale, mission.scale);
            _spawnedObjects.Add(instance);

            // 미션 오브젝트에 터치 이벤트 리스너 추가
            instance.GetComponent<MeshRenderer>().enabled = false;
            int index = i;
            instance.GetComponentInChildren<Collider>().enabled = true; // Collider를 통해 터치 이벤트 감지
            instance.GetComponentInChildren<Collider>().isTrigger = true; // Collider를 트리거로 설정하여 터치 이벤트를 감지하도록 함
            instance.GetComponentInChildren<Collider>().gameObject.AddComponent<MissionObjectTouchHandler>().OnTouch += () => OnMissionObjectTouched(index);
        }
    }


    //--------------------------------------------------------------
    // 메소드명 : OnMissionObjectTouched(int index)
    // 입력 : 
    // - index - 터치된 애가 어떤 미션 포인트인지 구분해줌.
    // 설명 : 터치된 미션 포인트에 해당하는 _mission 정보를 읽은 뒤, 해당하는 미니게임 씬으로 이동.
    //--------------------------------------------------------------
    void OnMissionObjectTouched(int index) {
        if (index >= 0 && index < _missions.Count) {
            var mission = _missions[index];
            StaticManager.PopUpUI.YesOrNoPopUp("선택된 미션은\n"+mission.sceneName+"에요. 미션을 플레이 하실건가요?",()=>{SceneLoader.LoadScene(mission.sceneName);}, ()=>{});
        } else {
            StaticManager.PopUpUI.PopUp("미션 인덱스 관련해서 오류가 터졌어요.\n어디가 잘못된건지 얼른 찾아보세요!");
        }
    }
}
