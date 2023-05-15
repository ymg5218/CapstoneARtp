//--------------------------------------------------------------
// 파일명: MatchInfo.cs
// 작성자: 이수민
// 작성일: 2023-05-05
// 설명: 매칭 관련 데이터 저장용 클래스
// 수정:
// - 
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using BackEnd;
using BackEnd.Tcp;

public class MatchInfo 
{
    public string title;                // 매칭 명
    public string inDate;               // 매칭 inDate (UUID)
    public MatchType matchType;            // 매치 타입
    public MatchModeType matchModeType;        // 매치 모드 타입
    public string headCount;            // 매칭 인원
    public bool isSandBoxEnable;        // 샌드박스 모드 
}
