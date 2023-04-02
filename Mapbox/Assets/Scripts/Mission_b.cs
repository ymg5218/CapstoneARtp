using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_b : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject menu1;

    private void Start()
    {
        menu1.SetActive(false);
    }

    private void OnMouseDown()
    {
        menu1.SetActive(!menu1.activeSelf);
        canvas.sortingOrder = 1;
    }
}
