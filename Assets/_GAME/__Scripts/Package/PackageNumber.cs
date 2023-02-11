using _GAME.__Scripts.Home;
using DG.Tweening;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Package
{
    public class PackageNumber : MonoBehaviour
    {
        public TMP_Text packageText;
        public PackageController packageController;
        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;

            if (packageController == null)
            {
                packageController = GetComponentInParent<PackageController>();
            }
        }
        
        private void Start()
        {
            if (UserPrefs.GetMaxHomeLevel() > 0)
            {
                transform.DOScale(transform.localScale + ((Vector3.one * UserPrefs.GetMaxHomeLevel()) * 0.2f), 0.1f);
            }
        }

        private void OnEnable()
        {
            HomeLockManager.OnHomeUnlocked += ScaleUpPackageNumber;
        }

        private void OnDisable()
        {
            HomeLockManager.OnHomeUnlocked -= ScaleUpPackageNumber;
        }

        private void ScaleUpPackageNumber()
        {
            transform.DOScale(transform.localScale + (Vector3.one * 0.2f), 0.1f);
        }


        private void OnValidate()
        {
            packageController = GetComponentInParent<PackageController>();
        }

        private void Update()
        {
            transform.LookAt(this.transform.position + _camera.transform.rotation * Vector3.forward,_camera.transform.rotation * Vector3.up);
            packageText.text = packageController.numberOfPackage.ToString();
        }
    }
}