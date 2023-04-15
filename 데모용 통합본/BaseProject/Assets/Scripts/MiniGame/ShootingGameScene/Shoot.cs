using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shoot : MonoBehaviour
{
    public int goal_point = 0;
    public Text result;
    public GameObject arCamera;
    public GameObject smoke;

    public void Shoot_Balloon()
    {
        RaycastHit hit;

        if(Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
        {
            if(hit.transform.tag == "Balloon")
            {
                Destroy(hit.transform.gameObject);

                Instantiate(smoke, hit.point, Quaternion.LookRotation(hit.normal));
                goal_point += 1;
                result.text = goal_point.ToString();
            }
        }
    }
}
