using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 50; //회전속도
    [SerializeField] private float floatAmp = 2.0f; //폭
    [SerializeField] private float floatFrequency = 0.5f; //빈도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmp;
        transform.position = tempPos;
    }
}
