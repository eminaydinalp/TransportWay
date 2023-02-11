using _GAME.__Scripts.Truck;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Spawner
{
    public abstract class PowerUpBase : MonoBehaviour
    {
        public TMP_Text restText;
        public int totalRest;
        public int currentRest;
        [SerializeField] protected int moneyAmount;
        public int numberOfRest;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            restText.text = currentRest + " / " + totalRest;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TruckController truckController = other.GetComponent<TruckController>();
                //if(truckController.isBackward) return;
                numberOfRest--;
                currentRest++;
                
                
                restText.text = currentRest + " / " + totalRest;
                
                if (numberOfRest <= 0)
                {

                    SpawnManager.Instance.randomSpawner.spawnedObjects.Remove(gameObject);
                    SpawnManager.Instance.plusObjects.Remove(this);
                    
                    Destroy(gameObject);
                }
                
                IncreaseProcess(truckController);
            }
        }

        private void Update()
        {
            restText.transform.LookAt(this.transform.position + _camera.transform.rotation * Vector3.forward,_camera.transform.rotation * Vector3.up);
        }

        protected abstract void IncreaseProcess(TruckController truckController);


    }
}