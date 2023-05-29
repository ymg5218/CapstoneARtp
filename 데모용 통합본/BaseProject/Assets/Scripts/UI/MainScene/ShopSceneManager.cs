using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using BackEnd;
using LitJson;
using System;

//--------------------------------------------------------------
// 파일명: ShopSceneManager.cs
// 작성자: 염민규
// 작성일: 2023-05-27
// 설명: 
// - 상점에 파는 아이템 차트 정보를 뒤끝api에서 Json 형태로 받아와 상점 UI에 표시
// 뒤끝 콘솔 - 차트 관리 참고
//--------------------------------------------------------------

public class ShopSceneManager : MonoBehaviour
{
    //--------------------------------------------------------------
    // 클래스명 : ItemSource
    // 설명 : Json 파일로 받아온 상점에 파는 아이템 정보들을 담을 클래스
    // ItemID : 아이템 고유번호
    // ItemName : 아이템 이름
    // ItemPrice : 아이템 가격
    // ItemInfo : 아이템 설명 정보
    //--------------------------------------------------------------
    class ItemSource
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
        public string ItemInfo { get; set; }
    }
    List<ItemSource> items = new List<ItemSource>();
    public GameObject Buypopup;
    public GameObject Infopopup;
    private Text ItemName;
    private Text ItemInfo;

    [SerializeField] private GameObject _ShopSceneUI_User;
    [SerializeField] private GameObject _ItemCount;

    private void Start()
    {
        StaticManager.Backend.Init();
        GetChartList();
        SetUIUser();
        CountItem();
    }
    //--------------------------------------------------------------
    // 메소드명 : SetUIUser()
    // 설명 :
    // - 화면 좌상단 플레이어 정보 UI 세팅
    //--------------------------------------------------------------
    private void SetUIUser()
    {
        //_ShopSceneUI_User.SetActive(true);
        NewGameDataGet();
        Text[] Texts = _ShopSceneUI_User.GetComponentsInChildren<Text>();

        Texts[0].text = StaticManager.PlayerData.userData.nickname;
        Texts[1].text = StaticManager.PlayerData.userData.title;
        Texts[2].text = "경험치 량 : " + StaticManager.PlayerData.userData.exp.ToString();
        Texts[3].text = "돈 량 : " + StaticManager.PlayerData.userData.money.ToString();
    }
    public void NewGameDataGet()
    {
        StaticManager.PlayerData.GameDataFind();
    }
    public void GetChartList()
    {
        var bro = Backend.Chart.GetChartContents("81337");
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("에러가 발생했습니다 : " + bro.ToString());
            return;
        }
        List<ItemSource> itemSources = new List<ItemSource>();
        foreach (JsonData itemData in bro.FlattenRows())
        {
            ItemSource itemSource = new ItemSource();

            itemSource.ItemID = itemData["ItemID"].ToString();
            itemSource.ItemName = itemData["ItemName"].ToString();
            itemSource.ItemPrice = Int32.Parse(itemData["ItemPrice"].ToString());
            itemSource.ItemInfo = itemData["ItemInfo"].ToString();

            itemSources.Add(itemSource);
        }

        items = itemSources;
    }
    
    public void BuyBtnListener(int itemID)
    {
        if (StaticManager.PlayerData.userData.money < items[itemID].ItemPrice)
        {
            StaticManager.PopUpUI.PopUp("크레딧이 부족합니다.");
        }
        else
        {
            Param param = new Param();
            param.Add("money", StaticManager.PlayerData.userData.money -= items[itemID].ItemPrice);
            var bro = Backend.GameData.UpdateV2("USER_DATA", StaticManager.PlayerData.gameDataRowInDate, Backend.UserInDate, param);
            
            if(itemID == 0)
            {
                param.Add("Item1", StaticManager.PlayerData.userData.Item1 += 1);
                Backend.GameData.UpdateV2("USER_DATA", StaticManager.PlayerData.gameDataRowInDate, Backend.UserInDate, param);

            }
            else if(itemID == 1)
            {
                param.Add("Item2", StaticManager.PlayerData.userData.Item2 += 1);
                Backend.GameData.UpdateV2("USER_DATA", StaticManager.PlayerData.gameDataRowInDate, Backend.UserInDate, param);
            }
            else if(itemID == 2)
            {
                param.Add("Item3", true);
                Backend.GameData.UpdateV2("USER_DATA", StaticManager.PlayerData.gameDataRowInDate, Backend.UserInDate, param);
            }
            else
            {
                param.Add("Item4", true);
                Backend.GameData.UpdateV2("USER_DATA", StaticManager.PlayerData.gameDataRowInDate, Backend.UserInDate, param);
            }
            
            if (bro.IsSuccess())
            {
                StaticManager.PopUpUI.PopUp("구매가 완료되었습니다.");
                SetUIUser();
                CountItem();
            }
            else
            {
                StaticManager.PopUpUI.PopUp("구매 실패하였습니다.");
            }
        }
    }
    public void CountItem()
    {
        NewGameDataGet();
        Text[] Texts = _ItemCount.GetComponentsInChildren<Text>();
        Texts[0].text = "보유 개수 : " + StaticManager.PlayerData.userData.Item1.ToString();
        Texts[1].text = "보유 개수 : " + StaticManager.PlayerData.userData.Item2.ToString();
        if (StaticManager.PlayerData.userData.Item3 == true) 
        {
            Texts[2].text = "보유 중";
        }
        else
        {
            Texts[2].text = "보유 하지 않음";
        }
        if (StaticManager.PlayerData.userData.Item4 == true)
        {
            Texts[3].text = "보유 중";
        }
        else
        {
            Texts[3].text = "보유 하지 않음";
        }
    }
    public void Click_Item1_BuyBtn()
    {
        BuyBtnListener(0);
            
    }
    public void Click_Item1_InfoBtn()
    {
        StaticManager.PopUpUI.PopUp(items[0].ItemInfo);
    }


    public void Click_Item2_BuyBtn()
    {
        BuyBtnListener(1);
    }
    public void Click_Item2_InfoBtn()
    {
        StaticManager.PopUpUI.PopUp(items[1].ItemInfo);
    }

    public void Click_Item3_BuyBtn()
    {
        if (StaticManager.PlayerData.userData.Item3)
        {
            StaticManager.PopUpUI.PopUp("이미 보유중인 아이템입니다.");
        }
        else
        {
            BuyBtnListener(2);
        }
    }
        
    public void Click_Item3_InfoBtn()
    {
        StaticManager.PopUpUI.PopUp(items[2].ItemInfo);
    }
    public void Click_Item4_BuyBtn()
    {
        if (StaticManager.PlayerData.userData.Item4)
        {
            StaticManager.PopUpUI.PopUp("이미 보유중인 아이템입니다.");
        }
        else
        {
            BuyBtnListener(3);
        }
    }
    public void Click_Item4_InfoBtn()
    {
        StaticManager.PopUpUI.PopUp(items[3].ItemInfo);
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

    // 뒤로가기 버튼
    public void BackToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

}
