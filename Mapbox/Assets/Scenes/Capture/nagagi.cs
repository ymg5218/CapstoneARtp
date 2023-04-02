using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nagagi : MonoBehaviour
{
    public void NagagiBtnClicked()
    {
       LoadingSceneController.LoadScene(m_Contents.SCENE_WORLD);
    }

}
