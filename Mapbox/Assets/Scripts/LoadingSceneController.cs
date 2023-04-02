using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{ 
    static string nextScene;
    [SerializeField] Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }


    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; //자동으로 불러오지 못하게 함. 왜냐면 로딩이 너무 빠르면 기껏만든 로딩씬이 의미 없어지기 때문.

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f )
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }    
        }
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
}
