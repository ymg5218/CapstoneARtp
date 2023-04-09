using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class worldSceneManager : m_SceneManager
{
    public override void playerTapped(GameObject player)
    {

    }

    public override void miniTapped(GameObject minigame)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(minigame);
        //additive모드로 불러오면 이상한 화면됨
       // SceneManager.LoadScene(m_Contents.SCENE_CAPTURE, LoadSceneMode.Additive);
        SceneTransManager.Instance.GoToScene(m_Contents.SCENE_CAPTURE, list);
    }
}
