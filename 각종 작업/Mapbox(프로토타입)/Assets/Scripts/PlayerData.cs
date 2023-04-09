using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    private int point;
    private List<MinigameData> missions; //미니게임 데이터

    public int Point
    {
        get
        { return point; }
    }

    public List<MinigameData> Missions { get { return missions; } }

    public PlayerData(Player player) //플레이어 데이터를 저장. 미니게임 끝나고 초기화되면 안되니까
    {
        point = player.Point;
        //missions = player.M_lst;

        foreach (GameObject missionObject in player.M_lst) //차례차례 데이터를 저장
        {
            Mission mission = missionObject.GetComponent<Mission>();
            if(mission != null)
            {
                MinigameData data = new MinigameData(mission);
                missions.Add(data);
            }
        }
    }
}
