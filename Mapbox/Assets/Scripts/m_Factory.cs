using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_Factory : Singleton<m_Factory>
{
    [SerializeField] private Mission[] availableMission;
    [SerializeField] private float waitTime = 60.0f; //리스폰 시간
    [SerializeField] private int startingMissions = 5; //미션수
    [SerializeField] private float minRange = 3.0f; // 최소 범위
    [SerializeField] private float maxRange = 10.0f; //최대범위

    private List<Mission> liveMissions = new List<Mission>();
    private Mission selectMission;
    private Player player;

    public List<Mission> LiveMissions
    {
        get { return liveMissions; }
    }
    public Mission SelectMission
    {
        get { return selectMission; }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        player = GameManager.Instance.CurrentPlayer;

        for(int i = 0; i<startingMissions; i++)
        {
            InstantiateMission();
        }
        StartCoroutine(GenerateMission());
    }


    private IEnumerator GenerateMission()
    {
        while(true)
        {
            InstantiateMission();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateMission()
    {
        int index = Random.Range(0, availableMission.Length);
        float x = player.transform.position.x + GenerateRange(); //x축이 항상 달라지게 생성하자
        float z = player.transform.position.z + GenerateRange(); //z축이 항상 달라지게 생성하자
        float y = player.transform.position.y; //y축은 그대로. 바닥을 뚫고 들어가면 안 되기 때문에... 
        liveMissions.Add(Instantiate(availableMission[index], new Vector3(x, y, z), Quaternion.identity));
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }
}