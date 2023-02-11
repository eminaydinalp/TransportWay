using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Package;
using _GAME.__Scripts.Spawner;
using _GAME.__Scripts.Spline;
using DG.Tweening;
using Rentire.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME.__Scripts.Home
{
    public class PackageSpawner : RMonoBehaviour
    {
        [SerializeField] private HomeController _homeController;
        private SplinePointController _splinePointController;
        public List<GameObject> spawnedObjects;
        public List<Vector3> splinePoints = new List<Vector3>();

        public GameObject[] spawnObjects;
        private Vector3 _randomSpawnPosition;

        public int numberOfPowerUp;

        public int spawnTime;
        public int firstSpawnTime;

        public float xMin;
        public float xMax;
        public float zMin;
        public float zMax;
        
        public float distanceSpline;
        public float distancePackage;
        public float distanceHome;

        public float spawnYPos;


        public float xPositiveMax;
        public float xPositiveMin;
        
        public float xNegativeMax;
        public float xNegativeMin;

        public float cameraSizeMin;
        public float cameraSizeMax;

        private void Awake()
        {
            if (_homeController == null)
            {
                _homeController = GetComponent<HomeController>();
            }
            _splinePointController = _homeController.splinePointController;
        }

        private void OnValidate()
        {
            _homeController = GetComponent<HomeController>();
        }

        private void Start()
        {
            if (!UserPrefs.GetTutorial())
                if (TutorialManager.Instance.canSpawnCubes)
                {
                    if (TutorialManager.Instance.canSpawnCubes)
                    {
                        InvokeRepeating(nameof(CreateObject), firstSpawnTime, spawnTime);
                    }
                    InvokeRepeating(nameof(CreateObject), firstSpawnTime, spawnTime);
                }
        }
        
        public void StartSpawning()
        {
            InvokeRepeating(nameof(CreateObject), firstSpawnTime, spawnTime);
        }

        private void GetSplinePoints()
        {
            splinePoints.Clear();
            
            for (int i = 0; i < _splinePointController._splinePointList.Count; i++)
            {
                splinePoints.Add(_splinePointController._splinePointList[i].position);
            }
        }

        private void CreateObject()
        {
            if (spawnedObjects.Count >= numberOfPowerUp) return;

            GetSplinePoints();

            StartCoroutine(FindSpawn());
        }

        private IEnumerator FindSpawn()
        {
            if (spawnedObjects.Count >= numberOfPowerUp) yield break;
            
            _randomSpawnPosition = new Vector3(Random.Range(xMin, xMax), 
                SpawnManager.Instance.spawnYPos, 
                Random.Range(zMin, zMax));

            bool isDifferent = splinePoints.Any(i => Vector3.Distance(i, _randomSpawnPosition) < distanceSpline);

            if (isDifferent)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(FindSpawn());
                yield break;
            }

            bool isDifferent2 = SpawnManager.Instance.packageControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distancePackage);

            if (isDifferent2)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(FindSpawn());
                yield break;
            }

            bool isDifferent3 = SpawnManager.Instance.homeControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);

            if (isDifferent3)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(FindSpawn());
                yield break;
            }

            bool isDifferent4 = SpawnManager.Instance.targetHomeControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);

            if (isDifferent4)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(FindSpawn());
                yield break;
            }


            GameObject spawnedObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)],
                _randomSpawnPosition, Quaternion.identity);


            spawnedObject.transform.DOScale(1, 0.3f).SetEase(Ease.InOutCirc).From(0);

            PackageController packageController = spawnedObject.GetComponent<PackageController>();
            packageController.packageSpawner = this;

            SpawnManager.Instance.packageControllers.Add(packageController);

            spawnedObjects.Add(spawnedObject);
            
        }
        
        private void FindSpawnPointAndCreate()
        {
            // while (true)
            // {
            //     // xMax = Fmap(CameraZoomOut.Instance.cameraSize, cameraSizeMin, cameraSizeMax, xPositiveMin, xPositiveMax);
            //     // xMin = Fmap(CameraZoomOut.Instance.cameraSize, cameraSizeMin, cameraSizeMax, xNegativeMin, xNegativeMax);
            //
            //     _randomSpawnPosition = new Vector3(Random.Range(xMin, xMax), 
            //         SpawnManager.Instance.spawnYPos, 
            //         Random.Range(zMin, zMax));
            //
            //     bool isDifferent = splinePoints.Any(i => Vector3.Distance(i, _randomSpawnPosition) < distanceSpline);
            //
            //     if (isDifferent) continue;
            //
            //     bool isDifferent2 = SpawnManager.Instance.packageControllers.Any(i =>
            //         Vector3.Distance(i.transform.position, _randomSpawnPosition) < distancePackage);
            //
            //     if (isDifferent2) continue;
            //
            //     bool isDifferent3 = SpawnManager.Instance.homeControllers.Any(i =>
            //         Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);
            //
            //     if (isDifferent3) continue;
            //
            //     bool isDifferent4 = SpawnManager.Instance.targetHomeControllers.Any(i =>
            //         Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);
            //
            //     if (isDifferent4) continue;
            //
            //
            //     GameObject spawnedObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)],
            //         _randomSpawnPosition, Quaternion.identity);
            //
            //
            //     spawnedObject.transform.DOScale(1, 0.3f).SetEase(Ease.InOutCirc).From(0);
            //
            //     PackageController packageController = spawnedObject.GetComponent<PackageController>();
            //     packageController.packageSpawner = this;
            //
            //     SpawnManager.Instance.packageControllers.Add(packageController);
            //
            //     spawnedObjects.Add(spawnedObject);
            //
            //
            //     if (spawnedObjects.Count < numberOfPowerUp)
            //     {
            //         continue;
            //     }
            //
            //     break;
            // }
            
            _randomSpawnPosition = new Vector3(Random.Range(xMin, xMax), 
                SpawnManager.Instance.spawnYPos, 
                Random.Range(zMin, zMax));

            bool isDifferent = splinePoints.Any(i => Vector3.Distance(i, _randomSpawnPosition) < distanceSpline);

            if (isDifferent)
            {
                FindSpawnPointAndCreate();
                return;
            }

            bool isDifferent2 = SpawnManager.Instance.packageControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distancePackage);

            if (isDifferent2)
            {
                FindSpawnPointAndCreate();
                return;
            }

            bool isDifferent3 = SpawnManager.Instance.homeControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);

            if (isDifferent3)
            {
                FindSpawnPointAndCreate();
                return;
            }

            bool isDifferent4 = SpawnManager.Instance.targetHomeControllers.Any(i =>
                Vector3.Distance(i.transform.position, _randomSpawnPosition) < distanceHome);

            if (isDifferent4)
            {
                FindSpawnPointAndCreate();
                return;
            }


            GameObject spawnedObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)],
                _randomSpawnPosition, Quaternion.identity);


            spawnedObject.transform.DOScale(1, 0.3f).SetEase(Ease.InOutCirc).From(0);

            PackageController packageController = spawnedObject.GetComponent<PackageController>();
            packageController.packageSpawner = this;

            SpawnManager.Instance.packageControllers.Add(packageController);

            spawnedObjects.Add(spawnedObject);


            if (spawnedObjects.Count < numberOfPowerUp)
            {
                FindSpawnPointAndCreate();
            }

           
        }
        
    }
}