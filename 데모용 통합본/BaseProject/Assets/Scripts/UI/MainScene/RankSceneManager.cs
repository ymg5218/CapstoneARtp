using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankSceneManager : MonoBehaviour
{
    public GameObject Rankpopup;
    private Text RankTitle;
    private Text RankInfo;


    public void Click_Rank1Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("종합랭킹","종합랭킹 목록들");
    }
    public void Click_Rank2Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("자동차 게임 랭킹", "자동차 게임 랭킹 목록들");
    }
    public void Click_Rank3Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("슈팅 게임 랭킹", "슈팅 게임 랭킹 목록들");
    }
    public void Click_Rank4Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("농구 게임 랭킹", "농구 게임 랭킹 목록들");
    }
    public void Click_Rank5Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("(임시)미니게임1 랭킹", "(임시)미니게임1 랭킹 목록들");
    }
    public void Click_Rank6Btn()
    {
        Rankpopup.SetActive(true);
        SetRankPopupUI("(임시)미니게임2 랭킹", "(임시)미니게임2 랭킹 목록들");
    }
    public void SetRankPopupUI(string _RankTitle, string _RankInfo)
    {
        RankTitle = GameObject.Find("RankPopupTitle").GetComponent<Text>();
        RankInfo = GameObject.Find("Rank").GetComponent<Text>();
        RankTitle.text = _RankTitle;
        RankInfo.text = _RankInfo;
    }
    public void BackToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
