using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_m : MonoBehaviour
{
    [SerializeField] Text pointText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menu1;
    [SerializeField] private GameObject menu2;



    void Update()
    {
        updatePoint();
    }

    public void updatePoint()
    {
        pointText.text = GameManager.Instance.CurrentPlayer.Point.ToString();
    }

    public void btn_click()
    {
        LoadingSceneController.LoadScene(m_Contents.SCENE_CAPTURE);
    }
    public void btn_click2()
    {
        LoadingSceneController.LoadScene(m_Contents.SCENE_BALLOON);
    }
    public void exitMenu()
    {
        menu.SetActive(false);
    }
    public void exitMenu2()
    {
        menu1.SetActive(false);
    }
    public void exitMenu3()
    {
        menu2.SetActive(false);
    }
    public void btn_click3()
    {
        LoadingSceneController.LoadScene(m_Contents.SCENE_CAR);
    }
}
