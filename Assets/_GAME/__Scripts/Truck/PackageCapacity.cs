using System;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class PackageCapacity : MonoBehaviour
    {
        public TMP_Text capacity;
        public TruckController truckController;
        [SerializeField] private float offsetX = 1.5f;
        [SerializeField] private float offsetY = 1.5f;
        [SerializeField] private float offsetZ = 0.5f;

        private Camera _camera;

        public bool isCapacityOpen;
        private void Awake()
        {
            _camera = Camera.main;
            truckController = GetComponentInParent<TruckController>();
        }

        private void Start()
        {
            //capacity.color = Color.black;
            transform.parent = null;
            
            IsCapacityOpen();
        }
        
        private void IsCapacityOpen()
        {
                #if !UNITY_EDITOR
                
                isCapacityOpen = RemoteManager.Instance.GetCapacityUI();

                #endif
        }

        private void Update()
        {
            transform.LookAt(this.transform.position + _camera.transform.rotation * Vector3.forward,_camera.transform.rotation * Vector3.up);

            if (isCapacityOpen)
            {
                if (truckController.isBackward)
                {
                    transform.position = truckController.transform.position.AddY(offsetY).AddX(-offsetX).AddZ(-offsetZ);
                }
                else
                {
                    transform.position = truckController.transform.position.AddY(offsetY).AddX(offsetX).AddZ(offsetZ);
                }
            }
            else
            {
                if (truckController.isBackward)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    transform.position = truckController.transform.position.AddY(offsetY).AddX(offsetX).AddZ(offsetZ);
                }
            }
            
            
        }

        public void SetCapacityText(int totalCapacity, int currentCapacity)
        {
            capacity.text = currentCapacity + " / " + totalCapacity;
        }
    }
}