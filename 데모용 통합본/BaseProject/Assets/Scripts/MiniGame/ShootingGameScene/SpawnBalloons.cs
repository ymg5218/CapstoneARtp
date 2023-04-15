using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBalloons : MonoBehaviour
{
    public GameObject[] Balloons;
    public Camera arCamera;
    public float spawnDistance = 5f; // 카메라에서 얼마나 떨어져 있는지
    public float spawnAngle = 45f; // 카메라의 각도에 따라 스폰

    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(3); //스폰대기시간

        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, Balloons.Length);
            Vector3 spawnPos = GetRandomSpawnPosition();
            Instantiate(Balloons[random], spawnPos, Quaternion.identity);
        }

        StartCoroutine(StartSpawning());
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 cameraPos = arCamera.transform.position;
        Quaternion cameraRot = arCamera.transform.rotation;
        float angle = Random.Range(-spawnAngle, spawnAngle);
        float distance = Random.Range(0, spawnDistance);
        Vector3 spawnPos = cameraPos + cameraRot * Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        return spawnPos;
    }
}
