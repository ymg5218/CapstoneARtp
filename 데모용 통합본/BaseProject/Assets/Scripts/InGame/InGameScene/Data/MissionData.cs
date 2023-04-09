//--------------------------------------------------------------
// 파일명: MissionData.cs
// 작성자: 이수민
// 작성일: 2023-04-08
// 설명: 미션 데이터 저장용 클래스
// 수정:
// - 
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// "원래는" 이거 private 시켜놔야 되는데, 지금은 public으로 해놨음. 나중에 빼놓지 말고 바꿔두기

public class MissionData {
    //--------------------------------------------------------------
    // 변수 리스트 :
    // - sceneName : 터치시 이동할 씬 이름.
    // - latitude : 위도? 맞나??
    // - longtitude : 경도?? 아무튼 맞겠지
    // - scale : 미션 포인트 크기
    // - prefabPath : 미션 포인트 프리팹
    //--------------------------------------------------------------
    public string sceneName = string.Empty;
    public double latitude;
    public double longitude;
    public float scale;
    public string prefabPath;


    //--------------------------------------------------------------
    // 메소드명 : ToString()
    // 설명 : 디버깅용 메소드(Debug.log(~~~))
    //--------------------------------------------------------------
    public override string ToString() {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"name : {sceneName}");
        return result.ToString();
    }
}