using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Spawner;
using _GAME.__Scripts.Stack;
using _GAME.__Scripts.Truck;
using DG.Tweening;
using Rentire.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _GAME.__Scripts.Package
{
    public class PackageController : MonoBehaviour
    {
        public TruckColor truckColor;
        public int numberOfPackage;
        public List<PackageBox> packages = new List<PackageBox>();
        public List<Vector3> targetPositions = new List<Vector3>();
        public bool isMove;

        public StackController parentStack;
        
        [Header("Stack Transform Information")] 
        [SerializeField] private PackageBox packageBoxPrefab;
        
        [SerializeField] private float xFirst;
        [SerializeField] private float zFirst;
        [SerializeField] private int columnSize;
        [SerializeField] private int lineSize;
        
        [Range(0, 2)] [SerializeField] private float xGap;
        [Range(0, 2)] [SerializeField] private float zGap;
        
        [SerializeField] private int floorCount; 
        [SerializeField] private int floorOffset; 
        [SerializeField] private float floorRate;

        public PackageSpawner packageSpawner;

        private Tween _delayTween;
        public GameObject packageTutorialText;
        private bool _willDestroy = false;
        
        private void Start()
        {
            //SetInitialPosition();
            numberOfPackage = packages.Count;
            if (numberOfPackage > 0)
            {
                OrderPackages();
            }
        }
        
        private void OrderPackages()
        {
            packages = packages.OrderByDescending(x => x.transform.position.z).ToList();
            packages = packages.OrderBy(x => x.transform.position.x).ToList();
            //packages.Reverse();
        }

        // private void OnValidate()
        // {
        //     ResetPackage();
        //     SetInitialPosition();
        // }

        #if UNITY_EDITOR
        [Button]
        private void SetInitialPosition()
        {
            for (int j = 0; j < lineSize; j++)
            {
                for (int i = 0; i < columnSize * floorCount; i++)
                {
                    PackageBox newPackageBox = PrefabUtility.InstantiatePrefab(packageBoxPrefab, transform) as PackageBox;
                    //Package newPackage = Instantiate(packagePrefab, transform);
                    newPackageBox.transform.position = Vector3.zero;
                    newPackageBox.transform.position = new Vector3(xFirst + (xGap * (i % columnSize)),
                        (j + floorOffset) * floorRate, zFirst + (zGap * (i / columnSize)));
                    
                    
                    packages.Add(newPackageBox);
                }
            }
        }
        
        #endif

        [Button]
        private void ResetPackage()
        {
            int count = packages.Count;
            
            for (int i = 0; i < count; i++)
            {
                PackageBox destroyedObject = packages[0];
                packages.Remove(destroyedObject);
                DestroyImmediate(destroyedObject.gameObject);
            }
        }

        public IEnumerator MovePackages(int packageCount, TruckController truckController, float durationOfPackage)
        {
            _delayTween = DOVirtual.DelayedCall(1f, SetNumberOfPackage);
            
            numberOfPackage -= packageCount;
            
            for (int i = 0; i < packageCount; i++)
            {
                if (truckController.isTruckMerge || packages.Count <= 0)
                {
                    _delayTween.Kill();
                    SetNumberOfPackage();
                    yield break;
                }
                packages[^1].Move(targetPositions[0], parentStack, durationOfPackage);
                packages.Remove(packages[^1]);
                targetPositions.Remove(targetPositions[0]);

                yield return new WaitForSeconds(0.02f);

                if (i == packageCount - 1)
                {
                    SetNumberOfPackage();
                }
            }
        }

        private void SetNumberOfPackage()
        {
            numberOfPackage = packages.Count;
            
            if (numberOfPackage <= 0 && !_willDestroy)
            {
                _willDestroy = true;
                packageSpawner.spawnedObjects.Remove(gameObject);
                SpawnManager.Instance.packageControllers.Remove(this);
                if (UserPrefs.GetTutorial() && TutorialManager.Instance.CurrentTutorialStep <= 3)
                {
                    TutorialManager.Instance.InvokeNextTutorialStep();
                }
                gameObject.SetActive(false);
                
                Destroy(gameObject, 2f);
            }
        }
    }
    

}