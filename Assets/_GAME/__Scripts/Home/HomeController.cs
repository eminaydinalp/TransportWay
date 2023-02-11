using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Incremental;
using _GAME.__Scripts.Spawner;
using _GAME.__Scripts.Spline;
using _GAME.__Scripts.Truck;
using DG.Tweening;
using UnityEngine;

namespace _GAME.__Scripts.Home
{
    public class HomeController : MonoBehaviour
    {
        public int levelNo;
        public SplinePointController splinePointController;
        public TruckController truckController;
        public List<TruckController> currentTrucks = new List<TruckController>();
        public Queue<TruckController> currentTrucksQueue = new Queue<TruckController>();
        public int truckCount;

        public bool isFirst;

        public float spawnTime;

        private IncrementalBase _addCarIncremental;

        public string levelTruckPref;

        public TruckColor truckColor;

        public PackageSpawner packageSpawner;

        public RemoveSpline removeSpline;
        
        public SpriteRenderer outlineSprite;
        public SpriteRenderer innerSprite;
        public MeshRenderer splineMeshRenderer;
        public GameObject targetSphere;
        private void Start()
        {
            _addCarIncremental = FindObjectOfType<AddCarIncremental>();

            truckCount = LocalPrefs.GetInt(levelTruckPref, truckCount);

            CreateTruckInitial();
            
            SpawnManager.Instance.SetSpawnValues(packageSpawner.xMin, packageSpawner.xMax, packageSpawner.zMin, packageSpawner.zMax);
        }

        private void OnEnable()
        {
            EventManager.OnSplineReset += CancelMoveTruck;
            EventManager.OnSplineReset += ResetPos;
            EventManager.OnSplineReset += ResetSpline;
            EventManager.OnSplineReset += RemoveTrucks;
            EventManager.OnAddNewTruck += HandleNewTruck;
        }

        private void OnDisable()
        {
            EventManager.OnSplineReset -= CancelMoveTruck;
            EventManager.OnSplineReset -= ResetPos;
            EventManager.OnSplineReset -= ResetSpline;
            EventManager.OnSplineReset -= RemoveTrucks;
            EventManager.OnAddNewTruck -= HandleNewTruck;
        }

        private void HandleNewTruck()
        {
            if (!isFirst) return;
            if(!_addCarIncremental.RequireMoney())return;

            Debug.Log("Add Car");
            CreateNewTruck();
            
            SpawnAndShowFloatingText(transform.position + new Vector3(0,2,1));
            truckCount++;
            
        }

        public void CreateTruckInitial()
        {
            for (int i = 0; i < truckCount; i++)
            {
                CreateNewTruck();
            }
        }

        private void SpawnAndShowFloatingText(Vector3 spawnPos)
        {
            var floatingText = PoolManager.Instance.Spawn_Object(PoolsEnum.PlusOne, spawnPos, Quaternion.identity , 1);
            floatingText.transform.DOMoveZ(floatingText.transform.position.z + 2, 0.8f);
            floatingText.transform.DOScale(0, 0.7f);
        }

        public void CreateNewTruck()
        {
            TruckController newTruckMovement = Instantiate(truckController);
            transform.DOPunchScale(0.2f * Vector3.one, 0.2f, 0, 0.1f).SetDelay(0.1f);
            newTruckMovement.homeController = this;
            newTruckMovement.transform.position = splinePointController.baseSplinePoints[0].position;
            
            
            currentTrucks.Add(newTruckMovement);
            currentTrucksQueue.Enqueue(newTruckMovement);

            newTruckMovement.gameObject.SetActive(false);
            
            LocalPrefs.SetInt(levelTruckPref, currentTrucks.Count);
        }

        public void SendTruckHomeBack(TruckController triggerTruckController)
        {
            triggerTruckController.isBackward = false;
            currentTrucksQueue.Enqueue(triggerTruckController);
            currentTrucks.Add(triggerTruckController);
        }

        public void InvokeMoveTruck()
        {
            ResetQueue();

            InvokeRepeating(nameof(MoveTruck), 0, spawnTime);
        }

        private void ResetQueue()
        {
            currentTrucksQueue.Clear();

            for (int i = 0; i < currentTrucks.Count; i++)
            {
                currentTrucksQueue.Enqueue(currentTrucks[i]);
            }
        }

        public void CancelMoveTruck()
        {
            CancelInvoke(nameof(MoveTruck));
        }

        public void ResetPos()
        {
            for (int i = 0; i < currentTrucks.Count; i++)
            {
                currentTrucks[i].transform.position = splinePointController.baseSplinePoints[0].position;
            }
        }

        public void MoveTruck()
        {
            if (currentTrucksQueue.Count <= 0 || !splinePointController.isEndPoint ||
                MergeManager.Instance.isMergeStart) return;

            TruckController truckMovementDeque = currentTrucksQueue.Dequeue();

            truckMovementDeque.gameObject.SetActive(true);
            truckMovementDeque.packageCapacity.gameObject.SetActive(true);
            truckMovementDeque.truckMovement.SplineRestart();
            truckMovementDeque.transform.position = splinePointController.baseSplinePoints[0].position;
            truckMovementDeque.truckMovement.SetSplineComputer(splinePointController.currentSpline);
            truckMovementDeque.isBackward = false;
        }

        public void RemoveFromQueue(TruckController truckController)
        {
            currentTrucksQueue.Enqueue(truckController);
            truckController.packageCapacity.gameObject.SetActive(false);
            truckController.isBackward = false;
            truckController.gameObject.SetActive(false);
        }

        public void ResetSpline()
        {
            splinePointController.ResetSpline();
        }

        public void RemoveTrucks()
        {
            for (int i = 0; i < currentTrucks.Count; i++)
            {
                currentTrucks[i].gameObject.SetActive(false);
                currentTrucks[i].packageCapacity.gameObject.SetActive(false);
            }
        }

        public void MergeControl()
        {
            for (int i = 0; i < truckController.truckSo.needCarForMerge; i++)
            {
                TruckController truckMovementMerged = currentTrucks[^1];
                if (currentTrucksQueue.Contains(truckMovementMerged))
                {
                    currentTrucksQueue =
                        new Queue<TruckController>(currentTrucksQueue.Where(x => x != truckMovementMerged));
                }
                
                truckMovementMerged.gameObject.SetActive(true);
                truckMovementMerged.packageCapacity.gameObject.SetActive(false);
                currentTrucks.Remove(truckMovementMerged);
                truckCount--;
                LocalPrefs.SetInt(levelTruckPref, currentTrucks.Count);

                MergeManager.Instance.Merge(truckMovementMerged);
            }

            MergeManager.Instance.mergeIndex = 0;
        }
        
    }
}