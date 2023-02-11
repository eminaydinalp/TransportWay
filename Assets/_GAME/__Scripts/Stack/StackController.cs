using System;
using System.Collections.Generic;
using _GAME.__Scripts.Package;
using _GAME.__Scripts.Truck;
using UnityEngine;


namespace _GAME.__Scripts.Stack
{
    public class StackController : MonoBehaviour
    {
        [SerializeField] private TruckController _truckController;
        public List<Vector3> stackList = new List<Vector3>();
        public List<PackageBox> stackObjects = new List<PackageBox>(); 
        
        public GameObject prefab;
        
        [Header("Stack Transform Information")]
        
        [SerializeField] private float xFirst;
        [SerializeField] private float zFirst;
        [SerializeField] private int columnSize;
        [SerializeField] private int lineSize;
        
        [Range(0, 2)] [SerializeField] private float xGap;
        [Range(0, 2)] [SerializeField] private float zGap;
        
        [SerializeField] private int floorCount; 
        [SerializeField] private float floorOffset; 
        [SerializeField] private float floorRate;

        public bool isEditor;

        private void Start()
        {
            SetInitialValuesStackList();
        }

        private void SetInitialValuesStackList()
        {
            for (int j = 0; j < floorCount; j++)
            {
                for (int i = 0; i < columnSize * lineSize; i++)
                {
                    Vector3 pos = new Vector3(xFirst + (xGap * (i % columnSize)),
                        (j + floorOffset) * floorRate, zFirst + (zGap * (i / columnSize)));
                    
                    stackList.Add( pos);

                    // GameObject clone = Instantiate(prefab);
                    //
                    // clone.transform.SetParent(transform);
                    //
                    // clone.transform.localPosition = pos;
                    //
                    // stackObjects.Add(clone);
                }
            }
        }

        private void OnValidate()
        {
            if (!isEditor)
            {
                return;
            }
            int index = 0;
            for (int j = 0; j < floorCount; j++)
            {
                for (int i = 0; i < columnSize * lineSize; i++)
                {
                    Vector3 pos = new Vector3(xFirst + (xGap * (i % columnSize)),
                        (j + floorOffset) * floorRate, zFirst + (zGap * (i / columnSize)));

                    stackObjects[index].transform.localPosition = pos;

                    index++;
                }
            }
        }

        public void StackReset()
        {
            _truckController.currentPackageCount = 0;
            
            int count = stackObjects.Count;
            
            for (int i = 0; i < count; i++)
            {
                PackageBox destroyedObject = stackObjects[0];
                stackObjects.Remove(destroyedObject);
                Destroy(destroyedObject.gameObject);
            }
            
            stackList.Clear();
            
            SetInitialValuesStackList();

            _truckController.packageCapacity.SetCapacityText(_truckController.truckSo.capacityOfPackage, _truckController.currentPackageCount);
        }
    }
}