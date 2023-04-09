using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landmarkManager : MonoBehaviour
{
public float lat1 = 37.341500f;
public float lon1 = 126.732382f;

public float lat2 = 37.341144f;
public float lon2 = 126.732382f;


        void Haver()
        {
            float R = 6371; // 지구 반지름이 6371km이므로
            float omega1 = ((lat1 / 180) * Mathf.PI);
            float omega2 = ((lat2 / 180) * Mathf.PI);
            float variacionomega1 = (((lat2 - lat1) / 180) * Mathf.PI);
            float variacionomega2 = (((lon2 - lon1) / 180) * Mathf.PI);
            float a = Mathf.Sin(variacionomega1 / 2) * Mathf.Sin(variacionomega1 / 2) +
                        Mathf.Cos(omega1) * Mathf.Cos(omega2) *
                        Mathf.Sin(variacionomega2 / 2) * Mathf.Sin(variacionomega2 / 2);
            float c = 2 * Mathf.Asin(Mathf.Sqrt(a));

            float d = R * c;
            Debug.Log(d*1000 + "m");
        }
        void Start()
        {
        Invoke("Haver", 2);
        }
    
}
