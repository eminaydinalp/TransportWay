using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Incremental;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class MergeManager : Singleton<MergeManager>
    {
        [SerializeField] private Transform mergePosition;

        [SerializeField] private GameObject[] mergedObject;

        [SerializeField] private Vector3[] truckPositions;

        [SerializeField] private IncrementalBase _mergeIncremental;

        public int mergeIndex;
        public int mergedObjectIndex;

        public bool isMerge;
        public bool isMergeStart;

        public Transform targetHome;

        public HomeController targetHomeController;

        public List<HomeController> homeControllers = new List<HomeController>();

        public float truckScale;

        private void Start()
        {
            StartCoroutine(FindHomeControllers());
        }

        private void OnEnable()
        {
            EventManager.OnMergeButtonClick += MergeControl;
        }

        private void OnDisable()
        {
            EventManager.OnMergeButtonClick -= MergeControl;
        }

        public void Merge(TruckController truck)
        {
            gameObject.SetLayer(LayerMask.NameToLayer("TruckNonCollision"));
            truck.packageCapacity.gameObject.SetActive(false);
            truck.boxCollider.enabled = false;
            truck.stackController.StackReset();
            truck.isTruckMerge = true;
            truck.CloseSpline();
            truck.transform.parent = mergePosition;
            //truck.transform.parent = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );
            truck.transform.DOScale(Vector3.one * truckScale, 0.5f);
            
            truck.transform.DOLocalMove(truckPositions[mergeIndex], 0.5f)
                .OnComplete(() =>
                {
                    truck.transform.DOLocalMove(Vector3.zero, 0.5f)
                        .OnComplete(() =>
                        {
                            CreateMergedObject();
                            truck.gameObject.SetActive(false);
                            Destroy(truck.gameObject, 3);
                            isMerge = true;
                        });
                });

            mergeIndex++;
        }
        

        private void CreateMergedObject()
        {
            if (!isMerge)
            {
                GameObject mergedObjectClone = Instantiate(mergedObject[mergedObjectIndex], mergePosition.position,
                    Quaternion.identity);
                
                FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
                
                mergedObjectClone.transform.localScale = Vector3.zero;
                mergedObjectClone.transform.DOScale(Vector3.one * truckScale, 0.5f)
                    .OnComplete(() =>
                    {
                        mergedObjectClone.transform.DOLocalMove(targetHome.position, 0.5f)
                            .OnComplete(() =>
                            {
                                mergedObjectClone.SetActive(false);
                                Destroy(mergedObjectClone, 3);
                                targetHomeController.CreateNewTruck();
                                targetHomeController.truckCount++;
                                isMerge = false;
                                isMergeStart = false;
                                EventManager.Instance.InvokeOnMergeFinish();
                            });
                    });
            }
        }

        private void MergeControl()
        {
            if (!IsMergeAble()) return;
            
            for (int i = 0; i < homeControllers.Count; i++)
            {
                if (homeControllers[i].currentTrucks.Count >= homeControllers[i].truckController.truckSo.needCarForMerge 
                    && homeControllers.IndexOf(homeControllers[i]) != homeControllers.IndexOf(homeControllers[^1]))
                {
                    if (!_mergeIncremental.RequireMoney())
                    {
                        return;
                    }

                    isMergeStart = true;
                    
                    targetHome = homeControllers[i + 1].transform;
                    targetHomeController = homeControllers[i + 1];

                    homeControllers[i].MergeControl();

                    mergedObjectIndex = i;

                    break;
                }
            }
        }

        public bool IsMergeAble()
        {
            var homeLockController = homeControllers.FirstOrDefault(x => x.truckCount >= x.truckController.truckSo.needCarForMerge 
                                                                         && homeControllers.IndexOf(x) != homeControllers.IndexOf(homeControllers[^1]));

            if (homeLockController != null && homeControllers.Count > 1)
            {
                return true;
            }

            return false;
        }


        private IEnumerator FindHomeControllers()
        {
            yield return new WaitForSeconds(0.5f);

            homeControllers = FindObjectsOfType<HomeController>().ToList();

            homeControllers = homeControllers.OrderBy(x => x.levelNo).ToList();
        }
    }
}