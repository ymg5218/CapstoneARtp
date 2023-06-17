using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_spider : MonoBehaviour
{
    private bool timer = false;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject GO;

    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameStartButton;
    [SerializeField] float maxTime = 30f;
    [SerializeField] Text score;
    float timeLeft;
    Image timerBar;
    public static bool gameStarted;

    void Start()
    {
        gameStarted = false;

        A.SetActive(false);
        B.SetActive(false);
        C.SetActive(false);
        GO.SetActive(false);
        gameOverText.SetActive(false);
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    public void StartGame()
    {
        gameStartButton.SetActive(false);
        Countdown(); 
    }

    void Countdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        C.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        C.SetActive(false);
        B.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        B.SetActive(false);
        A.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        A.SetActive(false);
        GO.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        GO.SetActive(false);
        gameStarted = true;

        timer = true;

       yield return new WaitForSeconds(30f);
        
        GameOverSequence();
    }

    void Update()
    {
        if (timer == true)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;
            }
        }
    }

    void GameOverSequence()
    {
        gameOverText.SetActive(true);
        gameStarted = false;
    }

    //--------------------------------------------------------------
    // 메소드명 : ExitButton()
    // 설명 : 
    // - 나가기 버튼 관련 작업.
    // - 누르면 동작해요!.
    //--------------------------------------------------------------
    public void ExitButton(){
        if (score != null)
        {
            if (int.TryParse(score.text, out int value))
            {
                StaticManager.Matching.IncreaseTeamScore(StaticManager.PlayerData.userData.nickname, value);
            }
            else
            {
                Debug.LogError("인트값 변환 실패. 뭔가 잘못 끼어있는거 같은데요?");
            }
        }
        else {
            Debug.LogError("점수 오브젝트 지정을 안해놨어요. 빠가사리!");
        }

        SceneLoader.LoadScene("MainScene");
    }
}
