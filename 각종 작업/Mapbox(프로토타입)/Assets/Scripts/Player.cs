using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int point = 10;
    [SerializeField] private List<GameObject> m_lst = new List<GameObject>();
    private string path;

    public int Point //미션 하고 얻는 포인트
    {
        get { return point; }
    }

    public List<GameObject> M_lst
    {
        get { return m_lst; }
    }
    //Application.persistentDataPath = C:\Users\see48\AppData\LocalLow\DefaultCompany\Mapbox game
    private void Start()
    {
        path = Application.persistentDataPath + "/player.dat";
        LoadData();
    }

    public void addPoint(int point) //미션을 끝내면 포인트 추가됨
    {
        this.point += Mathf.Max(0, point);
        Save();
    }

    public void Add_Mission(GameObject mission)
    {
        if (mission)
        m_lst.Add(mission);
        Save();
    }

    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData(this);
        bf.Serialize(file, data);
        file.Close();
    }

    private void LoadData()
    { 
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            point = data.Point; //포인트만 저장하면 될거같음

            //플레이어 미션들
        }
        else
        {

        }
    }
    

}
