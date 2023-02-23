using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private void OnMouseDown()
    {
        m_SceneManager[] managers = FindObjectsOfType<m_SceneManager>();
        foreach(m_SceneManager m_SceneManager in managers)
        {
            if(m_SceneManager.gameObject.activeSelf)
            {
                m_SceneManager.miniTapped(gameObject);
            }
        }
    }
    //collision일때
    /*private void OnCollisionEnter(Collision collision)
    {
        m_SceneManager[] managers = FindObjectsOfType<m_SceneManager>();
        foreach (m_SceneManager m_SceneManager in managers)
        {
            if (m_SceneManager.gameObject.activeSelf)
            {
                m_SceneManager.m_Collision(gameObject, collision);
            }
        }
    }*/
}
