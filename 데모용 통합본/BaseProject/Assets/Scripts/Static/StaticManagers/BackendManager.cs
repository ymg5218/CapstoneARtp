//--------------------------------------------------------------
// 파일명: BackendManager.cs
// 작성자: 이수민
// 작성일: (-)
// 설명: 뒤끝 API와 연결 수행.
// 수정:
// - 이수민(2023-04-07) - 문서화 작업, 씬 전환 기능 삭제(SceneLoader.cs로 통합)
// - 이수민(2023-04-09) : 싱글턴 해제(StaticManager로 통합)
//--------------------------------------------------------------



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.SceneManagement;

public class BackendManager : MonoBehaviour
{
    //--------------------------------------------------------------
    // 메소드명 : Init()
    // 설명 : 초기화 작업
    //--------------------------------------------------------------
    public void Init() {
        BackendInit();
    }


    //--------------------------------------------------------------
    // 메소드명 : BackendInit()
    // 설명 : 뒤끝 서버와 연결 시도 후, 초기화 작업 수행.
    //--------------------------------------------------------------
    public void BackendInit() {
        var bro = Backend.Initialize(true); // 뒤끝 초기화
        if (bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }
    }
}
