using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float resetTime = 3.0f;
    public Text result;
    public float captureRate = 0.5f;
    public int goal_point=0;
    Rigidbody rb;
    bool isReady = true;
    Vector2 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        result.text = "";
    }

    void Update()
    {
        if(!isReady)
        {
            return;
        }

        SetBallPosition(Camera.main.transform);

        if(Input.touchCount > 0 && isReady)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                float dragDistance = touch.position.y - startPos.y;

                Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

                rb.isKinematic = false;
                isReady = false;

                //직선 발사
                //rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                //포물선 발사
                rb.AddForce((throwAngle * dragDistance / 10.0f) + Vector3.up * dragDistance);

                Invoke("ResetBall", resetTime);

            }
        }

    }

    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;

        transform.position = anchor.position + offset;
    }

    void ResetBall()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        isReady = true;

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isReady)
        {
            return;
        }
        if (collision.gameObject.tag == "Monster")
        {
            goal_point += 1;
            result.text = goal_point.ToString();
            gameObject.SetActive(false);
        }
    }
}
