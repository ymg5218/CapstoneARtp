using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShopSceneManager : MonoBehaviour
{
    public GameObject Buypopup;
    public GameObject Infopopup;
    private Text ItemName;
    private Text ItemInfo;
    public void Click_Item1_BuyBtn()
    {
        Buypopup.SetActive(true);
        SetBuyPopupUI("미니게임 수행시간 + 15s");
    }
    public void Click_Item1_InfoBtn()
    {
        Infopopup.SetActive(true);
        SetInfoPopupUI("미니게임 수행시간 + 15s 설명");
    }
    public void Click_Item2_BuyBtn()
    {
        Buypopup.SetActive(true);
        SetBuyPopupUI("닉네임 변경권");
    }
    public void Click_Item2_InfoBtn()
    {
        Infopopup.SetActive(true);
        SetInfoPopupUI("닉네임 변경권 설명");
    }

    public void Click_Item3_BuyBtn()
    {
        Buypopup.SetActive(true);
        SetBuyPopupUI("<숙련된 자> 태그 획득");
    }
    public void Click_Item3_InfoBtn()
    {
        Infopopup.SetActive(true);
        SetInfoPopupUI("<숙련된 자> 태그 획득 설명");
    }
    public void Click_Item4_BuyBtn()
    {
        Buypopup.SetActive(true);
        SetBuyPopupUI("닉네임 색 변경권 : 파란색");
    }
    public void Click_Item4_InfoBtn()
    {
        Infopopup.SetActive(true);
        SetInfoPopupUI("닉네임 색 변경권 : 파란색 설명");
    }

    public void SetBuyPopupUI(string _ItemName)
    {
        ItemName = GameObject.Find("Item_Name").GetComponent<Text>();
        ItemName.text = _ItemName;
    }
    public void SetInfoPopupUI(string _ItemInfo)
    {
        ItemInfo = GameObject.Find("Item_Info").GetComponent<Text>();
        ItemInfo.text = _ItemInfo;
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
