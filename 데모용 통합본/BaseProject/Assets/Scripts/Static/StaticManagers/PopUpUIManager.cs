//--------------------------------------------------------------
// 파일명: PopUpUIManager.cs
// 작성자: 이수민
// 작성일: 2023-04-07
// 설명: 
// - 공통으로 사용되는 팝업 UI를 관리하는 매니저
// - 기본 사용법 : StaticManager.PopUpUI.PopUp("표시 내용", 콜백할 메소드); <- 자세한 내용은 아래 메소드 설명 참조할 것!
// 수정:
// - 이수민(2023-04-09) : 문서화 작업, 코드 정리(디자인 변경으로 필요없는 메소드 쳐냄)
// - 이수민(2023-04-09) : 싱글턴 해제(StaticManager로 통합)
//--------------------------------------------------------------



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpUIManager : MonoBehaviour
{
    //--------------------------------------------------------------
    // 변수 리스트 :
    // <콜백 모음집>
    // - Callback : 콜백용 대리자.
    // - callbackOK : OK 버튼 콜백 처리용.
    // - callbackYES : YES 버튼 콜백 처리용.
    // - callbackNO : NO 버튼 콜백 처리용.
    // <사용자 응답 처리>
    // - DialogResponse : 사용자가 어떤 버튼을 갖다 눌렀는지 구분을 위한 열거자 정리
    // - thisResult : 열거자 변수, 나중에 콜백할 때 연결하는 용도로 쓸 거임.
    // <유니티에서 세팅할 거>
    // - Dialogs : 팝업창 프리팹, 여담이지만 아마 나중에 리스트화 시킬 것 같음.
    // - Context : 팝업창 메시지
    // - OneButton : 확인 버튼. OKButton은 이름 겹쳐서 안됨.
    // - YesOrNoButton : 네/아니오 버튼
    // <내부 변수>
    // - OKButton/YesButton/NoButton : 이름대로 ㅇㅇ
    //--------------------------------------------------------------
    public delegate void Callback();
    private Callback callbackOK = null;
    private Callback callbackYES = null;
    private Callback callbackNO = null;

    public enum DialogResponse { OK, YES, NO, ERROR }
    private DialogResponse thisResult;
    
    [SerializeField] GameObject Dialogs;
    [SerializeField] GameObject Context;
    [SerializeField] GameObject OneButton;
    [SerializeField] GameObject YesOrNoButton;
    
    private Button OKButton;
    private Button YesButton;
    private Button NoButton;
    

    //--------------------------------------------------------------
    // 메소드명 : Init()
    // 설명 : 내부 변수에다 버튼 세팅 및 리스너 추가.
    //--------------------------------------------------------------
    public void Init() {
        // 버튼 배정
        OKButton = OneButton.GetComponent<Button>();
        YesButton = YesOrNoButton.transform.Find("YesButton").gameObject.GetComponent<Button>();
        NoButton = YesOrNoButton.transform.Find("NoButton").gameObject.GetComponent<Button>();
        
        // 리스너 추가
        OKButton.onClick.AddListener(PopUpClose);
        YesButton.onClick.AddListener(PopUpClose);
        NoButton.onClick.AddListener(PopUpClose);
    }


    //--------------------------------------------------------------
    // 메소드명 : PopUp(string message)
    // 입력 :
    // - message : 화면에다 갖다 띄울 메시지 내용.
    // 설명 : 콜백없이 "확인" 팝업 UI를 띄우는 메소드.
    //--------------------------------------------------------------
    public void PopUp(string message) {
        SetCallback(null, DialogResponse.OK);
        Context.GetComponent<Text>().text = message;
        YesOrNoButton.SetActive(false);
        OneButton.SetActive(true);
        OKButton.interactable = true;
        Dialogs.SetActive(true);
    }


    //--------------------------------------------------------------
    // 메소드명 : PopUp(string message, Callback Function)
    // 입력 :
    // - message : 화면에다 갖다 띄울 메시지 내용.
    // - Function : 콜백할 메소드
    // 설명 : "확인" 팝업 UI를 띄우는 메소드. 이쪽은 확인 누르면 콜백이 발동됨.
    //--------------------------------------------------------------
    public void PopUp(string message, Callback Function) {
        SetCallback(Function, DialogResponse.OK);
        Context.GetComponent<Text>().text = message;
        YesOrNoButton.SetActive(false);
        OneButton.SetActive(true);
        OKButton.interactable = true;
        Dialogs.SetActive(true);
    }



    //--------------------------------------------------------------
    // 메소드명 : YesOrNoPopUp(string message, Callback yesButtonFuncion, Callback noButtonFuntion)
    // 입력 :
    // - message : 화면에다 갖다 띄울 메시지 내용.
    // - yesButtonFunction : "네" 버튼 눌렀을 때 콜백할 메소드
    // - NoButtonFunction : "싫어요" 버튼 눌렀을 때 콜백할 메소드
    // 설명 : 
    // - "네/아니오" 팝업 UI를 띄우는 메소드.
    // - 특정 버튼에 굳이 콜백 메소드 필요없는 경우, 람다식 '()=>{}' 집어넣기.
    //--------------------------------------------------------------
    public void YesOrNoPopUp(string message, Callback yesButtonFuncion, Callback noButtonFuntion) {
        SetCallback(yesButtonFuncion, DialogResponse.YES);
        SetCallback(noButtonFuntion, DialogResponse.NO);

        Context.GetComponent<Text>().text = message;
        OneButton.SetActive(false);
        YesOrNoButton.SetActive(true);
        YesButton.interactable = true;
        NoButton.interactable = true;
        Dialogs.SetActive(true);
    }



    //--------------------------------------------------------------
    // 메소드명 : PopUpClose()
    // 설명 : 일단 확인/네/싫어요 어떤 버튼이든 눌러서 팝업이 닫히는 경우, 해당하는 콜백 메소드로 연결.
    //--------------------------------------------------------------
    public void PopUpClose()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "OKButton":
                thisResult = DialogResponse.OK;
                callbackOK?.Invoke();
                callbackOK = null;
                break;
                
            case "YesButton":
                thisResult = DialogResponse.YES;
                callbackYES?.Invoke();
                callbackYES = null;
                break;

            case "NoButton":
                thisResult = DialogResponse.NO;
                callbackNO?.Invoke();
                callbackYES = null;
                break;

            default:
                thisResult = DialogResponse.ERROR;
                break;
        }
        OKButton.interactable = false;
        YesButton.interactable = false;
        NoButton.interactable = false;
        Dialogs.SetActive(false);
    }

    //--------------------------------------------------------------
    // 메소드명 : SetCallback(Callback call, DialogResponse buttontype)
    // 설명 : 콜백 세팅용 메소드.
    //--------------------------------------------------------------
    public void SetCallback(Callback call, DialogResponse buttontype)
    {
        switch (buttontype)
        {
            case DialogResponse.OK:
                callbackOK = call;
                break;

            case DialogResponse.YES:
                callbackYES = call;
                break;

            case DialogResponse.NO:
                callbackNO = call;
                break;

            default:
                callbackOK = call;
                callbackYES = call;
                callbackNO = call;
                break;
        }
    }
}
