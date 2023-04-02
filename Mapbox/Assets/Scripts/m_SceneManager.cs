using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class m_SceneManager : MonoBehaviour
{
    public abstract void playerTapped(GameObject player);
    public abstract void miniTapped(GameObject minigame);
}