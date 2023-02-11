using _GAME.__Scripts.Home;
using _GAME.__Scripts.Package;
using _GAME.__Scripts.Spline;
using _GAME.__Scripts.Stack;
using DG.Tweening;
using Dreamteck.Splines;
using Rentire.Utils;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class TruckController : MonoBehaviour
    {
        public TruckSo truckSo;

        public SplineFollower splineFollower;

        public TruckMovement truckMovement;
        public TruckColor truckColor;
        public int increaseMoney;

        public bool isBackward;
        public bool isTruckMerge;

        public HomeController homeController;

        public StackController stackController;

        public int currentPackageCount;

        public TruckMoneyText truckMoneyText;

        public FullText fullText;

        public PackageCapacity packageCapacity;

        public TruckCrash truckCrash;

        public TruckAngle truckAngle;

        public BoxCollider boxCollider;

        public bool isCrash;

        public bool isTakeAble;

        private void Awake()
        {
            truckMovement = new TruckMovement(splineFollower);
        }

        private void ScaleUpPackageCapacity()
        {
            packageCapacity.gameObject.transform.DOScale(packageCapacity.transform.localScale + (0.2f * Vector3.one),
                0.1f);
        }

        private void OnEnable()
        {
            HomeLockManager.OnHomeUnlocked += ScaleUpPackageCapacity;
        }

        private void OnDisable()
        {
            //HomeLockManager.OnHomeUnlocked -= ScaleUpPackageCapacity;
        }

        private void Start()
        {
            packageCapacity.SetCapacityText(truckSo.capacityOfPackage, currentPackageCount);
            if (UserPrefs.GetMaxHomeLevel() > 0)
            {
                packageCapacity.gameObject.transform.DOScale(
                    packageCapacity.transform.localScale + ((Vector3.one * UserPrefs.GetMaxHomeLevel()) * 0.2f), 0.1f);
            }

            IsTakeAble();

        }

        private void IsTakeAble()
        {
                #if !UNITY_EDITOR
                
                isTakeAble = RemoteManager.Instance.GetCanTakeAble();

                #endif
        }

        private void FixedUpdate()
        {
            truckMovement.Move();

            //transform.Translate(transform.forward * Time.deltaTime * 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!truckCrash.canCrash) return;

                TruckController truckController = other.GetComponent<TruckController>();
                //if(truckController.homeController.levelTruckPref == homeController.levelTruckPref || isCrash || truckController.isCrash || MergeManager.Instance.isMergeStart) return;
                //
                // Debug.Log("Crashh Player");
                // isCrash = true;
                // homeController.currentTrucks.Remove(this);
                //
                // homeController.truckCount--;
                // LocalPrefs.SetInt(homeController.levelTruckPref, homeController.currentTrucks.Count);
                //
                // if (homeController.currentTrucksQueue.Contains(this))
                // {
                //     homeController.currentTrucksQueue =
                //         new Queue<TruckController>(homeController.currentTrucksQueue.Where(x => x != this));
                // }
                //
                // truckCrash.DoCrash(other.transform.forward, 4);


                if (truckController.homeController.levelTruckPref == homeController.levelTruckPref ||
                    MergeManager.Instance.isMergeStart || isCrash) return;

                isCrash = true;
                homeController.currentTrucks.Remove(this);

                homeController.truckCount--;

                LocalPrefs.SetInt(homeController.levelTruckPref, homeController.currentTrucks.Count);

                truckCrash.Crash(truckController.transform);
            }

            if (other.CompareTag("PackageControl"))
            {
                if (isBackward && !isTakeAble) return;

                PackageController packageController = other.GetComponentInParent<PackageController>();

                if (isCrash || packageController == null) return;

                if (packageController.isMove || truckColor != packageController.truckColor) return;

                packageController.isMove = true;

                DOVirtual.DelayedCall(0.5f, () => packageController.isMove = false);

                if (truckSo.capacityOfPackage <= currentPackageCount || packageController.numberOfPackage <= 0)
                {
                    fullText.OpenMoneyText();
                    return;
                }

                var packageCount = truckSo.capacityOfPackage - currentPackageCount >= packageController.numberOfPackage
                    ? packageController.numberOfPackage
                    : truckSo.capacityOfPackage - currentPackageCount;

                currentPackageCount += packageCount;

                packageCapacity.SetCapacityText(truckSo.capacityOfPackage, currentPackageCount);

                packageController.parentStack = stackController;
                packageController.targetPositions = stackController.stackList;

                StartCoroutine(packageController.MovePackages(packageCount, this, 0.001f));
            }
        }

        public void CloseSpline()
        {
            Destroy(splineFollower);
            enabled = false;
            transform.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}