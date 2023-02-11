using System;
using _GAME.__Scripts.Spline;
using _GAME.__Scripts.Truck;
using UnityEngine;

namespace _GAME.__Scripts.Home
{
    public class HomeBack : MonoBehaviour
    {
        [SerializeField] private HomeController _homeController;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TruckController truckController = other.GetComponent<TruckController>();
                
                if(!truckController.isBackward) return;
                _homeController.RemoveFromQueue(truckController);
            }
        }
    }
}
