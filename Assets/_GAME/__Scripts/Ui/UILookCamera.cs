using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILookCamera : MonoBehaviour
{
    public TMP_Text capacity;
    public GameObject parentGO;
    [SerializeField] private float offsetX = 1.5f;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private float offsetZ = 0.5f;
    
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(this.transform.position + _camera.transform.rotation * Vector3.forward,_camera.transform.rotation * Vector3.up);
        transform.position = parentGO.transform.position.AddY(offsetY).AddX(offsetX).AddZ(offsetZ);
    }
}
