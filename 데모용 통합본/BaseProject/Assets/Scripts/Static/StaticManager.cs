//--------------------------------------------------------------
// 파일명: StaticManager.cs
// 작성자: 이수민
// 작성일: 2023-04-09
// 설명: 
// - 자기 혼자 싱글턴으로 생성되어, 정적으로 존재할 매니저들을 싹다 관리함.
// - LoginScene 호출시 같이 호출됨.
// 수정 :
// - 이수민(2023-04-09) : 기존에 산발적이던 싱글턴 통합.
//--------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour
{
    //--------------------------------------------------------------
    // 변수 리스트 :
    // - Instance : 자체 인스턴스
    // <하위 매니저들>
    // - Backend : 백엔드 서버와 연결
    // - PopUpUI : 팝업 UI 관리
    // - PlayerData : 플레이어 데이터 관리
    //--------------------------------------------------------------
    public static StaticManager Instance { get; private set; }

    public static BackendManager Backend { get; private set; }
    public static PopUpUIManager PopUpUI { get; private set; }
    public static PlayerDataManager PlayerData { get; private set; }


    void Awake() {
        Init();
    }


    //--------------------------------------------------------------
    // 메소드명 : Init()
    // 설명 : 초기화 작업, 하위 메소드들을 싹다 초기화한다.
    //--------------------------------------------------------------
    void Init() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Backend = GetComponentInChildren<BackendManager>();
        PlayerData = GetComponentInChildren<PlayerDataManager>();
        PopUpUI = GetComponentInChildren<PopUpUIManager>();

        Backend.Init();
        PlayerData.Init();
        PopUpUI.Init();
    }
}
