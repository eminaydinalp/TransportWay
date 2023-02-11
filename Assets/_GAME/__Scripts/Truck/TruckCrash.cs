using System.Collections;
using System.Security.Cryptography;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Rentire.Utils;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class TruckCrash : RMonoBehaviour
    {
        [SerializeField] private TruckController _truckController;
        [SerializeField] private TruckAngle _truckAngle;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private BoxCollider _collider;

        [SerializeField] private float xDistance;
        [SerializeField] private float crashTime;

        private Vector3 _direction;

        private bool _isRight;

        public bool canCrash;

        public void DoCrashWithAngle(Vector3 direction, float force)
        {
            if (UserPrefs.GetTutorial() || !canCrash)
                return;

            _truckAngle.isCrash = false;
            _truckController.isCrash = true;
            gameObject.SetLayer(LayerMask.NameToLayer("TruckNonCollision"));
            StartCoroutine(PackageJump2());

            _direction = direction;
            _truckController.splineFollower.enabled = false;
            _collider.isTrigger = false;
            _rigidbody.isKinematic = false;

            _rigidbody.AddForce(transform.TransformDirection(_direction) * force, ForceMode.Force);
            _rigidbody.velocity = (_direction * force) + (transform.TransformDirection(Vector3.forward));
            _rigidbody.angularVelocity = (_direction * force) + (transform.TransformDirection(Vector3.forward) * .75f);

            FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);

            DOVirtual.DelayedCall(1, ResetCrashAngle);
        }


        private void ResetCrashAngle()
        {
            _truckController.isCrash = false;
            gameObject.SetLayer(LayerMask.NameToLayer("Truck"));
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;

            _truckController.splineFollower.enabled = true;
            _truckController.boxCollider.enabled = true;
            _truckController.packageCapacity.SetCapacityText(_truckController.truckSo.capacityOfPackage, 0);
            _truckController.currentPackageCount = 0;

            LocalPrefs.SetInt(_truckController.homeController.levelTruckPref,
                _truckController.homeController.currentTrucks.Count);

            _truckController.homeController.RemoveFromQueue(_truckController);

            _truckController.homeController.removeSpline.Punch();
        }

        private void ResetCrash()
        {
            _truckController.isCrash = false;
            gameObject.SetLayer(LayerMask.NameToLayer("Truck"));
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;

            _truckController.splineFollower.enabled = true;
            _truckController.boxCollider.enabled = true;
            _truckController.packageCapacity.SetCapacityText(_truckController.truckSo.capacityOfPackage, 0);
            _truckController.currentPackageCount = 0;

            _truckController.stackController.StackReset();

            _truckController.homeController.currentTrucks.Add(_truckController);
            _truckController.homeController.truckCount++;

            LocalPrefs.SetInt(_truckController.homeController.levelTruckPref,
                _truckController.homeController.currentTrucks.Count);

            _truckController.homeController.RemoveFromQueue(_truckController);
        }

        public IEnumerator PackageJump2()
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < _truckController.stackController.stackObjects.Count; i++)
            {
                var block = _truckController.stackController.stackObjects[i];
                block.transform.parent = null;

                Rigidbody rigidbody = block.rigidbody;

                BoxCollider boxCollider = block.boxCollider;

                boxCollider.enabled = true;
                boxCollider.isTrigger = false;

                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;

                rigidbody.velocity = (_direction * 4f) + (transform.TransformDirection(Vector3.forward));
                block.transform.DOScale(0, 1).OnComplete(() =>
                {
                    //block.gameObject.SetActive(false);
                    _truckController.stackController.stackObjects.Remove(block);
                    Destroy(block.gameObject);
                }).SetDelay(1f);

                //rigidbody.angularVelocity = (_direction * 4f) + (transform.TransformDirection(Vector3.forward) * 0.75f);

                // rigidbody.AddForce(_truckController.isBackward ? new Vector3(-5, 1, 0) : new Vector3(5, 1, 0),
                //     ForceMode.Impulse);
            }
        }


        public void Crash(Transform otherTruck)
        {
            if(!canCrash) return;
            _truckController.isCrash = true;
            gameObject.SetLayer(LayerMask.NameToLayer("TruckNonCollision"));
            _isRight = _truckController.transform.position.x > otherTruck.position.x;

            _truckController.splineFollower.enabled = false;
            _truckController.boxCollider.enabled = false;

            xDistance *= _isRight ? 1 : -1;
            PoolManager.Instance.Spawn_Object(PoolsEnum.SmokeExplosion,
                otherTruck.transform.position + Vector3.forward * 0.5f, Quaternion.identity);
            PoolManager.Instance.Spawn_Object(PoolsEnum.SmokeExplosion,
                transform.position + Vector3.forward * 0.5f, Quaternion.identity);

            // _truckController.transform
            //     .DOMove((_truckController.transform.position + (Vector3.right * xDistance * 0.8f)).WithY(0.3f), crashTime)
            //     .OnComplete(ResetCrash);
            //
            // _truckController.transform.DOLocalRotate(
            //     _truckController.transform.localEulerAngles - (Vector3.forward * 180), crashTime / 1.5f);
            var collider = GetComponent<Collider>();
            var rigidBody = GetComponent<Rigidbody>();
            collider.isTrigger = false;
            collider.enabled = true;
            rigidBody.isKinematic = false;
            rigidBody.AddForce(transform.position + (Vector3.right * (xDistance * 40)), ForceMode.Force);

            FeedbackManager.Instance.Vibrate(HapticTypes.LightImpact);
            CallMethodWithDelay(ResetCrash, 1.5f);
            StartCoroutine(PackageJump());
        }

        public IEnumerator PackageJump()
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < _truckController.stackController.stackObjects.Count; i++)
            {
                _truckController.stackController.stackObjects[i].transform.parent = null;

                Rigidbody rigidbody = _truckController.stackController.stackObjects[i].rigidbody;

                BoxCollider boxCollider = _truckController.stackController.stackObjects[i].boxCollider;

                boxCollider.enabled = true;
                boxCollider.isTrigger = false;

                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;


                rigidbody.AddForce(_isRight ? new Vector3(.2f, .2f, 0) : new Vector3(-.2f, .2f, 0),
                    ForceMode.Impulse);
            }
        }
    }
}