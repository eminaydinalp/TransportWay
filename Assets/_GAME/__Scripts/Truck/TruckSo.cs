using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    [CreateAssetMenu(menuName = "TruckSO", fileName = "NewTruck")]
    public class TruckSo : ScriptableObject
    {
        public int capacityOfPackage;
        public int boxMultiplyMoney;

        public int needCarForMerge = 3;
        public int maxAddCar;
    }
}