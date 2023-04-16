//--------------------------------------------------------------
// 파일명: MissionData.cs
// 작성자: 이수민
// 작성일: 2023-04-08
// 설명: 미션 데이터 저장용 클래스
// 수정:
// - 이수민(2023-04-12) : 파싱 오류 수정
//--------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[Serializable]
public class MissionData {
    //--------------------------------------------------------------
    // 변수 리스트 :
    // - sceneName : 터치시 이동할 씬 이름.
    // - latitude : 위도? 맞나??
    // - longtitude : 경도?? 아무튼 맞겠지
    // - scale : 미션 포인트 크기
    // - prefabPath : 미션 포인트 프리팹
    //--------------------------------------------------------------
    public string sceneName;
    public double latitude;
    public double longitude;
    public float scale;
    public string prefabPath;
}