using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISizer : MonoBehaviour
{
    private Camera cam;
    public Camera mainCamera=> Camera.main;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }


    void Update()
    {
        if (cam != null)
        {
            cam.orthographicSize = mainCamera.orthographicSize;
        }
    }
}
