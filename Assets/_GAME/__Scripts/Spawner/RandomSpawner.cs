using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Spline;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME.__Scripts.Spawner
{
    public class RandomSpawner : MonoBehaviour
    {
        public List<Vector3> splinePoints = new List<Vector3>();
        public SplinePointController[] _splinePointControllers;
        public GameObject[] spawnObjects;
        public Vector3 randomSpawnPosition;

        public int numberOfPowerUp;

        public List<GameObject> spawnedObjects = new List<GameObject>();

        public int spawnTime;

        [SerializeField] private float spawnYPos;
        

        private void Start()
        {
            InvokeRepeating(nameof(CreateObject), spawnTime, spawnTime);
        }

        private void GetSplinePoints()
        {
            splinePoints.Clear();

            _splinePointControllers = FindObjectsOfType<SplinePointController>();

            for (int i = 0; i < _splinePointControllers.Length; i++)
            {
                for (int j = 0; j < _splinePointControllers[i]._splinePointList.Count; j++)
                {
                    splinePoints.Add(_splinePointControllers[i]._splinePointList[j].position);
                }
            }
        }

        private void CreateObject()
        {
            if (spawnedObjects.Count >= numberOfPowerUp) return;
            GetSplinePoints();

            FindSpawnPointAndCreate();
        }

        private void FindSpawnPointAndCreate()
        {
            randomSpawnPosition = new Vector3(Random.Range(-4.5f, 4.5f), spawnYPos, Random.Range(2, 10));


            bool isDifferent1 = splinePoints.Any(i => Vector3.Distance(i, randomSpawnPosition) < 1);

            bool isDifferent2 = SpawnManager.Instance.packageControllers.Any(i =>
                Vector3.Distance(i.transform.position, randomSpawnPosition) < 2);
            bool isDifferent3 =
                spawnedObjects.Any(i => Vector3.Distance(i.transform.position, randomSpawnPosition) < 2);


            if (isDifferent1 || isDifferent2 || isDifferent3)
            {
                FindSpawnPointAndCreate();
            }
            else
            {
                GameObject spawnedObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)],
                    randomSpawnPosition, Quaternion.identity);
                
                spawnedObject.transform.DOScale(1,0.2f).From(0).SetEase(Ease.InOutCirc);

                PowerUpBase plusObject = spawnedObject.GetComponent<PowerUpBase>();

                SpawnManager.Instance.plusObjects.Add(plusObject);

                spawnedObjects.Add(spawnedObject);


                if (spawnedObjects.Count < numberOfPowerUp)
                {
                    FindSpawnPointAndCreate();
                }
            }
        }
    }
}