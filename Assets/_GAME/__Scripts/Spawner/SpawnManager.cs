using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Package;
using _GAME.__Scripts.TargetHome;
using DG.Tweening;
using Rentire.Core;

namespace _GAME.__Scripts.Spawner
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public RandomSpawner randomSpawner;
        
        public List<PackageController> packageControllers;
        
        public List<PowerUpBase> plusObjects;

        public List<HomeController> homeControllers;
        public List<TargetHomeController> targetHomeControllers;

        public float spawnYPos;
        
        public float xMin;
        public float xMax;
        
        public float zMin;
        public float zMax;
        
        public float distanceSpline;
        public float distancePackage;


        private HomeController _homeController;

        private bool _isHome;

        private void Start()
        {
            DOVirtual.DelayedCall(0.2f, SetHome);
        }


        private void SetHome()
        {
            _isHome = true;

            homeControllers = FindObjectsOfType<HomeController>().ToList();
            
            foreach (var homeController in homeControllers)
            {
                if (_homeController == null) _homeController = homeController;

                if (homeController.levelNo > _homeController.levelNo)
                {
                    _homeController = homeController;
                }
            }

            targetHomeControllers = FindObjectsOfType<TargetHomeController>().ToList();
            
            SetSpawnValues(_homeController.packageSpawner.xMin, 
                _homeController.packageSpawner.xMax, 
                _homeController.packageSpawner.zMin, 
                _homeController.packageSpawner.zMax);
        }

        public void SetSpawnValues(float xMinValue, float xMaxValue, float zMinValue, float zMaxValue)
        {
            if(!_isHome) return;
            
            xMin = xMinValue;
            xMax = xMaxValue;

            zMin = zMinValue;
            zMax = zMaxValue;
        }
    }
}