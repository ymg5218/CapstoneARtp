//--------------------------------------------------------------
// 파일명: BalloonSpawner.cs
// 작성자: 박영훈
// 작성일: (-)
// 설명: 
// - 화면에 풍선을 지속적으로 깔아주는 클래스.
// 수정 :
// - 이수민(2023-04-18) : 코드 문서화, 마이너한 설계 변경
//--------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBalloons : MonoBehaviour {
    public GameObject[] Balloons;
    public Camera arCamera;
    public float spawnDistance = 5f;
    public float spawnAngle = 45f;

    public void StartSpawning() {
        StartCoroutine(SpawnBalloon());
    }

    IEnumerator SpawnBalloon() {
        yield return new WaitForSeconds(3); //스폰대기시간

        for (int i = 0; i < 3; i++) {
            int random = Random.Range(0, Balloons.Length);
            Vector3 spawnPos = GetRandomSpawnPosition();
            Instantiate(Balloons[random], spawnPos, Quaternion.identity);
        }
        StartCoroutine(SpawnBalloon());
    }

    Vector3 GetRandomSpawnPosition() {
        Vector3 cameraPos = arCamera.transform.position;
        Quaternion cameraRot = arCamera.transform.rotation;
        float angle = Random.Range(-spawnAngle, spawnAngle);
        float distance = Random.Range(0, spawnDistance);
        Vector3 spawnPos = cameraPos + cameraRot * Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        return spawnPos;
    }
}
