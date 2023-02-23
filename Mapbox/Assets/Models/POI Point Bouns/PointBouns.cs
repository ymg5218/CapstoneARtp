using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBouns : MonoBehaviour
{
    [SerializeField] private int bonus = 10;
    void Start()
    {
        Invoke("distance", 2);
    }
    private void OnMouseDown()
    {
        GameManager.Instance.CurrentPlayer.addPoint(bonus);
        Debug.Log(bonus);
        Destroy(gameObject);
        SceneTransManager.Instance.GoToScene(m_Contents.SCENE_BALLOON, new List<GameObject>());
    }
    void distance()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.CurrentPlayer.transform.position);
        Debug.Log(distance);
        if(distance<10f)
        {
            Debug.Log("굉장히 가깝습니다");
        }
    }
}
