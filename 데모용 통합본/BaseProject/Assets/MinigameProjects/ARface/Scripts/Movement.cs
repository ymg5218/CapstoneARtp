using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Text result;
    [SerializeField]
    private Vector3 speed;

    void Update()
    {
        if (GameOver_face.gameStarted == true)
        {
            transform.position += speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "face")
        {
            ScoreManager.point += 1;
            gameObject.SetActive(false);
        }
    }
}

