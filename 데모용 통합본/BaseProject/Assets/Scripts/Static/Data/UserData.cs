//--------------------------------------------------------------
// 파일명: UserData.cs
// 작성자: 이수민
// 작성일: 2023-03-27
// 설명: 플레이어 데이터 저장용 클래스
// 수정:
// - 이수민(2023-04-07) - 문서화 작업, PlayerDataManager에서 분리
// - 염민규(2023-05-29) - Item 1~4 보유상황도 유저데이터에 추가함
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// "원래는" 이거 private 시켜놔야 되는데, 지금은 public으로 해놨음. 나중에 빼놓지 말고 바꿔두기
public class UserData {
    //--------------------------------------------------------------
    // 변수 리스트 :
    // - nickname : 플레이어 이름.
    // - title : 플레이어 칭호. 일단은 직접 입력인데, 나중에 선택 방식으로 바꿀지 고민중
    // - exp : 플레이어 경험치. 교수님이 넣으래서 넣었음.
    // - money : 플레이어 자본. 2차 데모때 상점 씬에서 사용하게 될 것.
    // - Item 1~4 : 미니게임 시간추가, 닉변권, 태그 구매권, 닉네임 색상 변경권 보유량/보유상황 확인
    //--------------------------------------------------------------
    public string nickname = string.Empty;
    public string title = string.Empty;
    public int exp = 0;
    public int money = 0;
    public int Item1 = 0;
    public int Item2 = 0;
    public bool Item3 = false;
    public bool Item4 = false;


    //--------------------------------------------------------------
    // 메소드명 : ToString()
    // 설명 : 디버깅용 메소드(Debug.log(~~~))
    //--------------------------------------------------------------
    public override string ToString() {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"nickname : {nickname}");
        result.AppendLine($"title : {title}");
        result.AppendLine($"exp : {exp}");
        result.AppendLine($"money : {money}");
        result.AppendLine($"Item1 : {Item1}");
        result.AppendLine($"Item2 : {Item2}");
        result.AppendLine($"Item3 : {Item3}");
        result.AppendLine($"Item4 : {Item4}");

        return result.ToString();
    }
}