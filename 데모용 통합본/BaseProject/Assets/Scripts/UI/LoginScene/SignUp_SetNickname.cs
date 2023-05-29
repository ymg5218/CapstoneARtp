using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class SignUp_SetNickname : MonoBehaviour
{
    [SerializeField] private GameObject _SetNicknameUIGroup;
    [SerializeField] private InputField _playerNicknameInput;

    private void Start()
    {
        StaticManager.Backend.Init();
        SetButton();
    }
    private void SetButton()
    {
        if (_SetNicknameUIGroup.activeSelf)
        {
            return;
        }
        _SetNicknameUIGroup.SetActive(true);

        Button[] buttons = _SetNicknameUIGroup.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
        buttons[0].onClick.AddListener(SetNickname);
    }

    public void SetNickname()
    {
        string playerNickname;
        playerNickname = _playerNicknameInput.GetComponent<InputField>().text;

        var bro = Backend.BMember.CreateNickname(playerNickname);
        if (bro.IsSuccess())
        {
            Backend.BMember.CreateNickname(playerNickname);
            StaticManager.PopUpUI.PopUp("환영합니다. " + playerNickname + " 님!", () => { SceneLoader.LoadScene("MainScene"); });
        }
        else if(playerNickname == null || playerNickname == "")
        {
            StaticManager.PopUpUI.PopUp("닉네임을 입력하세요.");
        }
        else
        {
            StaticManager.PopUpUI.PopUp("이미 존재하는 닉네임입니다.");
        }
    }
}
