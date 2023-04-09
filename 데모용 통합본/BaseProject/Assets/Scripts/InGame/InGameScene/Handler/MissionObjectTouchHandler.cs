//--------------------------------------------------------------
// 파일명: MissionObjectTouchHandler.cs
// 작성자: 이수민
// 작성일: 2023-04-08
// 설명: 휴대폰에서 미션 포인트 터치시 이벤트 처리용 핸들러
// 수정:
// - 
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class MissionObjectTouchHandler : MonoBehaviour
{
    public delegate void TouchHandlerDelegate();
    public event TouchHandlerDelegate OnTouch;

    void Update() {
        // 터치 발생
        if (Input.touchCount > 0) {
            // 터치된 위치 가져옴
            Touch touch = Input.GetTouch(0);
            // 터치 이벤트 처리
            if (touch.phase == TouchPhase.Began) {
                Vector3 touchPosition = touch.position; // 터치된 화면 좌표
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);  // 해당 좌표를 레이로 변환함
                RaycastHit hit;
                // 레이캐스트를 통해 터치한 오브젝트 검출
                if (Physics.Raycast(ray, out hit)) {
                    // 터치한 오브젝트가 현재 오브젝트인 경우
                    if (hit.collider.gameObject == gameObject) {
                        if (OnTouch != null) {
                            OnTouch.Invoke(); // 등록된 이벤트 핸들러 호출
                        }
                    }
                }
            }
        }
    }
}