using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosion;
    public GameObject scoreToSpawn;

    public GameObject enemyToSpawn0;
    public GameObject enemyToSpawn1;
    public GameObject enemyToSpawn2;
    public GameObject enemyToSpawn3;
    public GameObject enemyToSpawn4;
    public GameObject enemyToSpawn5;

    public float waitTime = 3.0f;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "car")
        {
            Destroy(other.transform.gameObject);
            Score.score += 5;

            Vector3 killPos = AddRandomOffset(other.transform.position);
            Quaternion killRot = other.transform.rotation;

            StartCoroutine(SpawnEnemyAgain(killPos, killRot));
            Destroy(Instantiate(explosion, other.transform.position, other.transform.rotation), waitTime);
            Destroy(Instantiate(scoreToSpawn, other.transform.position, other.transform.rotation), waitTime);

        }
    }

    Vector3 AddRandomOffset(Vector3 position)
    {
        float offsetX = Random.Range(-2f, 2f);
        float offsetZ = Random.Range(-2f, 2f);

        return position + new Vector3(offsetX, 0, offsetZ);
    }

    IEnumerator SpawnEnemyAgain(Vector3 spawnPos, Quaternion spawnRot)
    {
        yield return new WaitForSeconds(waitTime);

        int randomIndex = Random.Range(0, 6);

        GameObject enemyToSpawn = null;

        switch (randomIndex)
        {
            case 0:
                enemyToSpawn = enemyToSpawn0;
                break;
            case 1:
                enemyToSpawn = enemyToSpawn1;
                break;
            case 2:
                enemyToSpawn = enemyToSpawn2;
                break;
            case 3:
                enemyToSpawn = enemyToSpawn3;
                break;
            case 4:
                enemyToSpawn = enemyToSpawn4;
                break;
            case 5:
                enemyToSpawn = enemyToSpawn5;
                break;
            default:
                break;
        }

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, spawnPos, spawnRot);
        }
    }
}
